using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopGameData {

	/// This is a string in the format of 0s and 1s, where each 0 or 1 is an eighth note
	public string rhythmString;

	/// Returns a number between 0 and 1, where 1 is a perfectly matched string and 0 is perfectly unmatched?
	public static float ScorePlayerStringAgainstNPCString(string npcString, string playerString) {
		var maxScore = 0;
		foreach(var character in npcString) {
			maxScore += character == '1' ? 1 : 0;
		}

		// Prevent divide by 0 errors
		if(maxScore == 0) {
			return 0;
		}

		var playerScore = 0;
		for(var i = 0 ; i < npcString.Length; i++) {
			if(npcString[i] == '1' && playerString[i] == '1') {
				playerScore += 1;
			}
		}

		// Debug.Log("Scoring '" + npcString + "' against '" + playerString + "', score is " + playerScore + " out of " + maxScore);
		return (float)playerScore/maxScore;
	}
}
