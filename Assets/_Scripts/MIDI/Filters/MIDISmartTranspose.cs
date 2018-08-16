using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDISmartTranspose : MIDITrackFilter {

	public TransposeRules transposeRules;

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		
		foreach(var midiEvent in events) {
			// pitch is parameter 1
			int pitch = midiEvent.parameter1;
			var pitchShift = transposeRules.GetPitchShiftForPitch(pitch);
			pitch = pitch + pitchShift;

			var newMidiEvent = midiEvent.Duplicate();
			newMidiEvent.parameter1 = (byte)pitch;
			filteredEvents.Add(newMidiEvent);
		}
		return filteredEvents.ToArray();
	}
}
