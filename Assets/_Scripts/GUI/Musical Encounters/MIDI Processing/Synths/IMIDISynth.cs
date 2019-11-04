using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicianRPG;

public interface IMIDISynth {

	int SampleRate { get; }
	int InstrumentCount { get; }
	int DrumCount { get; }

	// Use this for initialization
	void setSequencer(MidiSequencer midiSequencer);

	void SetPitchBend(int channel,double amount);
	void SetPan(int channel, float amount);
	void SetVolume(int channel, float amount);

	void NoteOn(int channel, int note, int velocity, int program);
	void NoteOff(int channel, int note);
	void NoteOffAll(bool immediate);
	void ResetPanAndVolume();

	void Stop();
}
