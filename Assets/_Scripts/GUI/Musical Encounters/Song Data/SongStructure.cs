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
	hmV,
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
public class SongPhrase {
	public Chord chord { get; private set; }
	public string[] emotionTags { get; private set; }
	public int singlePlaythroughBeatLength { get; private set; }
	/// <summary>
	/// If set, indicates that the player should be forced to play this loop during this phrase
	/// </summary>
	public AudioLoop playerLoop { get; private set; }
	/// <summary>
	/// If set, indicates that the npc should be forced to play this loop during this phrase
	/// </summary>
	public AudioLoop npcLoop { get; private set; }

	public AudioLoop loop { get; private set; }
	public int numTimesToPlay { get; private set; }

	private int totalBeatLength;

	static public List<string> GetMostCommonEmotionsInPhrases(List<SongPhrase> phrases) {
		var tempDict = new Dictionary<string,int>();
		var maxCount = 0;
		foreach(var p in phrases) {
			foreach(var tag in p.emotionTags) {
				if(tempDict.ContainsKey(tag)) {
					tempDict[tag] = tempDict[tag] + 1;
				} else {
					tempDict[tag] = 1;
				}
				maxCount = Mathf.Max(maxCount,tempDict[tag]);
			}
		}

		var listOfTags = new List<string>();

		foreach(var kvPair in tempDict) {
			if(kvPair.Value == maxCount) {
				listOfTags.Add(kvPair.Key);
			}
		}

		return listOfTags;
	}

	public SongPhrase(string loopName, string chordName, int numTimesToPlay) {
		loop = AudioLoop.GetLoopForName(loopName);
		chord = SongStructureUtilities.ChordForString(chordName);
		totalBeatLength = 0;
		singlePlaythroughBeatLength = loop.beatDuration;
		this.numTimesToPlay = numTimesToPlay;
		this.emotionTags = new string[] {};
	}

	/// <summary>
	/// The player loop and NPC loop are allowed to be null
	/// </summary>
	public SongPhrase(string chordName, int totalBeatLength, string[] emotionTags, string playerLoop = "", string npcLoop = "") {
		loop = null;
		numTimesToPlay = 1;
		chord = SongStructureUtilities.ChordForString(chordName);
		this.totalBeatLength = totalBeatLength;
		singlePlaythroughBeatLength = totalBeatLength;
		this.emotionTags = emotionTags;
		if(playerLoop != null && playerLoop != "")
			this.playerLoop = AudioLoop.GetLoopForName(playerLoop);
		if(npcLoop != null && npcLoop != "")
			this.npcLoop = AudioLoop.GetLoopForName(npcLoop);
	}

	public int TotalBeatLength() {
		if(totalBeatLength > 0) 
			return totalBeatLength;

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
		return this == otherPhrase;
	}

	public static bool operator== (SongPhrase lhs, SongPhrase rhs) {
		if(object.ReferenceEquals(lhs,null) || object.ReferenceEquals(rhs,null)) 
			return object.ReferenceEquals(lhs,null) && object.ReferenceEquals(rhs,null);

		return lhs.loop == rhs.loop && lhs.chord == rhs.chord && lhs.numTimesToPlay == rhs.numTimesToPlay;
	}
	public static bool operator!= (SongPhrase lhs, SongPhrase rhs) {
		return !(lhs == rhs);
	}
}

[System.Serializable]
public struct SongSection {
	public string name;
	public SongPhrase[] phrases;
	public int beatLength {
		get {
			var length = 0;
			foreach(var phrase in phrases)
				length += phrase.TotalBeatLength();
			return length;
		}
	}

	public static List<AudioLoop> GetSongSpecificNPCLoops(SongSection[] songSections) {
		var npcLoops = new List<AudioLoop>();
		foreach(var section in songSections) {
			foreach(var phrase in section.phrases) {
				if(phrase.npcLoop != null)
					npcLoops.Add(phrase.npcLoop);
			}
		}
		return npcLoops;
	}

	public static List<AudioLoop> GetSongSpecificPlayerLoops(SongSection[] songSections) {
		var playerLoop = new List<AudioLoop>();
		foreach(var section in songSections) {
			foreach(var phrase in section.phrases) {
				if(phrase.playerLoop != null)
					playerLoop.Add(phrase.playerLoop);
			}
		}
		return playerLoop;
	}

	public static SongPhrase GetSongPhraseForBeat(SongSection[] songSections, int beatNumber) {
		var currentBeat = 0;
		foreach(var section in songSections) {
			foreach(var phrase in section.phrases) {
				var phraseStart = currentBeat;
				var phraseEnd = currentBeat + phrase.TotalBeatLength();
				if(beatNumber >= phraseStart && beatNumber < phraseEnd)
					return phrase;
				currentBeat += phrase.TotalBeatLength();
			}
		}
		return null;
	}
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
			case "hmV": return Chord.hmV;
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