using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIFeedback : MonoBehaviour {

	private Text text;
	private float displayTimer;
	private bool isTextEnabled;
	private float displayTime;
	public Image avatar;
	private Color avatarStartColor = Color.white;
	private float colorTimer;
	private bool colorIsChanged;
	private float colorChangeDuration;

	void Start(){
		if(avatar){
			avatarStartColor = avatar.color;
		}
		text = gameObject.GetComponent<Text>();
		if(text != null){
			text.enabled = false;
		}
	}

	public void ChangeAvatarColorForDuration(Color color, float duration){
		colorIsChanged = true;
		colorChangeDuration = duration;
	}

	public void DisplayText(string textToDisplay, float timeToDisplayText, Color color){
		return;
		
		text.enabled = true;
		displayTimer = 0;
		displayTime = timeToDisplayText;
		isTextEnabled = true;
		text.color = color;
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
		if(colorIsChanged){
			colorTimer += Time.deltaTime;
			if (colorTimer >= colorChangeDuration){
				colorTimer = 0;
				if(avatar)
					avatar.color = avatarStartColor;
				colorIsChanged = false;
			}

		}
	}
}
