using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransposeRules {

	public static TransposeRules transposeRules_I = new TransposeRules(new int[12]{ 0,1,2,3,4,5,6,7,8,9,10,11 });
	public static TransposeRules transposeRules_i = new TransposeRules(new int[12]{ 0,1,2,4,3,5,6,7,9,8,11,10 });
	public static TransposeRules transposeRules_ii = new TransposeRules(new int[12]{ 2,1,0,3,5,4,6,9,8,7,10,11 });
	public static TransposeRules transposeRules_iiDim = new TransposeRules(new int[12]{ 2,1,0,4,5,3,6,8,9,7,11,10 });
	public static TransposeRules transposeRules_iii = new TransposeRules(new int[12]{ -1,1,2,3,4,5,6,7,8,9,10,11 });
	public static TransposeRules transposeRules_bIII = new TransposeRules(new int[12]{ -2,1,2,4,3,5,6,7,9,8,11,10 });
	public static TransposeRules transposeRules_IV = new TransposeRules(new int[12]{ 2,1,4,3,5,7,6,9,8,11,10,12 });
	public static TransposeRules transposeRules_iv = new TransposeRules(new int[12]{ 2,1,3,4,5,7,6,8,9,10,11,12 });
	public static TransposeRules transposeRules_V = new TransposeRules(new int[12]{ -1,1,0,3,2,4,6,5,8,7,10,9 });
	public static TransposeRules transposeRules_v = new TransposeRules(new int[12]{ -2,1,0,4,2,3,5,7,9,8,11,10 });
	public static TransposeRules transposeRules_vi = new TransposeRules(new int[12]{ 0,1,2,3,4,7,6,9,8,11,10,12 });
	public static TransposeRules transposeRules_bVI = new TransposeRules(new int[12]{ 0,1,2,4,3,7,5,8,6,9,11,10 });	// THere might be a typo in this one
	public static TransposeRules transposeRules_viiDim = new TransposeRules(new int[12]{ -1,1,0,3,2,4,6,5,8,7,10,9 });
	public static TransposeRules transposeRules_bVII = new TransposeRules(new int[12]{ -2,1,0,4,2,3,6,5,9,7,11,8 });	

	/// <summary>
	/// The pitch shift rules are an array that maps from an old pitch to a new pitch,
	/// where the index in the array is the original pitch and the value at that index is the new pitch
	/// 0 is (whatever 0 is for midi files)
	/// </summary>
	[SerializeField]
	private int[] rules = new int[12];

	/// <summary>
	/// Returns the transposition rules to convert the standard notes into the given chord.
	/// This function currently only supports relative chord (ie chord numbers)
	/// TODO: Implement absolute (letter) chords.
	/// </summary>
	public static TransposeRules RulesForChord(Chord chord) {
		switch(chord) {
			case Chord.I: return transposeRules_I;
			case Chord.i: return transposeRules_i;
			case Chord.ii: return transposeRules_ii;
			case Chord.iiDim: return transposeRules_iiDim;
			case Chord.iii: return transposeRules_iii;
			case Chord.bIII: return transposeRules_bIII;
			case Chord.IV: return transposeRules_IV;
			case Chord.iv: return transposeRules_iv;
			case Chord.V: return transposeRules_V;
			case Chord.v: return transposeRules_v;
			case Chord.vi: return transposeRules_vi;
			case Chord.bVI: return transposeRules_bVI;
			case Chord.viiDim: return transposeRules_viiDim;
			case Chord.bVII: return transposeRules_bVII;
		}

		Debug.LogError("Tried to get transpose rules for chord '" + chord + "' but they don't exist");
		return transposeRules_I;
	}

	public TransposeRules(int[] rules) {
		if(rules.Length != 12) {
			Debug.LogError("Transpose rules should be exactly 12 items long! Got " + rules.Length);
		}
		this.rules = rules;
	}

	public int GetPitchShiftForPitch(int pitch) {
		var pitchIndex = pitch % 12;
		var newPitch = rules[pitchIndex];
		var pitchShift = newPitch - pitchIndex;
		return pitchShift;
	}

	public override string ToString() {
		return rules.ToString();
	}
}
