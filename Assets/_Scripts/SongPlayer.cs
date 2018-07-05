using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour {

	[System.Serializable]
	public struct SongPhrase {
		public AudioLoop loop;
		public AudioLoop.Chord chord;
		public int numTimesToPlay;
	}

	private double songStartDSPTime;
	public double bpm;
	public AudioLoop[] playerAudioLoops;
	public SongPhrase[] songPhrases;
	public SoundEvent soundEvent;

	public double currentPlayerLoopEndBeat;
	public int currentPlayerLoopIndex;
	
	private double currentSongPhraseEndBeat;
	private double currentSongBeat;
	private int currentSongPhraseIndex = -1;
	private int currentSongPhraseNumRepeatsRemaining = 0;

	// Update is called once per frame
	void Update () {
		// If we incremented to the next beat
		var previousBeat = currentSongBeat;
		currentSongBeat = GetCurrentBeat();
		// Debug.Log("Cur beat " + currentSongBeat + " current song phrase end beat " + currentSongPhraseEndBeat);
		if(currentSongBeat > previousBeat) {
			// Continue The song player
			if(currentSongBeat == currentSongPhraseEndBeat - 1)
				PlayNextPhrase();

			// Set up the next player's loop
			if(currentSongBeat == currentPlayerLoopEndBeat - 1)
				ContinuePlayerLoop();

			// If we actually transitioned to a new player, broadcast a message to tell other elements to update
			// if(currentSongBeat == currentPlayerLoopEndBeat)
			BroadcastMessage("DidPlayPlayerTrack", currentPlayerLoopIndex, SendMessageOptions.DontRequireReceiver);
		}
	}

	// MARK: Song Loops

	public void StartSong() {
		songStartDSPTime = AudioSettings.dspTime;
		PlayNextPhrase();
	}

	void PlayNextPhrase() {
		// Keep looping this loop if we should repeat it more, otherwise go to the next loop
		if(currentSongPhraseNumRepeatsRemaining > 0) {
			currentSongPhraseNumRepeatsRemaining--;
		} else {
			currentSongPhraseIndex++;
			if(currentSongPhraseIndex >= songPhrases.Length)
				currentSongPhraseNumRepeatsRemaining = songPhrases[currentSongPhraseIndex].numTimesToPlay - 1;
		}

		// Song is done if outside range
		if(currentSongPhraseIndex >= songPhrases.Length) {
			Debug.Log("Finished song!");
		} else {
			// Continue to the next phrase
			var nextPhrase = songPhrases[currentSongPhraseIndex];
			var nextPhraseStartDSPTime = ConvertBeatToDSPTime(currentSongPhraseEndBeat);
			currentSongPhraseEndBeat += nextPhrase.loop.numBeats;
			Debug.Log("Playing phrase "+ nextPhrase + " on beat " + currentSongPhraseEndBeat);
			nextPhrase.loop.PlayLoop(nextPhraseStartDSPTime, nextPhrase.chord, soundEvent);
		}
	}

	// MARK: Player Loops
	
	// Continues looping the current player loop
	void ContinuePlayerLoop() {
		var nextPhraseStartDSPTime = ConvertBeatToDSPTime(currentPlayerLoopEndBeat);
		currentPlayerLoopEndBeat += playerAudioLoops[currentPlayerLoopIndex].numBeats;
		// Debug.Log("Playing phrase "+ nextPhrase + " on beat " + currentSongPhraseEndBeat);
		playerAudioLoops[currentPlayerLoopIndex].PlayLoop(nextPhraseStartDSPTime, GetChordForBeat(currentPlayerLoopEndBeat), soundEvent);
	}

	public void ChangePlayerLoop(int loopIndex) {
		// We want to start this player loop at the next beat
		var beatToStartAt = currentPlayerLoopEndBeat > 0 ? currentPlayerLoopEndBeat : currentSongPhraseEndBeat;
		var beatToStartAtDSPTime = ConvertBeatToDSPTime(beatToStartAt);
		playerAudioLoops[loopIndex].PlayLoop(beatToStartAtDSPTime, GetChordForBeat(beatToStartAt), soundEvent);
		currentPlayerLoopEndBeat = beatToStartAt + playerAudioLoops[loopIndex].numBeats;
		currentPlayerLoopIndex = loopIndex;
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
		return GetSongPhraseForBeat(beat).chord;
	}

	// Returns the song phrase that should be playing on a given beat. 
	// If the beat is the border between two phrases, will return the latter of the two.
	SongPhrase GetSongPhraseForBeat(double beat) {
		var checkBeat = 0;
		foreach(var phrase in songPhrases) {
			var endOfPhraseBeat = checkBeat + phrase.numTimesToPlay*phrase.loop.numBeats;
			if(beat < endOfPhraseBeat)
				return phrase;
			checkBeat = endOfPhraseBeat;
		}

		Debug.LogError("Tried to find the song phrase for beat " + beat + " but it was past the end of the song.");
		return songPhrases[songPhrases.Length-1];
	}
}
