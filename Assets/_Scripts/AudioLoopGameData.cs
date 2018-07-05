using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopGameData {

	/// This is a string in the format of 0s and 1s, where each 0 or 1 is an eighth note
	public string rhythmString;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// Returns a number between 0 and 1, where 1 is a perfectly matched string and 0 is perfectly unmatched?
	static float ScoreTwoRhythmStrings(string rhythmStringA, string rhythmStringB) {
		// 
		return 0;
	}
}
