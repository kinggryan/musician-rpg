using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDIMonophonicFilter : MIDITrackFilter {

	private struct MidiNoteInfo {
		public int channelNumber;
		public int pitch;
		public MidiNoteInfo(int channelNumber, int pitch) {
			this.channelNumber = channelNumber;
			this.pitch = pitch;
		}
	}

	private List<MidiNoteInfo> ongoingNotesStack = new List<MidiNoteInfo>();

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		foreach(var midiEvent in events) {
			var newMidiEvent = midiEvent.Duplicate();
			// If this is a note on, we need to also send a note off event
			if(newMidiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On) {
				// If there's an ongoing note, send a note off event for that note
				if(ongoingNotesStack.Count > 0) {
					var lastOngoingNote = ongoingNotesStack[ongoingNotesStack.Count-1];
					var noteOffEvent = new MidiEvent();
					noteOffEvent.parameter1 = (byte)lastOngoingNote.pitch;
					noteOffEvent.channel = (byte)lastOngoingNote.channelNumber;
					noteOffEvent.midiChannelEvent = MidiHelper.MidiChannelEvent.Note_Off;
					noteOffEvent.deltaTime = newMidiEvent.deltaTime;
					filteredEvents.Add(noteOffEvent);
				}

				// Set the new ongoing note to this note
				int pitch = midiEvent.parameter1;
				var noteInfo = new MidiNoteInfo(midiEvent.channel, pitch);
				ongoingNotesStack.Add(noteInfo);
			} else if(newMidiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off && ongoingNotesStack.Count > 0) {
				// If we found the note off event for the current ongoing note, set ongoing note to off
				var continueToNextEvent = false;
				for(var i = 0 ; i < ongoingNotesStack.Count ; i++) {
					if(ongoingNotesStack[i].pitch == newMidiEvent.parameter1 && ongoingNotesStack[i].channelNumber == newMidiEvent.channel) {
						// If this is the last element of the stack
						if(i == ongoingNotesStack.Count-1) {
							// pop it and, after this event, send a note ON event for the next element in the stack
							ongoingNotesStack.RemoveAt(i);
							if(ongoingNotesStack.Count > 0) {
								var noteToReturnToInfo = ongoingNotesStack[ongoingNotesStack.Count-1];
								var noteOnEvent = new MidiEvent();
								noteOnEvent.parameter1 = (byte)noteToReturnToInfo.pitch;
								noteOnEvent.channel = (byte)noteToReturnToInfo.channelNumber;
								noteOnEvent.midiChannelEvent = MidiHelper.MidiChannelEvent.Note_On;
								noteOnEvent.deltaTime = newMidiEvent.deltaTime;
								filteredEvents.Add(newMidiEvent);
								filteredEvents.Add(noteOnEvent);
								continueToNextEvent = true;
								break;
							}
						} else {
							// If this is further down in the stack, then just remove it
							ongoingNotesStack.RemoveAt(i);
							break;
						}
					}
					
				}

				if(continueToNextEvent) {
					continue;
				}
			}
			
			filteredEvents.Add(newMidiEvent);
		}

		return filteredEvents.ToArray();
	}
}
