using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFileBank {

	// Get the audio clip for the given chord for this audioLoop's loop to play
	public static AudioClip AudioClipForLoopAndChord(AudioLoop loop, Chord chord) {
		switch(loop) {
			case AudioLoop.Flute_01:
				return GetFlute_01AudioClip(chord);
			case AudioLoop.Flute_02:
				return GetFlute_02AudioClip(chord);
			case AudioLoop.Flute_03:
				return GetFlute_03AudioClip(chord);
			case AudioLoop.Flute_04:
				return GetFlute_04AudioClip(chord);
			case AudioLoop.Guitar_01:
				return GetGuitar_01AudioClip(chord);
			case AudioLoop.Guitar_02:
				return GetGuitar_02AudioClip(chord);
			case AudioLoop.Oud_1:
				return GetOud_1AudioClip(chord);
			case AudioLoop.Oud_2:
				return GetOud_2AudioClip(chord);
			case AudioLoop.Oud_3:
				return GetOud_3AudioClip(chord);
			case AudioLoop.Oud_4:
				return GetOud_4AudioClip(chord);
			case AudioLoop.Derbakki_1:
				return GetDerbakki_1AudioClip(chord);
			case AudioLoop.Derbakki_2:
				return GetDerbakki_2AudioClip(chord);
			case AudioLoop.Derbakki_3:
				return GetDerbakki_3AudioClip(chord);
			case AudioLoop.Derbakki_4:
				return GetDerbakki_4AudioClip(chord);
		}

		// If we don't find the clip to play, something is misconfigured
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loop);
		return null;
	}

	public static AudioClip GetFlute_01AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM: return Resources.Load("Flute/Flute_01/Flute_01_Major_A", typeof(AudioClip)) as AudioClip;
			case Chord.Am: return Resources.Load("Flute/Flute_01/Flute_01_minor_A", typeof(AudioClip)) as AudioClip; 
			case Chord.BbM: return Resources.Load("Flute/Flute_01/Flute_01_Major_A#", typeof(AudioClip)) as AudioClip;
			case Chord.Bbm: return Resources.Load("Flute/Flute_01/Flute_01_minor_A#", typeof(AudioClip)) as AudioClip; 
			case Chord.BM: return Resources.Load("Flute/Flute_01/Flute_01_Major_B", typeof(AudioClip)) as AudioClip;
			case Chord.Bm: return Resources.Load("Flute/Flute_01/Flute_01_minor_B", typeof(AudioClip)) as AudioClip; 
			case Chord.CM: return Resources.Load("Flute/Flute_01/Flute_01_Major_C", typeof(AudioClip)) as AudioClip;
			case Chord.Cm: return Resources.Load("Flute/Flute_01/Flute_01_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DbM:return Resources.Load("Flute/Flute_01/Flute_01_Major_C#", typeof(AudioClip)) as AudioClip;
			case Chord.Dbm: return Resources.Load("Flute/Flute_01/Flute_01_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DM: return Resources.Load("Flute/Flute_01/Flute_01_Major_D", typeof(AudioClip)) as AudioClip;
			case Chord.Dm: return Resources.Load("Flute/Flute_01/Flute_01_minor_D", typeof(AudioClip)) as AudioClip; 
			case Chord.EbM: return Resources.Load("Flute/Flute_01/Flute_01_Major_D#", typeof(AudioClip)) as AudioClip;
			case Chord.Ebm: return Resources.Load("Flute/Flute_01/Flute_01_minor_D#", typeof(AudioClip)) as AudioClip; 
			case Chord.EM: return Resources.Load("Flute/Flute_01/Flute_01_Major_E", typeof(AudioClip)) as AudioClip;
			case Chord.Em: return Resources.Load("Flute/Flute_01/Flute_01_minor_E", typeof(AudioClip)) as AudioClip; 
			case Chord.FM: return Resources.Load("Flute/Flute_01/Flute_01_Major_F", typeof(AudioClip)) as AudioClip;
			case Chord.Fm: return Resources.Load("Flute/Flute_01/Flute_01_minor_F", typeof(AudioClip)) as AudioClip; 
			case Chord.GbM: return Resources.Load("Flute/Flute_01/Flute_01_Major_F#", typeof(AudioClip)) as AudioClip;
			case Chord.Gbm: return Resources.Load("Flute/Flute_01/Flute_01_minor_F#", typeof(AudioClip)) as AudioClip; 
			case Chord.GM: return Resources.Load("Flute/Flute_01/Flute_01_Major_G", typeof(AudioClip)) as AudioClip;
			case Chord.Gm: return Resources.Load("Flute/Flute_01/Flute_01_minor_G", typeof(AudioClip)) as AudioClip; 
			case Chord.AbM: return Resources.Load("Flute/Flute_01/Flute_01_Major_G#", typeof(AudioClip)) as AudioClip;
			case Chord.Abm: return Resources.Load("Flute/Flute_01/Flute_01_minor_G#", typeof(AudioClip)) as AudioClip; 
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Flute 01");
		return null;
	}

	private static AudioClip GetFlute_02AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM: return Resources.Load("Flute/Flute_02/Flute_02_Major_A", typeof(AudioClip)) as AudioClip;
			case Chord.Am: return Resources.Load("Flute/Flute_02/Flute_02_minor_A", typeof(AudioClip)) as AudioClip; 
			case Chord.BbM: return Resources.Load("Flute/Flute_02/Flute_02_Major_A#", typeof(AudioClip)) as AudioClip;
			case Chord.Bbm: return Resources.Load("Flute/Flute_02/Flute_02_minor_A#", typeof(AudioClip)) as AudioClip; 
			case Chord.BM: return Resources.Load("Flute/Flute_02/Flute_02_Major_B", typeof(AudioClip)) as AudioClip;
			case Chord.Bm: return Resources.Load("Flute/Flute_02/Flute_02_minor_B", typeof(AudioClip)) as AudioClip; 
			case Chord.CM: return Resources.Load("Flute/Flute_02/Flute_02_Major_C", typeof(AudioClip)) as AudioClip;
			case Chord.Cm: return Resources.Load("Flute/Flute_02/Flute_02_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DbM:return Resources.Load("Flute/Flute_02/Flute_02_Major_C#", typeof(AudioClip)) as AudioClip;
			case Chord.Dbm: return Resources.Load("Flute/Flute_02/Flute_02_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DM: return Resources.Load("Flute/Flute_02/Flute_02_Major_D", typeof(AudioClip)) as AudioClip;
			case Chord.Dm: return Resources.Load("Flute/Flute_02/Flute_02_minor_D", typeof(AudioClip)) as AudioClip; 
			case Chord.EbM: return Resources.Load("Flute/Flute_02/Flute_02_Major_D#", typeof(AudioClip)) as AudioClip;
			case Chord.Ebm: return Resources.Load("Flute/Flute_02/Flute_02_minor_D#", typeof(AudioClip)) as AudioClip; 
			case Chord.EM: return Resources.Load("Flute/Flute_02/Flute_02_Major_E", typeof(AudioClip)) as AudioClip;
			case Chord.Em: return Resources.Load("Flute/Flute_02/Flute_02_minor_E", typeof(AudioClip)) as AudioClip; 
			case Chord.FM: return Resources.Load("Flute/Flute_02/Flute_02_Major_F", typeof(AudioClip)) as AudioClip;
			case Chord.Fm: return Resources.Load("Flute/Flute_02/Flute_02_minor_F", typeof(AudioClip)) as AudioClip; 
			case Chord.GbM: return Resources.Load("Flute/Flute_02/Flute_02_Major_F#", typeof(AudioClip)) as AudioClip;
			case Chord.Gbm: return Resources.Load("Flute/Flute_02/Flute_02_minor_F#", typeof(AudioClip)) as AudioClip; 
			case Chord.GM: return Resources.Load("Flute/Flute_02/Flute_02_Major_G", typeof(AudioClip)) as AudioClip;
			case Chord.Gm: return Resources.Load("Flute/Flute_02/Flute_02_minor_G", typeof(AudioClip)) as AudioClip; 
			case Chord.AbM: return Resources.Load("Flute/Flute_02/Flute_02_Major_G#", typeof(AudioClip)) as AudioClip;
			case Chord.Abm: return Resources.Load("Flute/Flute_02/Flute_02_minor_G#", typeof(AudioClip)) as AudioClip; 
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Flute 02");
		return null;
	}

	private static AudioClip GetFlute_03AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM: return Resources.Load("Flute/Flute_03/Flute_03_Major_A", typeof(AudioClip)) as AudioClip;
			case Chord.Am: return Resources.Load("Flute/Flute_03/Flute_03_minor_A", typeof(AudioClip)) as AudioClip; 
			case Chord.BbM: return Resources.Load("Flute/Flute_03/Flute_03_Major_A#", typeof(AudioClip)) as AudioClip;
			case Chord.Bbm: return Resources.Load("Flute/Flute_03/Flute_03_minor_A#", typeof(AudioClip)) as AudioClip; 
			case Chord.BM: return Resources.Load("Flute/Flute_03/Flute_03_Major_B", typeof(AudioClip)) as AudioClip;
			case Chord.Bm: return Resources.Load("Flute/Flute_03/Flute_03_minor_B", typeof(AudioClip)) as AudioClip; 
			case Chord.CM: return Resources.Load("Flute/Flute_03/Flute_03_Major_C", typeof(AudioClip)) as AudioClip;
			case Chord.Cm: return Resources.Load("Flute/Flute_03/Flute_03_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DbM:return Resources.Load("Flute/Flute_03/Flute_03_Major_C#", typeof(AudioClip)) as AudioClip;
			case Chord.Dbm: return Resources.Load("Flute/Flute_03/Flute_03_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DM: return Resources.Load("Flute/Flute_03/Flute_03_Major_D", typeof(AudioClip)) as AudioClip;
			case Chord.Dm: return Resources.Load("Flute/Flute_03/Flute_03_minor_D", typeof(AudioClip)) as AudioClip; 
			case Chord.EbM: return Resources.Load("Flute/Flute_03/Flute_03_Major_D#", typeof(AudioClip)) as AudioClip;
			case Chord.Ebm: return Resources.Load("Flute/Flute_03/Flute_03_minor_D#", typeof(AudioClip)) as AudioClip; 
			case Chord.EM: return Resources.Load("Flute/Flute_03/Flute_03_Major_E", typeof(AudioClip)) as AudioClip;
			case Chord.Em: return Resources.Load("Flute/Flute_03/Flute_03_minor_E", typeof(AudioClip)) as AudioClip; 
			case Chord.FM: return Resources.Load("Flute/Flute_03/Flute_03_Major_F", typeof(AudioClip)) as AudioClip;
			case Chord.Fm: return Resources.Load("Flute/Flute_03/Flute_03_minor_F", typeof(AudioClip)) as AudioClip; 
			case Chord.GbM: return Resources.Load("Flute/Flute_03/Flute_03_Major_F#", typeof(AudioClip)) as AudioClip;
			case Chord.Gbm: return Resources.Load("Flute/Flute_03/Flute_03_minor_F#", typeof(AudioClip)) as AudioClip; 
			case Chord.GM: return Resources.Load("Flute/Flute_03/Flute_03_Major_G", typeof(AudioClip)) as AudioClip;
			case Chord.Gm: return Resources.Load("Flute/Flute_03/Flute_03_minor_G", typeof(AudioClip)) as AudioClip; 
			case Chord.AbM: return Resources.Load("Flute/Flute_03/Flute_03_Major_G#", typeof(AudioClip)) as AudioClip;
			case Chord.Abm: return Resources.Load("Flute/Flute_03/Flute_03_minor_G#", typeof(AudioClip)) as AudioClip; 
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Flute 03");
		return null;
	}

	private static AudioClip GetFlute_04AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM: return Resources.Load("Flute/Flute_04/Flute_04_Major_A", typeof(AudioClip)) as AudioClip;
			case Chord.Am: return Resources.Load("Flute/Flute_04/Flute_04_minor_A", typeof(AudioClip)) as AudioClip; 
			case Chord.BbM: return Resources.Load("Flute/Flute_04/Flute_04_Major_A#", typeof(AudioClip)) as AudioClip;
			case Chord.Bbm: return Resources.Load("Flute/Flute_04/Flute_04_minor_A#", typeof(AudioClip)) as AudioClip; 
			case Chord.BM: return Resources.Load("Flute/Flute_04/Flute_04_Major_B", typeof(AudioClip)) as AudioClip;
			case Chord.Bm: return Resources.Load("Flute/Flute_04/Flute_04_minor_B", typeof(AudioClip)) as AudioClip; 
			case Chord.CM: return Resources.Load("Flute/Flute_04/Flute_04_Major_C", typeof(AudioClip)) as AudioClip;
			case Chord.Cm: return Resources.Load("Flute/Flute_04/Flute_04_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DbM:return Resources.Load("Flute/Flute_04/Flute_04_Major_C#", typeof(AudioClip)) as AudioClip;
			case Chord.Dbm: return Resources.Load("Flute/Flute_04/Flute_04_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DM: return Resources.Load("Flute/Flute_04/Flute_04_Major_D", typeof(AudioClip)) as AudioClip;
			case Chord.Dm: return Resources.Load("Flute/Flute_04/Flute_04_minor_D", typeof(AudioClip)) as AudioClip; 
			case Chord.EbM: return Resources.Load("Flute/Flute_04/Flute_04_Major_D#", typeof(AudioClip)) as AudioClip;
			case Chord.Ebm: return Resources.Load("Flute/Flute_04/Flute_04_minor_D#", typeof(AudioClip)) as AudioClip; 
			case Chord.EM: return Resources.Load("Flute/Flute_04/Flute_04_Major_E", typeof(AudioClip)) as AudioClip;
			case Chord.Em: return Resources.Load("Flute/Flute_04/Flute_04_minor_E", typeof(AudioClip)) as AudioClip; 
			case Chord.FM: return Resources.Load("Flute/Flute_04/Flute_04_Major_F", typeof(AudioClip)) as AudioClip;
			case Chord.Fm: return Resources.Load("Flute/Flute_04/Flute_04_minor_F", typeof(AudioClip)) as AudioClip; 
			case Chord.GbM: return Resources.Load("Flute/Flute_04/Flute_04_Major_F#", typeof(AudioClip)) as AudioClip;
			case Chord.Gbm: return Resources.Load("Flute/Flute_04/Flute_04_minor_F#", typeof(AudioClip)) as AudioClip; 
			case Chord.GM: return Resources.Load("Flute/Flute_04/Flute_04_Major_G", typeof(AudioClip)) as AudioClip;
			case Chord.Gm: return Resources.Load("Flute/Flute_04/Flute_04_minor_G", typeof(AudioClip)) as AudioClip; 
			case Chord.AbM: return Resources.Load("Flute/Flute_04/Flute_04_Major_G#", typeof(AudioClip)) as AudioClip;
			case Chord.Abm: return Resources.Load("Flute/Flute_04/Flute_04_minor_G#", typeof(AudioClip)) as AudioClip; 
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Flute 04");
		return null;
	}

	private static AudioClip GetGuitar_01AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_A", typeof(AudioClip)) as AudioClip;
			case Chord.Am: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_A", typeof(AudioClip)) as AudioClip; 
			case Chord.BbM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_A#", typeof(AudioClip)) as AudioClip;
			case Chord.Bbm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_A#", typeof(AudioClip)) as AudioClip; 
			case Chord.BM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_B", typeof(AudioClip)) as AudioClip;
			case Chord.Bm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_B", typeof(AudioClip)) as AudioClip; 
			case Chord.CM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_C", typeof(AudioClip)) as AudioClip;
			case Chord.Cm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DbM:return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_C#", typeof(AudioClip)) as AudioClip;
			case Chord.Dbm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_D", typeof(AudioClip)) as AudioClip;
			case Chord.Dm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_D", typeof(AudioClip)) as AudioClip; 
			case Chord.EbM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_D#", typeof(AudioClip)) as AudioClip;
			case Chord.Ebm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_D#", typeof(AudioClip)) as AudioClip; 
			case Chord.EM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_E", typeof(AudioClip)) as AudioClip;
			case Chord.Em: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_E", typeof(AudioClip)) as AudioClip; 
			case Chord.FM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_F", typeof(AudioClip)) as AudioClip;
			case Chord.Fm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_F", typeof(AudioClip)) as AudioClip; 
			case Chord.GbM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_F#", typeof(AudioClip)) as AudioClip;
			case Chord.Gbm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_F#", typeof(AudioClip)) as AudioClip; 
			case Chord.GM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_G", typeof(AudioClip)) as AudioClip;
			case Chord.Gm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_G", typeof(AudioClip)) as AudioClip; 
			case Chord.AbM: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_Major_G#", typeof(AudioClip)) as AudioClip;
			case Chord.Abm: return Resources.Load("Guitar/Guitar_01/GuitarLoop_01_minor_G#", typeof(AudioClip)) as AudioClip; 
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Guitar 01");
		return null;
	}

	private static AudioClip GetGuitar_02AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_A", typeof(AudioClip)) as AudioClip;
			case Chord.Am: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_A", typeof(AudioClip)) as AudioClip; 
			case Chord.BbM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_A#", typeof(AudioClip)) as AudioClip;
			case Chord.Bbm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_A#", typeof(AudioClip)) as AudioClip; 
			case Chord.BM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_B", typeof(AudioClip)) as AudioClip;
			case Chord.Bm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_B", typeof(AudioClip)) as AudioClip; 
			case Chord.CM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_C", typeof(AudioClip)) as AudioClip;
			case Chord.Cm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DbM:return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_C#", typeof(AudioClip)) as AudioClip;
			case Chord.Dbm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_C", typeof(AudioClip)) as AudioClip; 
			case Chord.DM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_D", typeof(AudioClip)) as AudioClip;
			case Chord.Dm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_D", typeof(AudioClip)) as AudioClip; 
			case Chord.EbM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_D#", typeof(AudioClip)) as AudioClip;
			case Chord.Ebm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_D#", typeof(AudioClip)) as AudioClip; 
			case Chord.EM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_E", typeof(AudioClip)) as AudioClip;
			case Chord.Em: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_E", typeof(AudioClip)) as AudioClip; 
			case Chord.FM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_F", typeof(AudioClip)) as AudioClip;
			case Chord.Fm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_F", typeof(AudioClip)) as AudioClip; 
			case Chord.GbM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_F#", typeof(AudioClip)) as AudioClip;
			case Chord.Gbm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_F#", typeof(AudioClip)) as AudioClip; 
			case Chord.GM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_G", typeof(AudioClip)) as AudioClip;
			case Chord.Gm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_G", typeof(AudioClip)) as AudioClip; 
			case Chord.AbM: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_Major_G#", typeof(AudioClip)) as AudioClip;
			case Chord.Abm: return Resources.Load("Guitar/Guitar_02/GuitarLoop_02_minor_G#", typeof(AudioClip)) as AudioClip; 
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Guitar 02");
		return null;
	}

	private static AudioClip GetOud_1AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM:
			case Chord.Am: return Resources.Load("Oud/Oud_1/Oud_01_A", typeof(AudioClip)) as AudioClip;
			case Chord.BbM:
			case Chord.Bbm: return Resources.Load("Oud/Oud_1/Oud_01_Bb", typeof(AudioClip)) as AudioClip;
			case Chord.BM:
			case Chord.Bm: return Resources.Load("Oud/Oud_1/Oud_01_B", typeof(AudioClip)) as AudioClip;
			case Chord.CM: 
			case Chord.Cm: return Resources.Load("Oud/Oud_1/Oud_01_C", typeof(AudioClip)) as AudioClip;
			case Chord.DbM:
			case Chord.Dbm: return Resources.Load("Oud/Oud_1/Oud_01_Db", typeof(AudioClip)) as AudioClip;
			case Chord.DM: 
			case Chord.Dm: return Resources.Load("Oud/Oud_1/Oud_01_D", typeof(AudioClip)) as AudioClip;
			case Chord.EbM:
			case Chord.Ebm: return Resources.Load("Oud/Oud_1/Oud_01_Eb", typeof(AudioClip)) as AudioClip;
			case Chord.EM: 
			case Chord.Em: return Resources.Load("Oud/Oud_1/Oud_01_E", typeof(AudioClip)) as AudioClip;
			case Chord.FM:
			case Chord.Fm: return Resources.Load("Oud/Oud_1/Oud_01_F", typeof(AudioClip)) as AudioClip;
			case Chord.GbM: 
			case Chord.Gbm: return Resources.Load("Oud/Oud_1/Oud_01_Gb", typeof(AudioClip)) as AudioClip;
			case Chord.GM: 
			case Chord.Gm: return Resources.Load("Oud/Oud_1/Oud_01_G", typeof(AudioClip)) as AudioClip;
			case Chord.AbM:
			case Chord.Abm:  return Resources.Load("Oud/Oud_1/Oud_01_Ab", typeof(AudioClip)) as AudioClip;
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Oud 01");
		return null;
	}

	private static AudioClip GetOud_2AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM: 
			case Chord.Am: return Resources.Load("Oud/Oud_2/Oud_02_A", typeof(AudioClip)) as AudioClip;
			case Chord.BbM: 
			case Chord.Bbm: return Resources.Load("Oud/Oud_2/Oud_02_Bb", typeof(AudioClip)) as AudioClip;
			case Chord.BM: 
			case Chord.Bm: return Resources.Load("Oud/Oud_2/Oud_02_B", typeof(AudioClip)) as AudioClip;
			case Chord.CM:
			case Chord.Cm: return Resources.Load("Oud/Oud_2/Oud_02_C", typeof(AudioClip)) as AudioClip;
			case Chord.DbM:
			case Chord.Dbm: return Resources.Load("Oud/Oud_2/Oud_02_Db", typeof(AudioClip)) as AudioClip;
			case Chord.DM: 
			case Chord.Dm: return Resources.Load("Oud/Oud_2/Oud_02_D", typeof(AudioClip)) as AudioClip;
			case Chord.EbM:
			case Chord.Ebm: return Resources.Load("Oud/Oud_2/Oud_02_Eb", typeof(AudioClip)) as AudioClip;
			case Chord.EM: 
			case Chord.Em: return Resources.Load("Oud/Oud_2/Oud_02_E", typeof(AudioClip)) as AudioClip;
			case Chord.FM:
			case Chord.Fm: return Resources.Load("Oud/Oud_2/Oud_02_F", typeof(AudioClip)) as AudioClip;
			case Chord.GbM:
			case Chord.Gbm: return Resources.Load("Oud/Oud_2/Oud_02_Gb", typeof(AudioClip)) as AudioClip;
			case Chord.GM:
			case Chord.Gm: return Resources.Load("Oud/Oud_2/Oud_02_G", typeof(AudioClip)) as AudioClip;
			case Chord.AbM:
			case Chord.Abm: return Resources.Load("Oud/Oud_2/Oud_02_Ab", typeof(AudioClip)) as AudioClip;
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop Oud 02");
		return null;
	}

	private static AudioClip GetOud_3AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM:
			case Chord.Am: return Resources.Load("Oud/Oud_3/Oud_03_A", typeof(AudioClip)) as AudioClip;
			case Chord.BbM:
			case Chord.Bbm: return Resources.Load("Oud/Oud_3/Oud_03_Bb", typeof(AudioClip)) as AudioClip;
			case Chord.BM:
			case Chord.Bm: return Resources.Load("Oud/Oud_3/Oud_03_B", typeof(AudioClip)) as AudioClip;
			case Chord.CM:
			case Chord.Cm: return Resources.Load("Oud/Oud_3/Oud_03_C", typeof(AudioClip)) as AudioClip;
			case Chord.DbM:
			case Chord.Dbm: return Resources.Load("Oud/Oud_3/Oud_03_Db", typeof(AudioClip)) as AudioClip;
			case Chord.DM:
			case Chord.Dm: return Resources.Load("Oud/Oud_3/Oud_03_D", typeof(AudioClip)) as AudioClip;
			case Chord.EbM:
			case Chord.Ebm: return Resources.Load("Oud/Oud_3/Oud_03_Eb", typeof(AudioClip)) as AudioClip;
			case Chord.EM:
			case Chord.Em: return Resources.Load("Oud/Oud_3/Oud_03_E", typeof(AudioClip)) as AudioClip;
			case Chord.FM:
			case Chord.Fm: return Resources.Load("Oud/Oud_3/Oud_03_F", typeof(AudioClip)) as AudioClip;
			case Chord.GbM:
			case Chord.Gbm: return Resources.Load("Oud/Oud_3/Oud_03_Gb", typeof(AudioClip)) as AudioClip;
			case Chord.GM:
			case Chord.Gm: return Resources.Load("Oud/Oud_3/Oud_03_G", typeof(AudioClip)) as AudioClip;
			case Chord.AbM:
			case Chord.Abm: return Resources.Load("Oud/Oud_3/Oud_03_Ab", typeof(AudioClip)) as AudioClip;
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + "  with loop Oud 03");
		return null;
	}

	private static AudioClip GetOud_4AudioClip(Chord chord){
		switch(chord) {
			case Chord.AM:
			case Chord.Am: return Resources.Load("Oud/Oud_4/Oud_04_A", typeof(AudioClip)) as AudioClip;
			case Chord.BbM:
			case Chord.Bbm: return Resources.Load("Oud/Oud_4/Oud_04_Bb", typeof(AudioClip)) as AudioClip;
			case Chord.BM:
			case Chord.Bm: return Resources.Load("Oud/Oud_4/Oud_04_B", typeof(AudioClip)) as AudioClip;
			case Chord.CM:
			case Chord.Cm: return Resources.Load("Oud/Oud_4/Oud_04_C", typeof(AudioClip)) as AudioClip;
			case Chord.DbM:
			case Chord.Dbm: return Resources.Load("Oud/Oud_4/Oud_04_Db", typeof(AudioClip)) as AudioClip;
			case Chord.DM:
			case Chord.Dm: return Resources.Load("Oud/Oud_4/Oud_04_D", typeof(AudioClip)) as AudioClip;
			case Chord.EbM:
			case Chord.Ebm: return Resources.Load("Oud/Oud_4/Oud_04_Eb", typeof(AudioClip)) as AudioClip;
			case Chord.EM:
			case Chord.Em: return Resources.Load("Oud/Oud_4/Oud_04_E", typeof(AudioClip)) as AudioClip;
			case Chord.FM:
			case Chord.Fm: return Resources.Load("Oud/Oud_4/Oud_04_F", typeof(AudioClip)) as AudioClip;
			case Chord.GbM:
			case Chord.Gbm: return Resources.Load("Oud/Oud_4/Oud_04_Gb", typeof(AudioClip)) as AudioClip;
			case Chord.GM:
			case Chord.Gm: return Resources.Load("Oud/Oud_4/Oud_04_G", typeof(AudioClip)) as AudioClip;
			case Chord.AbM:
			case Chord.Abm: return Resources.Load("Oud/Oud_4/Oud_04_Ab", typeof(AudioClip)) as AudioClip;
		}
		Debug.LogError("Couldn't load a clip for chord " + chord + "  with loop Oud 04");
		return null;
	}

	private static AudioClip GetDerbakki_1AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-001", typeof(AudioClip)) as AudioClip;
	}

	private static AudioClip GetDerbakki_2AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-002", typeof(AudioClip)) as AudioClip;
	}

	private static AudioClip GetDerbakki_3AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-003", typeof(AudioClip)) as AudioClip;
	}

	private static AudioClip GetDerbakki_4AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-004", typeof(AudioClip)) as AudioClip;
	}
}
