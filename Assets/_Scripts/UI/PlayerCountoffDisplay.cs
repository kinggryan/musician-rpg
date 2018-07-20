using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountoffDisplay : MonoBehaviour {

	public UnityEngine.UI.Text text;
	int countOff = 0;

	void PulseWithText(string text) {
		// lol
		this.text.text = text;
		// this.text.fontSize = textPulsedFontSize;

	}

	public void NextBeat() {
		countOff++;
		if(countOff > 4) {
			text.enabled = false;
		} else {
			PulseWithText("" + countOff + "!");
		}
	}
}
