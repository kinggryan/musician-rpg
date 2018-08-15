using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyControlDisplay : MonoBehaviour {
	public Color highlightedColor;
	public PlayerMidiController playerMidiController;
	public string[] controlNames;

	// Use this for initialization
	void Start () {
		UpdateDisplayValues();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
			//UpdateDisplayValues();
		}
	}

	public void UpdateDisplayValues(){
		foreach(Transform child in transform){
			KeyControlValue childProperties = child.GetComponent<KeyControlValue>();
			int index = childProperties.index;
			Text text = child.GetComponent<Text>();
			//Highlight Active Control
			if(playerMidiController.keyControlIndex == index){
				text.color = highlightedColor;
				childProperties.isActive = true;
			}else{
				text.color = childProperties.color;
				childProperties.isActive = false;
			}

			//Set Text To Current Value
			if(index == 0){
				text.text = controlNames[index] + "\n" + playerMidiController.currentInstIndex.ToString();
			}else if(index == 1){
				text.text = controlNames[index] + "\n" + playerMidiController.midiPlayer.trackGateVelocity.ToString();
			}else if(index == 2){
				text.text = controlNames[index] + "\n" + playerMidiController.midiPlayer.playerVolume.ToString();
			}
		}
	}
}
