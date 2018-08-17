using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiSongStructureManager : SongStructureManager {
	public MIDISongPlayer songPlayer;

	MIDISmartTranspose transposeFilter;

	protected override void Start() {
		// Create and add the transposition filter to the midi player
		base.Start();
		transposeFilter = new MIDISmartTranspose();
		songPlayer.AddFilterToMainMidiStreamerGroup(transposeFilter);
	}

	override protected void QueueSongPhrase(SongPhrase phrase, double atBeat) {
		// Tell the transpose filter to start transposing at the given sample time
		var sampleTimeForPhrase = GetSampleTimeForBeat(atBeat);
		var transposeRules = TransposeRules.RulesForChord(phrase.chord);
		transposeFilter.AddTransposeRule(transposeRules, sampleTimeForPhrase);
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
