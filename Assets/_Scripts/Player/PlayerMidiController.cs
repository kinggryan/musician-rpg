using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMidiController : MonoBehaviour {

	public MIDIPlayer midiPlayer;
	public CharacterAnimationManager animationManager;
	PlayerMouseSpringInput mouseInput;
	
	float songBPM = 180f;

	float targetBPM;
	float currentBPM;

	int maxGate = 80;
	int minGate = 78;

	float maxVolume = 1.5f;
	float minVolume = 0.25f;

	// Use this for initialization
	void Start () {
		mouseInput = new PlayerMouseSpringInput();
		mouseInput.maxDistance = 250;
		mouseInput.tension = 2;

		targetBPM = songBPM;
		currentBPM = songBPM;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Loop2") || Input.GetButtonDown("Loop3")) {
			mouseInput.SetAnchor();
		}
		if(Input.GetButton("Loop2")) {
			int veloGate = maxGate - Mathf.RoundToInt((maxGate - minGate)*((mouseInput.GetMouseValue() + 1)/2f));
			Debug.Log("Gate: " + veloGate);
			midiPlayer.trackGateVelocity = veloGate;
		}
		if(Input.GetButton("Loop3")) {
			float volume = minVolume + (maxVolume - minVolume)*((mouseInput.GetMouseValue() + 1)/2f);
			midiPlayer.playerVolume = volume;
		}

		// Mathf.L
		currentBPM = Mathf.Lerp(currentBPM, targetBPM, 5*Time.deltaTime);
		midiPlayer.playbackRate = Mathf.Clamp(currentBPM / songBPM,0.3f,1.2f);
	}

	void DidChangeBPM(double bpm) {
		targetBPM = (float)bpm;
		
		if(!midiPlayer.IsPlaying()) {
			StartSongWithBPM(targetBPM);
		}
	}

	void StartSongWithBPM(float bpm) {
		currentBPM = bpm;
		animationManager.SetBPM(bpm);
		animationManager.DidStartSong();
		animationManager.UpdateGroove(1f);
		midiPlayer.Play();
	}
}
