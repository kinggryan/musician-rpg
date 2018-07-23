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
			if (midiEvent.channel == playerChannelNumber){
				if(midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On) {
					newMidiEvent.parameter2 = (byte)Mathf.FloorToInt(volumeMultiplier*(int)newMidiEvent.parameter2);
				}
			}
			filteredEvents.Add(newMidiEvent);
		}

		return filteredEvents.ToArray();
	}
}
