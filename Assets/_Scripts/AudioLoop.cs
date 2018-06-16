using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class  AudioLoop : MonoBehaviour {

	public SoundEvent[] soundEvent;
	public AudioClip[] loopToPlay;
	public int numBeats;

	public void playLoop(){
		AudioSource source = soundEvent[0].gameObject.AddComponent<AudioSource>();
		source.clip = loopToPlay[0];
		soundEvent[0].nextClipToPlay = source;
		soundEvent[0].PlaySound();
	}
}
