using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleCountoffDisplay : PlayerCountoffDisplay {

	public DialogueBubble dialogueBubble;

	void PulseWithText(string text) {
		// TODO: Use a different effect instead of typing for this text
		dialogueBubble.SetText(text,null);
	}

	public override void NextBeat() {
		// If not visible, set visible and set the text once visible
		if(!dialogueBubble.GetVisible()) {
			dialogueBubble.SetVisible(true, null);
		}

		// Increment the countoff
		countOff++;
		if(countOff > 4) {
			dialogueBubble.SetVisible(false,null);
		} else if(countOff > 0) {
			PulseWithText("" + countOff + "!");
		} else {
			PulseWithText("Ready?");
		}
	}

	public override void Reset() {
		// countOff = -1;
		// text.enabled = true;
		// text.text = "Hmm, why don't you try counting in again?";
	}
}
