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
	
	public PlayerJamMenu player;
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
	public ScoreManager scoreManager;
	


	// Use this for initialization
	void Awake () {
		firstMove = true;
		ai = Object.FindObjectOfType<AIJamController>();
		player = Object.FindObjectOfType<PlayerJamMenu>();
		dialogueController = Object.FindObjectOfType<DialogueController>();
		emoManager = GetComponent<EmotionManager>();
		toggleInstrument = Object.FindObjectOfType<ToggleInstrument>();
		scoreManager = Object.FindObjectOfType<ScoreManager>();
		//jammageBar.maxValue = hp;
		//jammageBar.minValue = hp * -1;
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
			scoreManager.UpdateScores();
			scoreManager.inEncounter = true;
			if(scoreManager.fail){
				EndEncounter();
			}
		}else{
			scoreManager.inEncounter = false;
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
		score = 0;
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
		scoreManager.StopGraceTimer();
		scoreManager.fail = false;
		player.UnlockPlayerControls();
	}
	

	

	public bool ScoreIsBelowMin(){
		if(score <= hp * -1){
			return true;
		}else{
			return false;
		}
	}

	public bool ScoreIsAboveMax(){
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
	}

	public void EndTurn(){
		if(!gameOver && !soloPlay){
			switch(turn){
				case Turn.Player:
					player.isPlayerTurn = false;
					ai.isNPCTurn = true;
					turn = Turn.NPC;
					break;
				case Turn.NPC:
					player.isPlayerTurn = true;
					ai.isNPCTurn = false;
					turn = Turn.Player;
					break;
			}
		}
	}
}
