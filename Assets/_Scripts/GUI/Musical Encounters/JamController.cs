using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JamController : MonoBehaviour {

	[SerializeField]
	public bool soloPlay;
	public Move newMove;
	public EmotionManager.Emo activeEmo;
	public float score = 0;
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
	private PersistentInfo persistenInfo;
	private DialogueController dialogueController;
	private EmotionManager emoManager;
	public bool isPlaying;
	public float aiCurrentPower;
	public float playerCurrentPower;
	public bool inEncounter;
	private ToggleInstrument toggleInstrument;
	public bool win;


	// Use this for initialization
	void Awake () {
		firstMove = true;
		ai = Object.FindObjectOfType<AIJamController>();
		player = Object.FindObjectOfType<PlayerJamMenu>();
		dialogueController = Object.FindObjectOfType<DialogueController>();
		emoManager = GetComponent<EmotionManager>();
		toggleInstrument = Object.FindObjectOfType<ToggleInstrument>();
		jammageBar.maxValue = hp;
		jammageBar.minValue = hp * -1;
		if(playerGoesFirst){
			turn = Turn.Player;
//			Debug.Log("Turn: " + turn);
//			turnDisplay.text = "Turn: Player";
			player.isPlayerTurn = true;
		}else{
			turn = Turn.NPC;
			Debug.Log("Turn: " + turn);
			turnDisplay.text = "Turn: NPC";
			player.isPlayerTurn = false;
			//ai.MakeMove();		
		}
		persistenInfo = Object.FindObjectOfType<PersistentInfo>();
		player.player.moveSets.moveSets = persistenInfo.activeMoves;
		
	}
	void Start(){
		// songFileName = player.soloSong;
		// musicalEncounterManager.StartedMusicalEncounter(songFileName/*, countoffDisplay) */);
		LoadSong(player.soloSong);
	}

	void Update(){
		if(inEncounter){
			score -= playerCurrentPower * Time.deltaTime;
			score += aiCurrentPower * Time.deltaTime;
			jammageBar.value = score;
			if(ScoreIsAboveMax()){
				EndEncounter();
				win = true;
			}else if(ScoreIsBelowMin()){
				EndEncounter();
				win = false;
			}
		}
	}

	public void OnStart(){
		//musicalEncounterManager.StartedMusicalEncounter(songFileName/*, countoffDisplay */);
		if(soloPlay){
			dialogueController.UpdateDialogue("Solo Play!",2);	
		}else{
			dialogueController.UpdateDialogue("Jammer wants to jam!",2);
		}
	}

	public void LoadAndPlaySong(string songToLoad, bool npcEncounter){
		player.startMuted = true;
		ai.aiMidiController.mute = false;
		soloPlay = !npcEncounter;
		LoadSong(songToLoad);
		StartSong();
		firstMove = false;
	}

	public void LoadSong(string songToLoad){
		musicalEncounterManager.LoadSong(songToLoad);
	}

	public void ResetSong(){
		LoadSong(player.soloSong);
	}

	public void StartSong(){
		Debug.Log("Starting song");
		if(soloPlay){
			ai.aiMidiController.mute = true;
		}
		musicalEncounterManager.StartSongWithBPM(120);
		player.StartSong();
	}

	public void StopSong(){
		player.StopSong();
		firstMove = true;
		inEncounter = false;
		ai.ResetBeatCounter();
	}
	public void EndEncounter(){
		StopSong();
		toggleInstrument.HideInstrument();
	}
	

	public void UpdateScore(){
		if(!soloPlay){
			switch(turn) {
				case Turn.Player:
					score += newMove.power;
					if (emoManager.checkEmoStrengths(newMove.emo, activeEmo)){
						score += newMove.power;
						StartCoroutine(DisplayBonusDialogue("It's super effective!", 1));
					}
					break;
				case Turn.NPC:
					score -= newMove.power;
					if (emoManager.checkEmoStrengths(newMove.emo, activeEmo)){
						score -= newMove.power;
						StartCoroutine(DisplayBonusDialogue("It's super effective!", 1));
					}
					break;
				}
			activeEmo = newMove.emo;
			UpdateEmoDisplayText();
			//Debug.Log("Score: " + score);
			jammageBar.value = score;
			if(ScoreIsBelowMin()){
				dialogueController.UpdateDialogue("You can't handle the jammage!",2);
				StartCoroutine(DisplayBonusDialogue("You passed out!", 2));
				gameOver = true;
				musicalEncounterManager.CompletedMusicalEncounter(MusicalEncounterManager.SuccessLevel.TotalFailure);
			}else if(ScoreIsAboveMax()){
				dialogueController.UpdateDialogue("Excellent jammage!",2);
				StartCoroutine(DisplayBonusDialogue("You outjammed Jammer!", 2));
				gameOver = true;
				musicalEncounterManager.CompletedMusicalEncounter(MusicalEncounterManager.SuccessLevel.Pass);
			}
		}else{
			activeEmo = newMove.emo;
			UpdateEmoDisplayText();
		}
	}

	bool ScoreIsBelowMin(){
		if(score <= hp * -1){
			return true;
		}else{
			return false;
		}
	}

	bool ScoreIsAboveMax(){
		if(score >= hp){
			return true;
		}else{
			return false;
		}
	}

	IEnumerator DisplayBonusDialogue(string dialogueToDispay, float timeToWait){
		yield return new WaitForSeconds(timeToWait);
		dialogueController.UpdateDialogue(dialogueToDispay,2);

	}

	void UpdateEmoDisplayText(){
//		emoDisplay.text = "Emotion: " + activeEmo.ToString();
	}

	public void EndTurn(){
		if(!gameOver && !soloPlay){
			switch(turn){
				
				case Turn.Player:
					//turnDisplay.text = "Turn: NPC";
					player.isPlayerTurn = false;
					//ai.MakeMove();
					ai.isNPCTurn = true;
					//Debug.Log("Player Turn Ended");
					turn = Turn.NPC;
					break;
				case Turn.NPC:
					//turnDisplay.text = "Turn: Player";
					player.isPlayerTurn = true;
					ai.isNPCTurn = false;
					//Debug.Log("NPC Turn Ended");
					turn = Turn.Player;
					break;
			}
		}
	}
}
