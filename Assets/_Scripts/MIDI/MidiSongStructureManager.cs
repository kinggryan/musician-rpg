using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiSongStructureManager : SongStructureManager {
	public MIDISongPlayer songPlayer;

	override protected void QueueSongPhrase(SongPhrase phrase) {
		// When we queue the song phrase
		// We are going to want to transition between chords at the sample time that that phrase starts
	}

	override protected double GetCurrentBeat() {
		return GetBeatForSampleTime(songPlayer.midiSequencer.SampleTime);
	}

	private double GetBeatForSampleTime(int sampleTime) {
		return Mathf.Floor((float)(sampleTime * 1f / songPlayer.midiStreamSynthesizer.SampleRate / 60f * bpm));
	}

	private int GetSampleTimeForBeat(double beat) {
		return Mathf.FloorToInt((float)(beat / bpm * 60 * songPlayer.midiStreamSynthesizer.SampleRate));
	}
}
