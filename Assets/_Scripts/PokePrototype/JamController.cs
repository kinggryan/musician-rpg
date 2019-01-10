using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JamController : MonoBehaviour {


	public Move activeMove;
	public int activeEmo;
	public int score = 0;
	public int hp;
	public int noOfEmos;
	public bool playerGoesFirst;
	public string[] emoNames;
	public Slider jammageBar;
	public Text turnDisplay;
	public Text emoDisplay;
	public Text dialogueDisplay;
	public MusicalEncounterManager musicalEncounterManager;
	public PlayerCountoffDisplay countoffDisplay;
	public string songFileName;
	public bool firstMove = true;
	public float bpm = 120;
	public AIJamController ai;
	enum Turn {Player,NPC};
	Turn turn;
	
	
	private PlayerJamMenu player;
	private bool gameOver;
	private DialogueController dialogueController;


	// Use this for initialization
	void Awake () {
		firstMove = true;
		ai = Object.FindObjectOfType<AIJamController>();
		player = Object.FindObjectOfType<PlayerJamMenu>();
		dialogueController = Object.FindObjectOfType<DialogueController>();
		jammageBar.maxValue = hp;
		jammageBar.minValue = hp * -1;
		if(playerGoesFirst){
			turn = Turn.Player;
			Debug.Log("Turn: " + turn);
			turnDisplay.text = "Turn: Player";
			player.isPlayerTurn = true;
		}else{
			turn = Turn.NPC;
			Debug.Log("Turn: " + turn);
			turnDisplay.text = "Turn: NPC";
			player.isPlayerTurn = false;
			ai.MakeMove();		
		}
		
	}
	void Start(){
		musicalEncounterManager.StartedMusicalEncounter(songFileName, countoffDisplay);
		dialogueController.UpdateDialogue("Jammer wants to jam!",2);
	}

	public void StartSong(){
		musicalEncounterManager.countoffController.StartCountoff();
	}

	public void UpdateScore(){
		switch(turn) {
			case Turn.Player:
				score += activeMove.power;
				if (activeMove.emo == activeEmo + 1 || activeEmo >= noOfEmos && activeMove.emo == 0 || activeMove.emo == activeEmo){
					score += activeMove.power;
					StartCoroutine(DisplayBonusDialogue("It's super effective!", 1));
				}
				break;
			case Turn.NPC:
				score -= activeMove.power;
				if (activeMove.emo == activeEmo + 1 || activeEmo >= noOfEmos && activeMove.emo == 0 || activeMove.emo == activeEmo){
					score -= activeMove.power;
				}
				break;
			}
		activeEmo = activeMove.emo;
		UpdateEmoDisplayText();
		//Debug.Log("Score: " + score);
		jammageBar.value = score;
		if(score <= hp * -1){
			dialogueController.UpdateDialogue("You can't handle the jammage!",2);
			StartCoroutine(DisplayBonusDialogue("You passed out!", 2));
			gameOver = true;
			musicalEncounterManager.CompletedMusicalEncounter(MusicalEncounterManager.SuccessLevel.TotalFailure);
		}else if(score >= hp){
			dialogueController.UpdateDialogue("Excellent jammage!",2);
			StartCoroutine(DisplayBonusDialogue("You outjammed Jammer!", 2));
			gameOver = true;
			musicalEncounterManager.CompletedMusicalEncounter(MusicalEncounterManager.SuccessLevel.Pass);
		}

		
	}

	IEnumerator DisplayBonusDialogue(string dialogueToDispay, float timeToWait){
		yield return new WaitForSeconds(timeToWait);
		dialogueController.UpdateDialogue(dialogueToDispay,2);

	}

	void UpdateEmoDisplayText(){
		emoDisplay.text = "Emotion: " + emoNames[activeEmo];
	}

	public void EndTurn(){
		if(!gameOver){
			switch(turn){
				
				case Turn.Player:
					turnDisplay.text = "Turn: NPC";
					player.isPlayerTurn = false;
					//ai.MakeMove();
					ai.isNPCTurn = true;
					//Debug.Log("Player Turn Ended");
					turn = Turn.NPC;
					break;
				case Turn.NPC:
					turnDisplay.text = "Turn: Player";
					player.isPlayerTurn = true;
					ai.isNPCTurn = false;
					//Debug.Log("NPC Turn Ended");
					turn = Turn.Player;
					break;
			}
		}
	}
}
