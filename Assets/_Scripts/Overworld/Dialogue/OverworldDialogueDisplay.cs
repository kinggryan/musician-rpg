using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldDialogueDisplay : MonoBehaviour {

	public delegate void SetTextCallback();

	private float lettersPerSecond = 30;
	private float letterTypeTimer = 0f;
	private string targetText = "";
	private SetTextCallback textCompleteCallback;
	[SerializeField]
	private UnityEngine.UI.Text text;

	// Update is called once per frame
	void Update () {
		letterTypeTimer -= Time.deltaTime;
		if(letterTypeTimer <= 0) {
			// Type more if needed
			if(text.text.Length < targetText.Length ) {
				text.text = text.text + targetText[text.text.Length];
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
		text.text = "";
		letterTypeTimer = 1/lettersPerSecond;
	}
}
