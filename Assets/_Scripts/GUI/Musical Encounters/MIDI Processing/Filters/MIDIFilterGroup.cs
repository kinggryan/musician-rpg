using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

// This filter takes a group of other filters and applies them all to a series of events
public class MIDIFilterGroup : MIDITrackFilter {

	private List<MIDITrackFilter> filters = new List<MIDITrackFilter>();

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var retEvents = events;
		foreach(var filter in filters) {
			retEvents = filter.FilterMidiEvents(retEvents);
		}
		return retEvents;
	}

	public void AddFilter(MIDITrackFilter filter) {
		filters.Add(filter);
	}
}
