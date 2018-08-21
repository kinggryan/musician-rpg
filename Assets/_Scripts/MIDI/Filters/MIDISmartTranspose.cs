using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDISmartTranspose : MIDITrackFilter {

	class TransposeRulesQueueElement {
		public TransposeRules transposeRules;
		public int startSampleTime;

		public TransposeRulesQueueElement(TransposeRules rule, int startSampleTime) {
			transposeRules = rule;
			this.startSampleTime = startSampleTime;
		}
	}

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

	/// <summary>
	/// A queue of the transpose rules that should be applied at certain sample times.
	/// For now, it is safe to assume that they are both ordered and that the sample time will only increase.
	/// This means that it is safe to remove elements from the front of the queue when the start time of the next element has been reached
	/// </summary>
	List<TransposeRulesQueueElement> transposeRulesQueue = new List<TransposeRulesQueueElement>();

	/// <summary>
	/// When a note on event goes through the filter, the transposition gets saved here, where the key 
	/// is the original pitch and the value is the pitch shift. When a note off event is received, it should
	/// Check this array for how to transpose itself, and remove that entry from the dictionary
	/// </summary>
	Dictionary<MidiNoteInfo, int> currentNoteTranspositions = new Dictionary<MidiNoteInfo, int>();

	/// <summary>
	/// If this is a note on event, then transpose it according to transpose rules and store in memory that it is "on" and has been translated
	/// If this is a note off event, then look to see what the corresponding note on event was and translate the note off event back
	/// </summary>
	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		
		foreach(var midiEvent in events) {
			var newMidiEvent = midiEvent.Duplicate();

			if(midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On) {
				var rule = GetTransposeRulesForSampleTimeAndUpdateQueue((int)midiEvent.deltaTime);
				if(rule != null) {
					// pitch is parameter 1
					int pitch = midiEvent.parameter1;
					var pitchShift = rule.GetPitchShiftForPitch(pitch);
					var noteInfo = new MidiNoteInfo(pitch, midiEvent.channel);
					Debug.Log("Setting " + noteInfo + " to " + pitchShift);
					currentNoteTranspositions[noteInfo] = pitchShift;
					pitch = pitch + pitchShift;

					newMidiEvent.parameter1 = (byte)pitch;
				}
			} else if(midiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off) {
				// pitch is parameter 1
				int pitch = midiEvent.parameter1;
				var noteInfo = new MidiNoteInfo(pitch, midiEvent.channel);
				var pitchShift = GetShiftForNoteOffEvent(noteInfo);
				pitch = pitch + pitchShift;

				newMidiEvent.parameter1 = (byte)pitch;
			}
			
			filteredEvents.Add(newMidiEvent);
		}
		return filteredEvents.ToArray();
	}

	public void AddTransposeRule(TransposeRules rule, int startSampleTime) {
		var newQElement = new TransposeRulesQueueElement(rule, startSampleTime);
		transposeRulesQueue.Add(newQElement);
	}

	// Private
	private TransposeRules GetTransposeRulesForSampleTimeAndUpdateQueue(int sampleTime) {
		if(transposeRulesQueue.Count == 0 || transposeRulesQueue[0].startSampleTime > sampleTime) {
			return null;
		}

		// First, make sure we dequeue elements that have passed
		while(transposeRulesQueue.Count > 1 && transposeRulesQueue[1].startSampleTime <= sampleTime) {
			transposeRulesQueue.RemoveAt(0);
		}

		return transposeRulesQueue[0].transposeRules;
	}

	// Given a note off midi event, transposes it to match the correct note on 
	private int GetShiftForNoteOffEvent(MidiNoteInfo noteInfo) {
		// Look through the unresolved note on events for one matching that pitch
		if(currentNoteTranspositions.ContainsKey(noteInfo)) {
			// Use that mapping to transpose this note as well
			var shift = currentNoteTranspositions[noteInfo];
			// Remove that element from the list
			Debug.Log("Shifting " + noteInfo + " with shift " + shift);
			// currentNoteTranspositions.Remove(noteInfo);
			return shift;
		}
		else {
			Debug.LogError("Couldn't find transposition for " + noteInfo);
		}

		return 0;
	}
}
