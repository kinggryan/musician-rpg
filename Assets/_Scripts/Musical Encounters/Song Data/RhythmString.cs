using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the rhythm information over a given period of time
/// Currently this data is just saved as a string primitive, 
/// but in the future we may need more complex data and metadata around rhythm
/// </summary>
public class RhythmString {
	/// <summary>
	/// The rhythm is stored as a string
	/// </summary>
	string rhythmString;

	public RhythmString(string str) {
		rhythmString = str;
	}

	public RhythmString GetRhythmStringForBeat(int beat) {
		var beatToUse = beat % (rhythmString.Length/2);
		// Since the rhythm string is eighth notes, we just return the beat plus its upbeat
		var rhythmStringIndex = beatToUse*2;
		if(rhythmStringIndex + 1 >= rhythmString.Length) {
			//Debug.LogError("Rhythm string error: index of " + (rhythmStringIndex + 1) + " is outside string " + rhythmString);
			return new RhythmString("");
		}

		return new RhythmString(rhythmString.Substring(rhythmStringIndex,2));
	}

	// End beat is inclusive
	public RhythmString GetRhythmStringForBeatRange(int startBeat, int endBeat) {
		// TODO: Do a less memory intensive way of doing this
		var compositeString = new RhythmString("");
		for(var beat = startBeat ; beat < endBeat; beat++) {
			compositeString = compositeString.AppendRhythmString(GetRhythmStringForBeat(beat));
		}
		return compositeString;
	}

	public int GetNumRhythmStringMatches(RhythmString otherRhythmString) {
		var numMatchedChars = 0;
		// TODO: Implement the part where they extend the shorter string
		
		for(var i = 0 ; i < Mathf.Min(otherRhythmString.rhythmString.Length, rhythmString.Length); i++) {
			if(rhythmString[i].ToString() == "1" && otherRhythmString.rhythmString[i].ToString() == "1"){
				numMatchedChars++;
			}
		}
		return numMatchedChars;
	}

	public int GetNumRhythmStringMatchesOnDownbeat(RhythmString otherRhythmString) {
		var numMatchedChars = 0;
		// TODO: Implement the part where they extend the shorter string
		
		for(var i = 0 ; i < Mathf.Min(otherRhythmString.rhythmString.Length, rhythmString.Length); i += 2) {
			if(rhythmString[i].ToString() == "1" && otherRhythmString.rhythmString[i].ToString() == "1"){
				numMatchedChars++;
			}
		}
		return numMatchedChars;
	}

	public int GetNumRhythmStringMatchesOnUpbeat(RhythmString otherRhythmString) {
		var numMatchedChars = 0;
		// TODO: Implement the part where they extend the shorter string
		
		for(var i = 1 ; i < Mathf.Min(otherRhythmString.rhythmString.Length, rhythmString.Length); i += 2) {
			if(rhythmString[i].ToString() == "1" && otherRhythmString.rhythmString[i].ToString() == "1"){
				numMatchedChars++;
			}
		}
		return numMatchedChars;
	}

	public int GetMaxNumRhythmStringMatches() {
		var numOnes = 0;
		foreach(var cha in rhythmString) {
			if(cha == '1')
				numOnes++;
		}
		return numOnes;
	}

	public RhythmString AppendRhythmString(RhythmString otherString) {
		return new RhythmString(rhythmString + otherString.rhythmString);
	}
}
