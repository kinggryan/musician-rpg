using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDIInstrumentChangeFilter : MIDITrackFilter {

	public int inputChannel = 1;
	public int outputChannel = 3;

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		foreach(var midiEvent in events) {
			var newMidiEvent = midiEvent.Duplicate();

			if (midiEvent.channel == inputChannel){
				newMidiEvent.channel = (byte)outputChannel;
			}
			filteredEvents.Add(newMidiEvent);
		}

		return filteredEvents.ToArray();
	}
}
