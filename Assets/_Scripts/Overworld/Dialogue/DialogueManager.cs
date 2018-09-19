using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour {

	private Story story;

	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private RectTransform pcDialogueOptionsParent;
	[SerializeField]
	private OverworldDialogueDisplay npcDialogueDisplay;

	[SerializeField]
	private PlayerChoice buttonPrefab;

	private bool canContinueText;
	private List<PlayerChoice> choices = new List<PlayerChoice>();
	private int currentChoiceIndex = 0;

	void Awake () {
		
	}

	void Start() {
		canvas.enabled = false;
	}

	void Update() {
		if(story != null) {
			if(story.currentChoices.Count > 0) {
				if(Input.GetButtonDown("Left")) {
					choices[currentChoiceIndex].Unhighlight();
					currentChoiceIndex = Mathf.Max(0, currentChoiceIndex - 1);
					choices[currentChoiceIndex].Highlight();
				} else if(Input.GetButtonDown("Right")) {
					choices[currentChoiceIndex].Unhighlight();
					currentChoiceIndex = Mathf.Min(story.currentChoices.Count - 1, currentChoiceIndex + 1);
					choices[currentChoiceIndex].Highlight();
				} else if(Input.GetButtonDown("Select")) {
					story.ChooseChoiceIndex (currentChoiceIndex);
					RefreshView();
				}
			} else if(canContinueText && Input.GetButtonDown("Select")) {
				RefreshView();
			}
		}
	}

	public void StartStory (TextAsset inkJSONAsset) {
		var player = UnityEngine.Object.FindObjectOfType<PlayerController>();
		player.enabled = false;
		this.story = new Story (inkJSONAsset.text);
		canvas.enabled = true;
		RefreshView();
	}

	void EndStory() {
		this.story = null;
		canvas.enabled = false;
		var player = UnityEngine.Object.FindObjectOfType<PlayerController>();
		player.enabled = true;
	}

	void RefreshView () {
		RemoveChoices ();

		if (story.canContinue) {
			string text = story.Continue ().Trim();
			ShowNPCDialogue(text);
		} else {
			EndStory();
		}
	}

	//  when the story text callback happens
	// 	if there are story choices
	// 	make those appear
	// 	else if the story can continue
	// 	enable it to continue

	void ShowNPCDialogue (string text) {
		canContinueText = false;
		npcDialogueDisplay.SetText(text, FinishedShowingNPCDialogue);
	}

	void FinishedShowingNPCDialogue() {
		if(story.currentChoices.Count > 0) {
			choices.Clear();
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				PlayerChoice button = CreateChoiceView (choice.text.Trim ());
				choices.Add(button);
			}
			currentChoiceIndex = 0;
			choices[0].Highlight();
		} else {
			canContinueText = true;
		}
		// 	if there are story choices
		// 	make those appear
		// 	else if the story can continue
		// 	enable it to continue
	}

	PlayerChoice CreateChoiceView (string text) {
		PlayerChoice choice = Instantiate (buttonPrefab) as PlayerChoice;
		choice.transform.SetParent (pcDialogueOptionsParent, false);

		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		return choice;
	}

	void RemoveChoices () {
		int childCount = pcDialogueOptionsParent.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (pcDialogueOptionsParent.transform.GetChild (i).gameObject);
		}
	}
}
