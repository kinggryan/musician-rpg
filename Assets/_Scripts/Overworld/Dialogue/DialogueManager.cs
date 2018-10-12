using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour {

	public GameObject player;
	public GameObject npc;
	public GameObject jamInterface;
	private Story story;
	private string musicalEncounterSongfileName;
	private TransitionManager transitionManager;
	private LevelManager levelManager;

	// [SerializeField]
	// private Canvas canvas;
	[SerializeField]
	private RectTransform pcDialogueOptionsParent;
	[SerializeField]
	private OverworldDialogueDisplay npcDialogueDisplay;

	[SerializeField]
	private PlayerChoice buttonPrefab;
	
	[SerializeField]
	private string musicalEncounterScene;

	private bool canContinueText;
	private List<PlayerChoice> choices = new List<PlayerChoice>();
	private int currentChoiceIndex = 0;
	private bool moveNPC = false;
	private Transform camera;

	void Awake () {
		transitionManager = UnityEngine.Object.FindObjectOfType<TransitionManager>();
		levelManager = UnityEngine.Object.FindObjectOfType<LevelManager>();
	}

	void Start() {
		//canvas.enabled = false;
		camera = GameObject.FindObjectOfType<Camera>().transform;
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
		
		if(moveNPC){
			if(npc.transform.position.x < player.transform.position.x + 8){
				npc.transform.position = new Vector3 (npc.transform.position.x + 0.1f,npc.transform.position.y,npc.transform.position.z);
			}else if(npc.transform.position.y < player.transform.position.y - 0.05){
				npc.transform.position = new Vector3 (npc.transform.position.x,npc.transform.position.y + 0.1f,npc.transform.position.z);
			}else if(npc.transform.position.y > player.transform.position.y + 0.05){
				npc.transform.position = new Vector3 (npc.transform.position.x,npc.transform.position.y - 0.1f,npc.transform.position.z);
			}else{
				Debug.Log("NPC moved");
				moveNPC = false;
			}
		}
	}

	public void StartStory (TextAsset inkJSONAsset, string musicalEncounterFilename, OverworldDialogueDisplay dialogueBox) {
		musicalEncounterSongfileName = musicalEncounterFilename;
		StartStory(new Story (inkJSONAsset.text), dialogueBox);
	}

	void StartStory(Story story, OverworldDialogueDisplay dialogueBox) {
		var player = UnityEngine.Object.FindObjectOfType<PlayerController>();
		player.enabled = false;
		this.story = story;

		// Get the canvas of the dialogue box
		npcDialogueDisplay = dialogueBox;
		npcDialogueDisplay.gameObject.SetActive(true);
		// var npcCanvas = dialogueBox.GetComponent<Canvas>();
		// npcCanvas.enabled = true;
		RefreshView();
	}

	void PauseStory() {
		npcDialogueDisplay.gameObject.SetActive(false);
	}

	public void ResumeStory() {
		npcDialogueDisplay.gameObject.SetActive(true);
		jamInterface.SetActive(false);
		RefreshView();
	}

	void EndStory() {
		this.story = null;
		// canvas.enabled = false;
		npcDialogueDisplay.gameObject.SetActive(false);
		var player = UnityEngine.Object.FindObjectOfType<PlayerController>();
		player.enabled = true;
	}

	void RefreshView () {
		RemoveChoices ();

		// If the current story point starts a musical encounter, start the musical encounter
		if(story.currentTags.Contains("start_encounter")) {
			canContinueText = false;
			StartMusicalEncounter();
			// There should always be one JUNK line after the start of a musical encounter, so we proceed here knowing that this line will be thrown away
			if(story.canContinue)
				story.Continue().Trim();
			return;
		}

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

	void StartMusicalEncounter() {
		// Load the game scene
		var player = UnityEngine.Object.FindObjectOfType<PlayerController>();

		PauseStory();
		var cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
		cameraController.TransitionToMusicalEncounterCam();

		moveNPC = true;
		MusicalEncounterManager.StartedMusicalEncounter(musicalEncounterSongfileName);
		jamInterface.SetActive(true);
		
		//transitionManager.LoadMusicalEncounterScene(musicalEncounterScene);
	}
}
