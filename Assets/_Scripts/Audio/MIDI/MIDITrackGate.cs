using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDITrackGate : MIDITrackFilter {
	public int gateVelocity;

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		foreach(var midiEvent in events) {
			var newMidiEvent = midiEvent.Duplicate();
			// Velocity is parameter 2
			int velocity = midiEvent.parameter2;
			if(midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On && velocity < gateVelocity) {
				newMidiEvent.parameter2 = 0;
			}

			filteredEvents.Add(newMidiEvent);
		}

		return filteredEvents.ToArray();
	}
}
