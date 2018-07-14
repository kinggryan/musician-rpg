using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SongPlayer : MonoBehaviour {

	[System.Serializable]
	public struct SongPhrase {
		public AudioLoop loop;
		public AudioLoop.Chord chord;
		public int numTimesToPlay;

		public SongPhrase(string loopName, string chordName, int numTimesToPlay) {
			loop = new AudioLoop(AudioLoop.LoopNameForString(loopName));
			chord = AudioLoop.ChordForString(chordName);
			this.numTimesToPlay = numTimesToPlay;
		}

		public static bool operator== (SongPhrase lhs, SongPhrase rhs) {
			return lhs.loop == rhs.loop && lhs.chord == rhs.chord && lhs.numTimesToPlay == rhs.numTimesToPlay;
		}

		public static bool operator!= (SongPhrase lhs, SongPhrase rhs) {
			return !(lhs.loop == rhs.loop && lhs.chord == rhs.chord && lhs.numTimesToPlay == rhs.numTimesToPlay);
		}
	}

	[System.Serializable]
	public struct SongSection {
		public string name;
		public SongPhrase[] phrases;
	}

	public struct BeatUpdateInfo {
		public int currentBeat;
		public SongSection currentSection;
		public int beatsUntilNextSection;
		public SongSection nextSection;
	}

	struct PhraseOffsetTuple {
		public readonly SongPhrase phrase;
		public readonly double beatOffset;

		public PhraseOffsetTuple(SongPhrase phrase, double beatOffset) {
			this.phrase = phrase;
			this.beatOffset = beatOffset;
		}
	}

	// This phrase is used to signify the lack of a phrase, e.g. the end of the song
	static SongPhrase nullPhrase = new SongPhrase();

	public SongGameManager gameManager;
	public CharacterAnimationManager characters;

	private double songStartDSPTime;
	public double bpm;
	public AudioLoop[] playerAudioLoops;
	public string songFilename;

	private SongSection[] songSections;
	public SoundEvent soundEvent;

	public double currentPlayerLoopEndBeat = -1;
	public int currentPlayerLoopIndex;
	
	public double nextPlayerLoopEndBeat;
	public int nextPlayerLoopIndex;
	/// Save the next player clip so that we can cancel it's playback when the PC changes which loop they are going to play
	public AudioMixerGroup playerOutput;
	
	public AudioMixerGroup npcOutput;
	private AudioSource nextPlayerAudioSource;	

	private double currentSongPhraseEndBeat;
	private double currentSongBeat;

	private int currentSongSectionIndex = 0;
	private int currentSongPhraseIndex = -1;
	private int currentSongPhraseNumRepeatsRemaining = 0;

	private bool songStarted = false;

	void Start() {
		// Initialize the song and start the countoff
		songSections = SongFileReader.ReadSongFile(songFilename);
		Invoke("StartCountoff",2);
	}

	// Update is called once per frame
	void Update () {
		if(songStarted) {
			// If we incremented to the next beat
			var previousBeat = currentSongBeat;
			currentSongBeat = GetCurrentBeat();
			// Debug.Log("Cur beat " + currentSongBeat + " current song phrase end beat " + currentSongPhraseEndBeat);
			if(currentSongBeat > previousBeat) {
				// Continue The song player
				if(currentSongBeat == currentSongPhraseEndBeat - 1)
					PlayNextPhrase();

				// Set up the next player's loop
				Debug.Log("Current beat : " + currentSongBeat + " end beat: " + currentPlayerLoopEndBeat);
				if(currentSongBeat == currentPlayerLoopEndBeat)
					ProceedToNextPlayerLoop();

				// If we actually transitioned to a new player, broadcast a message to tell other elements to update
				// The current player loop end beat is <= 0 when the song hasn't started
				// TODO: Add a utility method for this
				if(currentPlayerLoopEndBeat >= 0) {
					BroadcastMessage("DidPlayPlayerTrack", currentPlayerLoopIndex, SendMessageOptions.DontRequireReceiver);
					// Tell the game manager what the current song's rhythm track is
					var currentBeat = GetCurrentBeat();
					gameManager.currentSongRhythmString = GetSongRhythmStringForBeat(currentBeat);
					gameManager.currentPlayerRhythmString = GetCurrentPlayerRhythmString();
				}

				// Broadcast this message
				var beatUpdateInfo = new BeatUpdateInfo();
				beatUpdateInfo.currentBeat = Mathf.RoundToInt((float)currentSongBeat);
				beatUpdateInfo.currentSection = songSections[currentSongSectionIndex];
				if(currentSongSectionIndex + 1 < songSections.Length) {
					beatUpdateInfo.nextSection = songSections[currentSongSectionIndex+1];
					beatUpdateInfo.beatsUntilNextSection = GetStartBeatForSectionIndex(currentSongSectionIndex+1) - Mathf.RoundToInt((float)currentSongBeat);
				}
				
				BroadcastMessage("DidStartNextBeat", beatUpdateInfo, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// MARK: Song Loops

	public void StartCountoff() {
		var numCountoffBeats = 4;
		Invoke("StartSong", (float)(numCountoffBeats*60/bpm));

		BroadcastMessage("DidStartCountoff",(float)bpm,SendMessageOptions.DontRequireReceiver);
	}

	public void StartSong() {
		songStartDSPTime = AudioSettings.dspTime;
		songStarted = true;
		gameManager.songStarted = true;
		characters.DidStartSong();
		PlayNextPhrase();
		// HACK:? 
		PlayNextPhrase();

		// Also kind of a hack
		if(currentPlayerLoopIndex >= 0) {
			ContinuePlayerLoop();
			ProceedToNextPlayerLoop();
		}


		BroadcastMessage("DidStartSong",SendMessageOptions.DontRequireReceiver);
	}

	void PlayNextPhrase() {

		var nextPhrase = GetNextPhrase();

		// Song is done if outside range
		if(nextPhrase == nullPhrase) {
			Debug.Log("Finished song!");
		} else {
			// Continue to the next phrase
			var nextPhraseStartDSPTime = ConvertBeatToDSPTime(currentSongPhraseEndBeat);
			currentSongPhraseEndBeat += nextPhrase.loop.numBeats;
			// Debug.Log("Playing phrase "+ nextPhrase + " on beat " + currentSongPhraseEndBeat);
			nextPhrase.loop.PlayLoop(nextPhraseStartDSPTime, nextPhrase.chord, soundEvent, npcOutput);
		}
	}

	SongPhrase GetNextPhrase() {
		// If we're repeating this phrase, keep repeating it
		if(currentSongPhraseNumRepeatsRemaining > 0) {
			currentSongPhraseNumRepeatsRemaining--;
		} else {
			currentSongPhraseIndex++;
			if(currentSongPhraseIndex >= songSections[currentSongSectionIndex].phrases.Length) {
				currentSongPhraseIndex = 0;
				currentSongSectionIndex++;
			}
			currentSongPhraseNumRepeatsRemaining = songSections[currentSongSectionIndex].phrases[currentSongPhraseIndex].numTimesToPlay - 1;
		}

		// If past the end of the song, return the null phrase
		if(currentSongSectionIndex >= songSections.Length) {
			return nullPhrase;
		}

		// Otherwise, return the correct loop
		return songSections[currentSongSectionIndex].phrases[currentSongPhraseIndex];
	}

	// MARK: Player Loops
	
	// Continues looping the current player loop
	void ContinuePlayerLoop() {
		Debug.Log("Continuing player loop");
		nextPlayerLoopIndex = currentPlayerLoopIndex;
		var nextPhraseStartDSPTime = ConvertBeatToDSPTime(currentPlayerLoopEndBeat);
		nextPlayerLoopEndBeat = currentPlayerLoopEndBeat + playerAudioLoops[nextPlayerLoopIndex].numBeats;
		nextPlayerAudioSource = playerAudioLoops[nextPlayerLoopIndex].PlayLoop(nextPhraseStartDSPTime, GetChordForBeat(currentPlayerLoopEndBeat), soundEvent, playerOutput);
	}

	public void ChangePlayerLoop(int loopIndex) {
		if(songStarted) {
			// We want to start this player loop at the next beat
			var beatToStartAt = currentPlayerLoopEndBeat > 0 ? currentPlayerLoopEndBeat : currentSongPhraseEndBeat;
			// If not playing any player loops yet, set the current song phrase end beat
			if(currentPlayerLoopEndBeat <= 0)
				currentPlayerLoopEndBeat = currentSongPhraseEndBeat;

			var beatToStartAtDSPTime = ConvertBeatToDSPTime(beatToStartAt);
			// Cancel the next clip
			if(nextPlayerAudioSource)
				nextPlayerAudioSource.Stop();
			nextPlayerAudioSource = playerAudioLoops[loopIndex].PlayLoop(beatToStartAtDSPTime, GetChordForBeat(beatToStartAt), soundEvent, playerOutput);
			nextPlayerLoopEndBeat = beatToStartAt + playerAudioLoops[loopIndex].numBeats;
			nextPlayerLoopIndex = loopIndex;
			BroadcastMessage("DidQueuePlayerTrack", nextPlayerLoopIndex, SendMessageOptions.DontRequireReceiver);
		} else {
			currentPlayerLoopIndex = loopIndex;
			nextPlayerLoopIndex = loopIndex;
			currentPlayerLoopEndBeat = 0;
			nextPlayerLoopEndBeat = playerAudioLoops[loopIndex].numBeats;
			BroadcastMessage("DidQueuePlayerTrack", loopIndex, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void StopPlayerLoops() {
		// Set teh player loop end beat to 0
		currentPlayerLoopEndBeat = -1;
		BroadcastMessage("DidStopPlayingTracks", SendMessageOptions.DontRequireReceiver);
	}

	void ProceedToNextPlayerLoop() {
		currentPlayerLoopIndex = nextPlayerLoopIndex;
		currentPlayerLoopEndBeat = nextPlayerLoopEndBeat;
		gameManager.currentPlayerRhythmString = playerAudioLoops[currentPlayerLoopIndex].GetRhythmString();
		ContinuePlayerLoop();
	}

	// MARK: Utilities

	double GetCurrentBeat() {
		return System.Math.Floor(ConvertDSPTimeToBeat(AudioSettings.dspTime));
	}

	double ConvertDSPTimeToBeat(double dspTime) {
		return bpm / 60 * (dspTime - songStartDSPTime);
	}

	double ConvertBeatToDSPTime(double beat) {
		return (beat / bpm * 60) + songStartDSPTime;
	}

	// Returns the chord that should be played on a given beat
	AudioLoop.Chord GetChordForBeat(double beat) {
		return GetSongPhraseForBeat(beat).phrase.chord;
	}

	// Returns the song phrase that should be playing on a given beat. 
	// If the beat is the border between two phrases, will return the latter of the two.
	PhraseOffsetTuple GetSongPhraseForBeat(double beat) {
		var checkBeat = 0;
		foreach(var section in songSections) {
			foreach(var phrase in section.phrases) {
				var endOfPhraseBeat = checkBeat + phrase.numTimesToPlay*phrase.loop.numBeats;
				if(beat < endOfPhraseBeat) {
					return new PhraseOffsetTuple(phrase, beat - checkBeat);
				}
				checkBeat = endOfPhraseBeat;
			}
		}
		
		Debug.LogError("Tried to find the song phrase for beat " + beat + " but it was past the end of the song.");
		return new PhraseOffsetTuple(nullPhrase, beat);
	}

	int GetStartBeatForSectionIndex(int index) {
		var numBeats = 0;
		for(var i = 0 ; i < index ; i++) {
			if(i >= songSections.Length) {
				break;
			}
			foreach(var phrase in songSections[i].phrases) {
				numBeats += phrase.numTimesToPlay*phrase.loop.numBeats;
			}
		}

		return numBeats;
	}

	string GetSongRhythmStringForBeat(double beat) {
		var phraseOffsetTuple = GetSongPhraseForBeat(beat);
		// For each beat into the offset, we want to return a different 2-eighth note string
		// TODO: Make this way less gnarly
		var fullRhythmString = phraseOffsetTuple.phrase.loop.GetRhythmString();
		var beatOffsetIndex = Mathf.RoundToInt((float)phraseOffsetTuple.beatOffset) % phraseOffsetTuple.phrase.loop.numBeats;
		return "" + fullRhythmString[2*beatOffsetIndex] + fullRhythmString[2*beatOffsetIndex+1];
	}

	string GetCurrentPlayerRhythmString() {
		var currentPlayerLoopStartBeat = currentPlayerLoopEndBeat - playerAudioLoops[currentPlayerLoopIndex].numBeats;
		var beatOffsetIndex = Mathf.RoundToInt((float)(GetCurrentBeat() - currentPlayerLoopStartBeat));
		var fullRhythmString = playerAudioLoops[currentPlayerLoopIndex].GetRhythmString();
		return "" + fullRhythmString[2*beatOffsetIndex] + fullRhythmString[2*beatOffsetIndex+1];
	}
}
