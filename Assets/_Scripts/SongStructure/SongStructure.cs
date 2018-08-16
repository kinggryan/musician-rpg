using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public enum Chord {
	// All the chord relatives
	I,
	i,
	ii,
	iiDim,
	iii,
	bIII,
	IV,
	iv,
	V,
	v,
	vii,
	bVI,
	viiDim,
	bVII,

	// All the Chord Literals
	AM,
	Am,
	BbM,
	Bbm,
	BM,
	Bm,
	CM,
	Cm,
	DbM,
	Dbm,
	DM,
	Dm,
	EbM,
	Ebm,
	EM,
	Em,
	FM,
	Fm,
	GbM,
	Gbm,
	GM,
	Gm,
	AbM,
	Abm
}

/// AudioLoop stores all the different loop name types.
[System.Serializable]
public enum AudioLoop {
	Flute_01,
	Flute_02,
	Flute_03,
	Flute_04,
	Guitar_01,
	Guitar_02,
	Oud_1,
	Oud_2,
	Oud_3,
	Oud_4,
	Derbakki_1,
	Derbakki_2,
	Derbakki_3,
	Derbakki_4
}

[System.Serializable]
public struct SongPhrase {
	public AudioLoop loop;
	public Chord chord;
	public int numTimesToPlay;

	public SongPhrase(string loopName, string chordName, int numTimesToPlay) {
		loop = SongStructureUtilities.LoopForString(loopName);
		chord = SongStructureUtilities.ChordForString(chordName);
		this.numTimesToPlay = numTimesToPlay;
	}

	public int TotalBeatLength() {
		return SongStructureUtilities.NumBeatsForLoop(loop)*numTimesToPlay;
	}

	public override int GetHashCode() {
		return 0;
	}

	public override bool Equals(object obj) {
		if(!(obj is SongPhrase)) {
			return false;
		}
		var otherPhrase = (SongPhrase)obj;
		return this.loop == otherPhrase.loop && this.chord == otherPhrase.chord && this.numTimesToPlay == otherPhrase.numTimesToPlay;
	}

	public static bool operator== (SongPhrase lhs, SongPhrase rhs) {
		return lhs.loop == rhs.loop && lhs.chord == rhs.chord && lhs.numTimesToPlay == rhs.numTimesToPlay;
	}
	public static bool operator!= (SongPhrase lhs, SongPhrase rhs) {
		return !(lhs.loop == rhs.loop && lhs.chord == rhs.chord && lhs.numTimesToPlay == rhs.numTimesToPlay);
	}
}

[System.Serializable]
public struct SongSection {
	public string name;
	public SongPhrase[] phrases;
}


public static class SongStructureUtilities {
	public static Chord ChordForString(string chord) {
		switch(chord) {
			// The Chord Numbers
			case "I": return Chord.I;
			case "i": return Chord.i;
			case "ii": return Chord.ii;
			case "iiDim": return Chord.iiDim;
			case "iii": return Chord.iii;
			case "bIII": return Chord.bIII;
			case "IV": return Chord.IV;
			case "iv": return Chord.iv;
			case "V": return Chord.V;
			case "v": return Chord.v;
			case "vii": return Chord.vii;
			case "bVI": return Chord.bVI;
			case "viiDim": return Chord.viiDim;
			case "bVII": return Chord.bVII;
			// The Chord Literals
			case "AM": return Chord.AM;
			case "Am": return Chord.Am;
			case "BbM": return Chord.BbM;
			case "Bbm": return Chord.Bbm;
			case "BM": return Chord.BM;
			case "Bm": return Chord.Bm;
			case "CM": return Chord.CM;
			case "Cm": return Chord.Cm;
			case "DbM": return Chord.DbM;
			case "Dbm": return Chord.Dbm;
			case "DM": return Chord.DM;
			case "Dm": return Chord.Dm;
			case "EbM": return Chord.EbM;
			case "Ebm": return Chord.Ebm;
			case "EM": return Chord.EM;
			case "Em": return Chord.Em;
			case "FM": return Chord.FM;
			case "Fm": return Chord.Fm;
			case "GbM": return Chord.GbM;
			case "Gbm": return Chord.Gbm;
			case "GM": return Chord.GM;
			case "Gm": return Chord.Gm;
			case "AbM": return Chord.AbM;
			case "Abm": return Chord.Abm;
		}

		Debug.LogError("Tried to load chord for '" + chord + "' but one doesn't exist");
		return Chord.I;
	}

	public static AudioLoop LoopForString(string loopName) {
		switch(loopName) {
			case "Oud_1": return AudioLoop.Oud_1;
			case "Oud_2": return AudioLoop.Oud_2;
			case "Oud_3": return AudioLoop.Oud_3;
			case "Oud_4": return AudioLoop.Oud_4;
			case "Derbakki_1": return AudioLoop.Derbakki_1;
			case "Derbakki_2": return AudioLoop.Derbakki_2;
			case "Derbakki_3": return AudioLoop.Derbakki_3;
			case "Derbakki_4": return AudioLoop.Derbakki_4;
			case "Guitar_01": return AudioLoop.Guitar_01;
			case "Guitar_02": return AudioLoop.Guitar_02;
			case "Flute_01": return AudioLoop.Flute_01;
			case "Flute_02": return AudioLoop.Flute_02;
			case "Flute_03": return AudioLoop.Flute_03;
			case "Flute_04": return AudioLoop.Flute_04;
		}

		Debug.LogError("Tried to load loop for '" + loopName + "' but one doesn't exist");
		return AudioLoop.Oud_1;
	}

	public static int NumBeatsForLoop(AudioLoop loop) {
		switch(loop) {
			case AudioLoop.Derbakki_1:
			case AudioLoop.Derbakki_2:
			case AudioLoop.Derbakki_3:
			case AudioLoop.Derbakki_4:
				return 1;
			case AudioLoop.Oud_1:
			case AudioLoop.Oud_2:
			case AudioLoop.Oud_3:
			case AudioLoop.Oud_4:
				return 2;
			case AudioLoop.Guitar_01:
			case AudioLoop.Guitar_02:
				return 4;
			case AudioLoop.Flute_01:
			case AudioLoop.Flute_02:
			case AudioLoop.Flute_03:
			case AudioLoop.Flute_04:
				return 8;
		}

		Debug.LogError("Tried to get num beats for for '" + loop + "' but can't");
		return 0;
	}

	public static string GetRhythmStringFromLoop(AudioLoop loop) {
		switch(loop) {
			case AudioLoop.Flute_01: return "10010000";
			case AudioLoop.Flute_02: return "10100000";
			case AudioLoop.Flute_03: return "11010000";
			case AudioLoop.Flute_04: return "00110000";
			case AudioLoop.Guitar_01: return "10100000";
			case AudioLoop.Guitar_02: return "10100000";
			case AudioLoop.Oud_1: return "1101";
			case AudioLoop.Oud_2: return "1010";
			case AudioLoop.Oud_3: return "1011";
			case AudioLoop.Oud_4: return "1110";
			case AudioLoop.Derbakki_1: return "10";
			case AudioLoop.Derbakki_2: return "01";
			case AudioLoop.Derbakki_3: return "10";
			case AudioLoop.Derbakki_4: return "11";
		}

		Debug.LogError("Tried to get rhythm string for loop '" + loop + "' but can't");
		return "";
	}
}