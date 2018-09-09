﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountoffDisplay : MonoBehaviour {

	public UnityEngine.UI.Text text;
	int countOff = 0;

	void PulseWithText(string text) {
		// lol
		this.text.text = text;
	}

	public void NextBeat() {
		countOff++;
		if(countOff > 4) {
			text.enabled = false;
		} else if(countOff > 0) {
			PulseWithText("" + countOff + "!");
		} else {
			PulseWithText("Ready?");
		}
	}

	public void Reset() {
		countOff = -1;
		text.enabled = true;
		text.text = "Hmm, why don't you try counting in again?";
	}
}
