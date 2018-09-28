using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SongStructureManager : MonoBehaviour {

	public static class Notifications {
		public static string didStartSong = "didStartSong";
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

	[HideInInspector]
	public double bpm;
	public string songFilename;

	public SongSection[] songSections { get; protected set; }

	/// <summary>
	/// The current beat of the song being played
	/// </summary>
	protected double currentSongBeat;
	/// <summary>
	/// The index of the current song section in the songSections array
	/// </summary>
	protected int currentSongSectionIndex = 0;
	protected double currentSongPhraseEndBeat;
	protected int currentSongPhraseIndex = -1;
	protected int currentSongPhraseNumRepeatsRemaining = 0;

	protected bool isSongPlaying = false;
	protected List<ISongUpdateListener> songUpdateListeners = new List<ISongUpdateListener>();

	private int endBeat {
		get {
			var beat = 0;
			foreach(var section in songSections) {
				foreach(var phrase in section.phrases) {
					beat += phrase.TotalBeatLength();
				}
			}

			return beat;
		}
	}

	//-- Public functions

	/// <summary>
	/// The base method should always be called
	/// </summary>
	public virtual void StartSong() {
		isSongPlaying = true;
		QueueNextSongPhrase();
		BroadcastMessage("DidStartSong",SendMessageOptions.DontRequireReceiver);
		NotificationBoard.SendNotification(Notifications.didStartSong, this, null);
	}

	public void RegisterSongUpdateListener(ISongUpdateListener listener) {
		songUpdateListeners.Add(listener);
	}

	//-- Protected Functions
	protected virtual void Awake() {
		// Do this during Awake because other classes may need to  know the song structure to initialize themselves
		var savedSongFileName = MusicalEncounterManager.GetCurrentMusicalEncounterSongFile();
		if(savedSongFileName != "" && savedSongFileName != null)
			songFilename = savedSongFileName;

		songSections = SongFileReader.ReadSongFile(songFilename);
	}

	protected virtual void QueueNextSongPhrase() {

		SongPhrase nextPhrase;
		var phraseStartBeat = currentSongPhraseEndBeat - 1;

		// If we're repeating this phrase, keep repeating it
		if(currentSongPhraseNumRepeatsRemaining > 0) {
			currentSongPhraseNumRepeatsRemaining--;
		} else {
			currentSongPhraseIndex++;
			if(currentSongPhraseIndex >= songSections[currentSongSectionIndex].phrases.Length) {
				currentSongPhraseIndex = 0;
				currentSongSectionIndex++;
			}
			if(currentSongSectionIndex < songSections.Length) {
				currentSongPhraseNumRepeatsRemaining = songSections[currentSongSectionIndex].phrases[currentSongPhraseIndex].numTimesToPlay - 1;
			}
		}

		// If past the end of the song, return the null phrase
		if(currentSongSectionIndex >= songSections.Length) {
			nextPhrase = null;
		} else {
			// Otherwise, return the correct loop
			nextPhrase = songSections[currentSongSectionIndex].phrases[currentSongPhraseIndex];
		}

		// Song is done if outside range
		if(nextPhrase == null) {
			EndSong();
		} else {
			// Continue to the next phrase
			currentSongPhraseEndBeat += nextPhrase.singlePlaythroughBeatLength;
			QueueSongPhrase(nextPhrase, phraseStartBeat);
		}
	}

	/// <summary>
	/// This method handles the actual song playback side logic of how to queue up a new song phrase
	/// </summary>
	protected abstract void QueueSongPhrase(SongPhrase phrase, double atBeat);
	/// <summary>
	/// This method should return the current beat as the song is being played.
	/// </summary>
	protected abstract double GetCurrentBeat();

	protected abstract void EndSong();

	//- Private Methods

	// Update is called once per frame
	private void Update () {
		if(isSongPlaying) {
			CheckBeatTransition();
		}
	}

	private void CheckBeatTransition() {
		// If we incremented to the next beat
		var previousBeat = currentSongBeat;
		currentSongBeat = GetCurrentBeat();

		if(currentSongBeat > previousBeat) {
			if(currentSongBeat == endBeat) {
				EndSong();
			}

			// Play the next phrase
			if(currentSongBeat == currentSongPhraseEndBeat - 1)
				QueueNextSongPhrase();

			// Broadcast this message
			var beatUpdateInfo = new BeatUpdateInfo();
			beatUpdateInfo.currentBeat = Mathf.RoundToInt((float)currentSongBeat);
			if(currentSongSectionIndex < songSections.Length) {
				beatUpdateInfo.currentSection = songSections[currentSongSectionIndex];
			}
			if(currentSongSectionIndex + 1 < songSections.Length) {
				beatUpdateInfo.nextSection = songSections[currentSongSectionIndex+1];
				beatUpdateInfo.beatsUntilNextSection = GetStartBeatForSectionIndex(currentSongSectionIndex+1) - Mathf.RoundToInt((float)currentSongBeat);
			}
			
			foreach(var listener in songUpdateListeners) {
				listener.DidStartNextBeat(beatUpdateInfo);
			}
		}
	}

	// MARK: Utilities

	// Returns the chord that should be played on a given beat
	private Chord GetChordForBeat(double beat) {
		return GetSongPhraseForBeat(beat).phrase.chord;
	}

	// Returns the song phrase that should be playing on a given beat. 
	// If the beat is the border between two phrases, will return the latter of the two.
	private PhraseOffsetTuple GetSongPhraseForBeat(double beat) {
		var checkBeat = 0;
		foreach(var section in songSections) {
			foreach(var phrase in section.phrases) {
				var endOfPhraseBeat = checkBeat + phrase.TotalBeatLength();
				if(beat < endOfPhraseBeat) {
					return new PhraseOffsetTuple(phrase, beat - checkBeat);
				}
				checkBeat = endOfPhraseBeat;
			}
		}
		
		Debug.LogError("Tried to find the song phrase for beat " + beat + " but it was past the end of the song.");
		return new PhraseOffsetTuple(null, beat);
	}

	private int GetStartBeatForSectionIndex(int index) {
		var numBeats = 0;
		for(var i = 0 ; i < index ; i++) {
			if(i >= songSections.Length) {
				break;
			}
			foreach(var phrase in songSections[i].phrases) {
				numBeats += phrase.TotalBeatLength();
			}
		}

		return numBeats;
	}

	private RhythmString GetSongRhythmStringForBeat(double beat) {
		var phraseOffsetTuple = GetSongPhraseForBeat(beat);
		return phraseOffsetTuple.phrase.loop.GetRhythmStringForBeat(Mathf.FloorToInt((float)beat));
	}
}
