using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class MIDISmartTranspose : MIDITrackFilter {

	public TransposeRules transposeRules;

	private int[] rules;

	void Start(){
		
	}

	public override MidiEvent[] FilterMidiEvents(MidiEvent[] events) {
		var filteredEvents = new List<MidiEvent>();
		rules = transposeRules.rules;
		
		foreach(var midiEvent in events) {
			if(rules != null) {
				foreach (int i in rules){
					int pitch = midiEvent.parameter1;
					//Debug.Log("Pitch: " + pitch);
					int index = System.Array.IndexOf(rules,i);
					if (pitch % 12 == index){
						var newMidiEvent = midiEvent.Duplicate();
						// pitch is parameter 1
						pitch = pitch + (i - index);
						//Debug.Log("New Pitch: " + pitch);
						//Debug.Log("Transposig by " + (i  - index));
						newMidiEvent.parameter1 = (byte)pitch;
						filteredEvents.Add(newMidiEvent);
						break;
					}
				}
			} else {
				Debug.LogError("!No Transpose Rules Set!");
			}
		}
		return filteredEvents.ToArray();
	}

}
