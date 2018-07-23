using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public abstract class MIDITrackFilter {

	public const int playerChannelNumber = 0;

	// Use this for initialization
	public abstract MidiEvent[] FilterMidiEvents(MidiEvent[] events);
}
