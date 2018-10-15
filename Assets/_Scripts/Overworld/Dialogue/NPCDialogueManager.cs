using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour {

	private PlayerController player;

	[SerializeField]
	private TextAsset inkJSONAsset;
	[SerializeField]
	private string songFileName;
	[SerializeField]
	private DialogueBubble dialogueBox;
	private DialogueManager dialogueManager;

	// Use this for initialization
	void Awake () {
		player = Object.FindObjectOfType<PlayerController>();
		dialogueManager = Object.FindObjectOfType<DialogueManager>();
	}
	
	public bool CanStartConversation() {
		return true;
	}

	// Update is called once per frame
	public void StartConversation () {
		dialogueManager.StartStory(inkJSONAsset, songFileName, dialogueBox);
	}
}
