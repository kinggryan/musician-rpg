using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor.Animations;

public class Metronome : MonoBehaviour {

	public double bpm;

	private double beatLength;
	public int beatsSinceStart;
	private double startTime;
	public bool running;
	public bool playSound;

	public void StartMetro(float newBpm){
		bpm = newBpm;
		CalculateBeats();
		beatsSinceStart = 1;
		startTime = AudioSettings.dspTime;
		running = true;
		KeyControlValue[] keyVals = Object.FindObjectsOfType<KeyControlValue>();
		for(int i = 0; i <= keyVals.Length -1; i++){
			keyVals[i].animator.speed = (float)bpm / 120f;			
			if(!keyVals[i].isActive){
				keyVals[i].animator.enabled = false;
			}
			
		}
		//Debug.Log("Metro Started");

	}

	void CalculateBeats(){
		beatLength = (60/bpm) * 2;
	}


	
	
	void FixedUpdate () {
		if(running){
			double dspTime = AudioSettings.dspTime;
			double nextBeat = (beatsSinceStart * beatLength) + startTime;
			if ((nextBeat - dspTime) <= ((dspTime + Time.deltaTime) - nextBeat)){
				beatsSinceStart++;
				OnBeatPlayed();
				if(playSound){
					GetComponent<AudioSource>().Play();
				}
				//Debug.Log("tick");
			}
			
		}
	}

	void OnBeatPlayed(){
		KeyControlValue[] keyVals = Object.FindObjectsOfType<KeyControlValue>();
		for(int i = 0; i <= keyVals.Length -1; i++){
			keyVals[i].OnBeatPlayed((float)bpm);
			
		}
	}

	
}
