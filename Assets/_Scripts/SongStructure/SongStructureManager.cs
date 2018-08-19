using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SongStructureManager : MonoBehaviour {

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

	[HideInInspector]
	public double bpm;
	public string songFilename;

	protected SongSection[] songSections;

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

	//-- Public functions

	/// <summary>
	/// The base method should always be called
	/// </summary>
	public virtual void StartSong() {
		isSongPlaying = true;
		QueueNextSongPhrase();
		BroadcastMessage("DidStartSong",SendMessageOptions.DontRequireReceiver);
	}

	//-- Protected Functions
	protected virtual void Start() {
		songSections = SongFileReader.ReadSongFile(songFilename);
	}

	protected virtual void QueueNextSongPhrase() {

		SongPhrase nextPhrase;
		var phraseStartBeat = currentSongPhraseEndBeat;

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
			nextPhrase = nullPhrase;
		} else {
			// Otherwise, return the correct loop
			nextPhrase = songSections[currentSongSectionIndex].phrases[currentSongPhraseIndex];
		}

		// Song is done if outside range
		if(nextPhrase == nullPhrase) {
			Debug.Log("Finished song!");
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
			// Play the next phrase
			if(currentSongBeat == currentSongPhraseEndBeat - 1)
				QueueNextSongPhrase();

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
		return new PhraseOffsetTuple(nullPhrase, beat);
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

	private string GetSongRhythmStringForBeat(double beat) {
		var phraseOffsetTuple = GetSongPhraseForBeat(beat);
		// For each beat into the offset, we want to return a different 2-eighth note string
		// TODO: Make this way less gnarly
		var fullRhythmString = phraseOffsetTuple.phrase.loop.rhythmString;
		var beatOffsetIndex = Mathf.RoundToInt((float)phraseOffsetTuple.beatOffset) % phraseOffsetTuple.phrase.loop.beatDuration;
		return "" + fullRhythmString[2*beatOffsetIndex] + fullRhythmString[2*beatOffsetIndex+1];
	}
}
