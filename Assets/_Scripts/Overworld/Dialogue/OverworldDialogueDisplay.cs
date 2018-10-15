using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldDialogueDisplay : MonoBehaviour {

	public delegate void SetTextCallback();

	private float lettersPerSecond = 30;
	private float letterTypeTimer = 0f;
	private string targetText = "";
	private int numLettersTyped = 0;
	private SetTextCallback textCompleteCallback;
	[SerializeField]
	protected UnityEngine.UI.Text text;

	// Update is called once per frame
	protected virtual void Update () {
		letterTypeTimer -= Time.deltaTime;
		if(letterTypeTimer <= 0) {
			// Type more if needed
			if(numLettersTyped < targetText.Length ) {
				numLettersTyped++;
				text.text = FormattedText(targetText, numLettersTyped);
				letterTypeTimer += 1/lettersPerSecond;
			} else if(textCompleteCallback != null) {
				// Do the text complete callback
				textCompleteCallback();
				textCompleteCallback = null;
			}
		}
	}

	public void SetText(string targetText, SetTextCallback textCompleteCallback) {
		this.targetText = targetText;
		this.textCompleteCallback = textCompleteCallback;
		text.text = FormattedText(targetText, 0);
		letterTypeTimer = 1/lettersPerSecond;
		numLettersTyped = 0;
	}

	private string FormattedText(string plaintext, int numLettersVisible) {
		return plaintext.Substring(0, numLettersVisible) + "<color=#00000000>" + plaintext.Substring(numLettersVisible, plaintext.Length - numLettersVisible) + "</color>";
	}
}
