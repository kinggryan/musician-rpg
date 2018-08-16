using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransposeRules {

	/// <summary>
	/// The pitch shift rules are an array that maps from an old pitch to a new pitch,
	/// where the index in the array is the original pitch and the value at that index is the new pitch
	/// 0 is (whatever 0 is for midi files)
	/// </summary>
	[SerializeField]
	private int[] rules = new int[12];

	public int GetPitchShiftForPitch(int pitch) {
		var pitchIndex = pitch % 12;
		var newPitch = rules[pitchIndex];
		var pitchShift = newPitch - pitchIndex;
		return pitchShift;
	}
}
