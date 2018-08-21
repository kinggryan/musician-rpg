using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiSongStructureManager : SongStructureManager {
	public MIDISongPlayer songPlayer;

	MIDISmartTranspose transposeFilter;

	const double songBPM = 240;

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
		// NOTE: We use a static songBPM here because the sampleTime from the stream synthesizer is stretched to adjust for dynamic BPM already
		return Mathf.Floor((float)(sampleTime * 1f / songPlayer.midiStreamSynthesizer.SampleRate / 60f * songBPM));
	}

	private int GetSampleTimeForBeat(double beat) {
		// NOTE: We use a static songBPM here because the sampleTime from the stream synthesizer is stretched to adjust for dynamic BPM already
		return Mathf.FloorToInt((float)(beat / songBPM * 60 * songPlayer.midiStreamSynthesizer.SampleRate));
	}
}
