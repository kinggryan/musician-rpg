using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDIVolumeFilter : MIDITrackFilter {
	public float volumeMultiplier;

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		foreach(var midiEvent in events) {
			var newMidiEvent = midiEvent.Duplicate();
			// Velocity is parameter 2
			int velocity = midiEvent.parameter2;
			if (midiEvent.channel == activeChannel){
				if(midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On) {
					newMidiEvent.parameter2 = (byte)Mathf.FloorToInt(volumeMultiplier*velocity);
				}
			}
			filteredEvents.Add(newMidiEvent);
		}

		return filteredEvents.ToArray();
	}
}
