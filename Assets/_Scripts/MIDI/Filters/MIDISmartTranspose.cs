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

	/// <summary>
	/// A queue of the transpose rules that should be applied at certain sample times.
	/// For now, it is safe to assume that they are both ordered and that the sample time will only increase.
	/// This means that it is safe to remove elements from the front of the queue when the start time of the next element has been reached
	/// </summary>
	List<TransposeRulesQueueElement> transposeRulesQueue = new List<TransposeRulesQueueElement>();

	// If this is a note on event, then transpose it according to transpose rules and store in memory that it is "on" and has been translated
	// If this is a note off event, then look to see what the corresponding note on event was and translate the note off event back

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		
		foreach(var midiEvent in events) {
			var newMidiEvent = midiEvent.Duplicate();

			var rule = GetTransposeRulesForSampleTimeAndUpdateQueue((int)midiEvent.deltaTime);
			if(rule != null) {
				// pitch is parameter 1
				int pitch = midiEvent.parameter1;
				var pitchShift = rule.GetPitchShiftForPitch(pitch);
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
		if(transposeRulesQueue.Count == 0) {
			Debug.Log("L 0");
			return null;
		}

		// First, make sure we dequeue elements that have passed
		while(transposeRulesQueue.Count > 1 && transposeRulesQueue[1].startSampleTime < sampleTime) {
			transposeRulesQueue.RemoveAt(0);
		}

		Debug.Log("Transposing " + transposeRulesQueue[0].transposeRules.ToString());
		return transposeRulesQueue[0].transposeRules;
	}
}
