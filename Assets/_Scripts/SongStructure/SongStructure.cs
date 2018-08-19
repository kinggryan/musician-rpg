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
	vi,
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

[System.Serializable]
public struct SongPhrase {
	public AudioLoop loop;
	public Chord chord;
	public int numTimesToPlay;

	public SongPhrase(string loopName, string chordName, int numTimesToPlay) {
		loop = AudioLoop.GetLoopForName(loopName);
		chord = SongStructureUtilities.ChordForString(chordName);
		this.numTimesToPlay = numTimesToPlay;
	}

	public int TotalBeatLength() {
		return loop.beatDuration*numTimesToPlay;
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
			case "vi": return Chord.vi;
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
}