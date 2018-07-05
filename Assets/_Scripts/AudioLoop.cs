using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLoop {

	/// LoopName stores all the different loop name types.
	public enum LoopName {
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
		A,
		Bb,
		B,
		C,
		Db,
		D,
		Eb,
		E,
		F,
		Gb,
		G,
		Ab
	}

	// The name of the loop to play, e.g. 'Oud_1'
	public LoopName loopToPlay;
	// The length of this loop in beats
	public int numBeats;

	// Plays this loop with a given chord at a given time
	public AudioSource PlayLoop(double dspTime, Chord chord, SoundEvent soundEvent){
		var source = soundEvent.gameObject.AddComponent<AudioSource>();
		var clip = AudioClipForChord(chord);
		// Debug.Log("Clip is null?: " + (clip == null));
		source.clip = clip;
		soundEvent.nextClipToPlay = source;
		soundEvent.PlaySoundAtNextGrid(dspTime);
		return source;
	}

	// Get the audio clip for the given chord for this audioLoop's loop to play
	AudioClip AudioClipForChord(Chord chord) {
		switch(loopToPlay) {
			case LoopName.Oud_1:
				switch(chord) {
					case Chord.A: return Resources.Load("Oud/Oud_1/Oud_01_A", typeof(AudioClip)) as AudioClip;
					case Chord.Bb: return Resources.Load("Oud/Oud_1/Oud_01_Bb", typeof(AudioClip)) as AudioClip;
					case Chord.B: return Resources.Load("Oud/Oud_1/Oud_01_B", typeof(AudioClip)) as AudioClip;
					case Chord.C: return Resources.Load("Oud/Oud_1/Oud_01_C", typeof(AudioClip)) as AudioClip;
					case Chord.Db: return Resources.Load("Oud/Oud_1/Oud_01_Db", typeof(AudioClip)) as AudioClip;
					case Chord.D: return Resources.Load("Oud/Oud_1/Oud_01_D", typeof(AudioClip)) as AudioClip;
					case Chord.Eb: return Resources.Load("Oud/Oud_1/Oud_01_Eb", typeof(AudioClip)) as AudioClip;
					case Chord.E: return Resources.Load("Oud/Oud_1/Oud_01_E", typeof(AudioClip)) as AudioClip;
					case Chord.F: return Resources.Load("Oud/Oud_1/Oud_01_F", typeof(AudioClip)) as AudioClip;
					case Chord.Gb: return Resources.Load("Oud/Oud_1/Oud_01_Gb", typeof(AudioClip)) as AudioClip;
					case Chord.G: return Resources.Load("Oud/Oud_1/Oud_01_G", typeof(AudioClip)) as AudioClip;
					case Chord.Ab: return Resources.Load("Oud/Oud_1/Oud_01_Ab", typeof(AudioClip)) as AudioClip;
				}
				break;
			case LoopName.Oud_2:
				switch(chord) {
					case Chord.A: return Resources.Load("Oud/Oud_2/Oud_02_A", typeof(AudioClip)) as AudioClip;
					case Chord.Bb: return Resources.Load("Oud/Oud_2/Oud_02_Bb", typeof(AudioClip)) as AudioClip;
					case Chord.B: return Resources.Load("Oud/Oud_2/Oud_02_B", typeof(AudioClip)) as AudioClip;
					case Chord.C: return Resources.Load("Oud/Oud_2/Oud_02_C", typeof(AudioClip)) as AudioClip;
					case Chord.Db: return Resources.Load("Oud/Oud_2/Oud_02_Db", typeof(AudioClip)) as AudioClip;
					case Chord.D: return Resources.Load("Oud/Oud_2/Oud_02_D", typeof(AudioClip)) as AudioClip;
					case Chord.Eb: return Resources.Load("Oud/Oud_2/Oud_02_Eb", typeof(AudioClip)) as AudioClip;
					case Chord.E: return Resources.Load("Oud/Oud_2/Oud_02_E", typeof(AudioClip)) as AudioClip;
					case Chord.F: return Resources.Load("Oud/Oud_2/Oud_02_F", typeof(AudioClip)) as AudioClip;
					case Chord.Gb: return Resources.Load("Oud/Oud_2/Oud_02_Gb", typeof(AudioClip)) as AudioClip;
					case Chord.G: return Resources.Load("Oud/Oud_2/Oud_02_G", typeof(AudioClip)) as AudioClip;
					case Chord.Ab: return Resources.Load("Oud/Oud_2/Oud_02_Ab", typeof(AudioClip)) as AudioClip;
				}
				break;
			case LoopName.Oud_3:
				switch(chord) {
					case Chord.A: return Resources.Load("Oud/Oud_3/Oud_03_A", typeof(AudioClip)) as AudioClip;
					case Chord.Bb: return Resources.Load("Oud/Oud_3/Oud_03_Bb", typeof(AudioClip)) as AudioClip;
					case Chord.B: return Resources.Load("Oud/Oud_3/Oud_03_B", typeof(AudioClip)) as AudioClip;
					case Chord.C: return Resources.Load("Oud/Oud_3/Oud_03_C", typeof(AudioClip)) as AudioClip;
					case Chord.Db: return Resources.Load("Oud/Oud_3/Oud_03_Db", typeof(AudioClip)) as AudioClip;
					case Chord.D: return Resources.Load("Oud/Oud_3/Oud_03_D", typeof(AudioClip)) as AudioClip;
					case Chord.Eb: return Resources.Load("Oud/Oud_3/Oud_03_Eb", typeof(AudioClip)) as AudioClip;
					case Chord.E: return Resources.Load("Oud/Oud_3/Oud_03_E", typeof(AudioClip)) as AudioClip;
					case Chord.F: return Resources.Load("Oud/Oud_3/Oud_03_F", typeof(AudioClip)) as AudioClip;
					case Chord.Gb: return Resources.Load("Oud/Oud_3/Oud_03_Gb", typeof(AudioClip)) as AudioClip;
					case Chord.G: return Resources.Load("Oud/Oud_3/Oud_03_G", typeof(AudioClip)) as AudioClip;
					case Chord.Ab: return Resources.Load("Oud/Oud_3/Oud_03_Ab", typeof(AudioClip)) as AudioClip;
				}
				break;
			case LoopName.Oud_4:
				switch(chord) {
					case Chord.A: return Resources.Load("Oud/Oud_4/Oud_04_A", typeof(AudioClip)) as AudioClip;
					case Chord.Bb: return Resources.Load("Oud/Oud_4/Oud_04_Bb", typeof(AudioClip)) as AudioClip;
					case Chord.B: return Resources.Load("Oud/Oud_4/Oud_04_B", typeof(AudioClip)) as AudioClip;
					case Chord.C: return Resources.Load("Oud/Oud_4/Oud_04_C", typeof(AudioClip)) as AudioClip;
					case Chord.Db: return Resources.Load("Oud/Oud_4/Oud_04_Db", typeof(AudioClip)) as AudioClip;
					case Chord.D: return Resources.Load("Oud/Oud_4/Oud_04_D", typeof(AudioClip)) as AudioClip;
					case Chord.Eb: return Resources.Load("Oud/Oud_4/Oud_04_Eb", typeof(AudioClip)) as AudioClip;
					case Chord.E: return Resources.Load("Oud/Oud_4/Oud_04_E", typeof(AudioClip)) as AudioClip;
					case Chord.F: return Resources.Load("Oud/Oud_4/Oud_04_F", typeof(AudioClip)) as AudioClip;
					case Chord.Gb: return Resources.Load("Oud/Oud_4/Oud_04_Gb", typeof(AudioClip)) as AudioClip;
					case Chord.G: return Resources.Load("Oud/Oud_4/Oud_04_G", typeof(AudioClip)) as AudioClip;
					case Chord.Ab: return Resources.Load("Oud/Oud_4/Oud_04_Ab", typeof(AudioClip)) as AudioClip;
				}
				break;
			case LoopName.Derbakki_1:
				return Resources.Load("Derbakki/Derbakki-001", typeof(AudioClip)) as AudioClip;
			case LoopName.Derbakki_2:
				return Resources.Load("Derbakki/Derbakki-002", typeof(AudioClip)) as AudioClip;
			case LoopName.Derbakki_3:
				return Resources.Load("Derbakki/Derbakki-003", typeof(AudioClip)) as AudioClip;
			case LoopName.Derbakki_4:
				return Resources.Load("Derbakki/Derbakki-004", typeof(AudioClip)) as AudioClip;
		}

		// If we don't find the clip to play, something is misconfigured
		Debug.LogError("Couldn't load a clip for chord " + chord + " with loop name " + loopToPlay);
		return null;
	}
}
