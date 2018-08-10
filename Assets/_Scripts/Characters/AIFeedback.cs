using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIFeedback : MonoBehaviour {

	private Text text;
	private float displayTimer;
	private bool isTextEnabled;
	private float displayTime;

	void Start(){
		text = gameObject.GetComponent<Text>();
		text.enabled = false;
	}

	public void DisplayText(string textToDisplay, float timeToDisplayText){
		text.enabled = true;
		displayTimer = 0;
		displayTime = timeToDisplayText;
		isTextEnabled = true;
		text.text = textToDisplay;
		
	}

	void Update(){
		if(isTextEnabled){
			displayTimer += Time.deltaTime;
			if (displayTimer >= displayTime){
				isTextEnabled = false;
				text.enabled = false;
			}
		}
	}
}
