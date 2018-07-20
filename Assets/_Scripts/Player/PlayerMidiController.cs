﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMidiController : MonoBehaviour {

	public MIDIPlayer midiPlayer;
	PlayerMouseSpringInput mouseInput;

	// Use this for initialization
	void Start () {
		mouseInput = new PlayerMouseSpringInput();
		mouseInput.maxDistance = 400;
		mouseInput.tension = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Loop1")) {
			mouseInput.SetAnchor();
		}
		if(Input.GetButton("Loop1")) {
			var playRate = (mouseInput.GetMouseValue() + 1)/2f;
			midiPlayer.playbackRate = playRate;
		}
		if(Input.GetButtonDown("Loop2")) {
			mouseInput.SetAnchor();
		}
		if(Input.GetButton("Loop2")) {
			int veloGate = Mathf.RoundToInt(79 - ((mouseInput.GetMouseValue() + 1)/2f));
			midiPlayer.trackGateVelocity = veloGate;
		}
	}
}
