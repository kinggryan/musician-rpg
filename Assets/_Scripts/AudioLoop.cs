using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioLoop {

	/// LoopName stores all the different loop name types.
	public enum LoopName {
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

	public enum Chord {
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

	// The name of the loop to play, e.g. 'Oud_1'
	public LoopName loopToPlay;
	// The length of this loop in beats
	public int numBeats;

	public AudioLoop(LoopName loopName) {
		loopToPlay = loopName;
		numBeats = AudioLoop.NumBeatsForLoopName(loopName);
	}

	// Plays this loop with a given chord at a given time
	public AudioSource PlayLoop(double dspTime, Chord chord, SoundEvent soundEvent, AudioMixerGroup output){
		var source = soundEvent.gameObject.AddComponent<AudioSource>();
		var clip = AudioClipForChord(chord);
		// Debug.Log("Clip is null?: " + (clip == null));
		source.outputAudioMixerGroup = output;
		source.clip = clip;
		soundEvent.nextClipToPlay = source;
		soundEvent.PlaySoundAtNextGrid(dspTime);
		return source;
	}


	// Get the audio clip for the given chord for this audioLoop's loop to play
	AudioClip AudioClipForChord(Chord chord) {
		switch(loopToPlay) {
			case LoopName.Flute_01:
				return GetFlute_01AudioClip(chord);
				break;
			case LoopName.Flute_02:
				return GetFlute_02AudioClip(chord);
				break;
			case LoopName.Flute_03:
				return GetFlute_03AudioClip(chord);
				break;
			case LoopName.Flute_04:
				return GetFlute_04AudioClip(chord);
				break;			
			case LoopName.Guitar_01:
				return GetGuitar_01AudioClip(chord);
				break;
			case LoopName.Guitar_02:
				return GetGuitar_02AudioClip(chord);
				break;
			case LoopName.Oud_1:
				return GetOud_1AudioClip(chord);
				break;
			case LoopName.Oud_2:
				return GetOud_2AudioClip(chord);
				break;
			case LoopName.Oud_3:
				return GetOud_3AudioClip(chord);
				break;
			case LoopName.Oud_4:
				return GetOud_4AudioClip(chord);
				break;
			case LoopName.Derbakki_1:
				return GetDerbakki_1AudioClip(chord);
			case LoopName.Derbakki_2:
				return GetDerbakki_2AudioClip(chord);
			case LoopName.Derbakki_3:
				return GetDerbakki_3AudioClip(chord);
			case LoopName.Derbakki_4:
				return GetDerbakki_4AudioClip(chord);
		}

		// If we don't find the clip to play, something is misconfigured
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetFlute_01AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetFlute_02AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}
	AudioClip GetFlute_03AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetFlute_04AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetGuitar_01AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetGuitar_02AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	

	AudioClip GetOud_1AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}
	AudioClip GetOud_2AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetOud_3AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetOud_4AudioClip(Chord chord){
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
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}

	AudioClip GetDerbakki_1AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-001", typeof(AudioClip)) as AudioClip;
	}

	AudioClip GetDerbakki_2AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-002", typeof(AudioClip)) as AudioClip;
	}

	AudioClip GetDerbakki_3AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-003", typeof(AudioClip)) as AudioClip;
	}

	AudioClip GetDerbakki_4AudioClip(Chord chord){
		return Resources.Load("Derbakki/Derbakki-004", typeof(AudioClip)) as AudioClip;
	}

	public static LoopName LoopNameForString(string loopName) {
		switch(loopName) {
			case "Oud_1": return LoopName.Oud_1;
			case "Oud_2": return LoopName.Oud_2;
			case "Oud_3": return LoopName.Oud_3;
			case "Oud_4": return LoopName.Oud_4;
			case "Derbakki_1": return LoopName.Derbakki_1;
			case "Derbakki_2": return LoopName.Derbakki_2;
			case "Derbakki_3": return LoopName.Derbakki_3;
			case "Derbakki_4": return LoopName.Derbakki_4;
			case "Guitar_01": return LoopName.Guitar_01;
			case "Guitar_02": return LoopName.Guitar_02;
			case "Flute_01": return LoopName.Flute_01;
			case "Flute_02": return LoopName.Flute_02;
			case "Flute_03": return LoopName.Flute_03;
			case "Flute_04": return LoopName.Flute_04;
		}

		Debug.LogError("Tried to load loop for '" + loopName + "' but one doesn't exist");
		return LoopName.Oud_1;
	}

	public static int NumBeatsForLoopName(LoopName loopName) {
		switch(loopName) {
			case LoopName.Derbakki_1:
			case LoopName.Derbakki_2:
			case LoopName.Derbakki_3:
			case LoopName.Derbakki_4:
				return 1;
			case LoopName.Oud_1:
			case LoopName.Oud_2:
			case LoopName.Oud_3:
			case LoopName.Oud_4:
				return 2;
			case LoopName.Guitar_01:
			case LoopName.Guitar_02:
				return 4;
			case LoopName.Flute_01:
			case LoopName.Flute_02:
			case LoopName.Flute_03:
			case LoopName.Flute_04:
				return 8;
		}

		Debug.LogError("Tried to get num beats for for '" + loopName + "' but can't");
		return 0;
	}

	public static Chord ChordForString(string chord) {
		switch(chord) {
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
		return Chord.Am;
	}

	// Returns the rhythm string for this audioloop
	public string GetRhythmString() {
		switch(loopToPlay) {
			case LoopName.Oud_1: return "1101";
			case LoopName.Oud_2: return "1010";
			case LoopName.Oud_3: return "1011";
			case LoopName.Oud_4: return "1110";
			case LoopName.Derbakki_1: return "10";
			case LoopName.Derbakki_2: return "01";
			case LoopName.Derbakki_3: return "10";
			case LoopName.Derbakki_4: return "11";
		}

		Debug.LogError("Couldn't find rhythm string for loop to play " + loopToPlay);
		return "";
	}
}
