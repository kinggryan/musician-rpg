using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDITrackGate : MIDITrackFilter {
	struct MidiNoteInfo {
		public int pitch;
		public int channel;
		public MidiNoteInfo(int pitch, int channel) {
			this.pitch = pitch;
			this.channel = channel;
		}
		public override int GetHashCode() {
			// Since I really doubt there will be over 1000 channels, this is a safe hash function
			return pitch * 1000 + channel;
		}
		public override string ToString() {
			return "" + pitch + ":" + channel;
		}
	}

	public int gateVelocity;

	private Dictionary<MidiNoteInfo,int> ongoingNotesCount = new Dictionary<MidiNoteInfo,int>();

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		foreach(var midiEvent in events) {
			var newMidiEvent = midiEvent.Duplicate();
			int velocity = midiEvent.parameter2;

			// Velocity is parameter 2
			if (midiEvent.channel == activeChannel){
				if(midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On) {
					// If we should let it through, let it through
					if(velocity >= gateVelocity) {
						filteredEvents.Add(newMidiEvent);
						var noteInfo = new MidiNoteInfo(midiEvent.parameter1, midiEvent.channel);
						if(ongoingNotesCount.ContainsKey(noteInfo)) {
							ongoingNotesCount[noteInfo]++;
						} else {
							ongoingNotesCount[noteInfo] = 1;
						}
					}
				} else if (midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off) {
					// We want to check to see if there is an entry for htis note in teh ongoing notes and if so, let the note through
					var noteInfo = new MidiNoteInfo(midiEvent.parameter1,midiEvent.channel);
					if(ongoingNotesCount.ContainsKey(noteInfo) && ongoingNotesCount[noteInfo] > 0) {
						ongoingNotesCount[noteInfo]--;
						filteredEvents.Add(newMidiEvent);
					}
				} else {
					filteredEvents.Add(newMidiEvent);
				}
			} else {
				filteredEvents.Add(newMidiEvent);
			}
		}

		return filteredEvents.ToArray();
	}
}
