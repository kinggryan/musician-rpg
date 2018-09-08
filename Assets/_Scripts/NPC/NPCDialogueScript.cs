using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueScript : MonoBehaviour {

	public string[] dialogue;

	PlayerBPMCounter bpmCounter;
	UnityEngine.UI.Text text;
	private int dialogueIndex = -1;

	// Use this for initialization
	void Awake () {
		text = GetComponent<UnityEngine.UI.Text>();
		bpmCounter = Object.FindObjectOfType<PlayerBPMCounter>();
		bpmCounter.enabled = false;
	}

	void Start() {
		ShowNextText();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pulse")) {
			ShowNextText();
		}	
	}

	void ShowNextText() {
		dialogueIndex++;
		if(dialogueIndex < dialogue.Length) {
			text.text = dialogue[dialogueIndex];
		} else {
			bpmCounter.enabled = true;
			text.text = "Ready?";
			Object.Destroy(this);
		}
	}
}
