using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountoffDisplay : MonoBehaviour {

	public UnityEngine.UI.Text text;
	public Color textUnpulsedColor;
	public Color textPulsedColor;

	float bpm;
	int currentBeat;
	int finalBeat = 4;
	float timeTillNextBeat;
	float beatDuration;

	// Use this for initialization
	void Start () {
		text.text = "Let's get this thing goin'...";
	}
	
	// Update is called once per frame
	void Update () {
		if(currentBeat > 0) {
			timeTillNextBeat -= Time.deltaTime;
			if(timeTillNextBeat <= Time.deltaTime) {
				timeTillNextBeat += beatDuration;
				NextBeat();
			}
		}
	}

	void DidStartCountoff(float bpm) {
		this.bpm = bpm;
		currentBeat = 1;
		beatDuration = 60/bpm;
		timeTillNextBeat = beatDuration;
		PulseWithText("" + currentBeat);
	}

	void PulseWithText(string text) {
		// lol
		this.text.text = text;
		// this.text.fontSize = textPulsedFontSize;

	}

	void NextBeat() {
		currentBeat++;
		if(currentBeat > finalBeat) {
			text.enabled = false;
			Object.Destroy(this);
		} else {
			PulseWithText(text.text = "" + currentBeat);
		}
	}
}
