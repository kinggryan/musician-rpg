using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticCountoffController : MonoBehaviour {

	public float bpm;
	public PlayerCountoffDisplay countoffDisplay;

	private bool startedCountoff;
	private float nextBeatTimer;
	private int currentBeat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(startedCountoff) {
			nextBeatTimer -= Time.deltaTime;
			if(nextBeatTimer <= 0)
				Countoff();
		} else {
			if(Input.GetButtonDown("Select")) {
				StartCountoff();
			}
		}
	}

	void StartCountoff() {
		startedCountoff = true;
		nextBeatTimer = 60 / bpm;
		currentBeat = 1;
		countoffDisplay.NextBeat();
	}

	void Countoff() {
		nextBeatTimer += 60 / bpm;
		currentBeat++;
		countoffDisplay.NextBeat();
		if(currentBeat > 4) {
			BroadcastMessage("DidChangeBPM",bpm,SendMessageOptions.DontRequireReceiver);
			Object.Destroy(this);
		}
	}
}
