using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIJamController : MonoBehaviour {

	public bool isNPCTurn;

	private CharacterJamController characterJamController;
	private JamController jamController;
	public Text currentMoveDisplay;
	private AIMIDIController aiMidiController;
	private bool lastMoveWasStyleChange = true;
	private DialogueController dialogueController;
	

	// Use this for initialization
	void Awake () {
		characterJamController = GetComponent<CharacterJamController>();
		jamController = Object.FindObjectOfType<JamController>();
		dialogueController = Object.FindObjectOfType<DialogueController>();
		aiMidiController = Object.FindObjectOfType<AIMIDIController>();
	}

	public void MakeMove(){
		StartCoroutine(WaitThenPickMove());
	}

	void Update(){
		if(isNPCTurn && dialogueController.dialogueQueue.Count <= 0){
			PickMove();
		}
	}

	public IEnumerator WaitThenPickMove(){
		yield return new WaitForSeconds(2);
		PickMove();
	}

	void PickMove(){
		if(jamController.firstMove){
			Debug.Log("First move!!");
			jamController.StartSong();
			jamController.firstMove = false;
		}
		Move[] moveSet = characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves;
		int totalPp = 0;
		foreach(Move moveToCheck in moveSet){
			totalPp ++;
		}
		if(totalPp <= 0){
			Debug.Log("AI Changing Moveset");
			int newMoveSet = Random.Range(0,characterJamController.moveSets.moveSets.Length);
			characterJamController.ChangeMoveSet(newMoveSet);
			dialogueController.UpdateDialogue("NPC changed styles!", 2);
			lastMoveWasStyleChange = true;
		}else if(!lastMoveWasStyleChange){
			int move = Random.Range(0,characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves.Length + 1);
			if (move < moveSet.Length){
				Debug.Log("AI Selecting Move");
				if(moveSet[move].Pp > 0){
					characterJamController.SelectMove(move);
					currentMoveDisplay.text = moveSet[move].name;
					aiMidiController.SetCurrentLoopWithName(moveSet[move].loopName);
					dialogueController.UpdateDialogue("NPC used " + characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves[move].name + "!", 2);
					lastMoveWasStyleChange = false;
				}else{
					Debug.Log("AI repicking move cuz out of PP");
					PickMove();
				}
			}else{
				Debug.Log("AI Changing Moveset");
				int newMoveSet = Random.Range(0,characterJamController.moveSets.moveSets.Length);
				characterJamController.ChangeMoveSet(newMoveSet);
				
				dialogueController.UpdateDialogue("NPC changed styles!", 2);
				lastMoveWasStyleChange = true;
			}
		}else{
			int move = Random.Range(0,characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves.Length);
			Debug.Log("AI Selecting Move");
			if(moveSet[move].Pp > 0){
				characterJamController.SelectMove(move);
				currentMoveDisplay.text = moveSet[move].name;
				aiMidiController.SetCurrentLoopWithName(moveSet[move].loopName);
				dialogueController.UpdateDialogue("NPC used " + characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves[move].name + "!", 2);
				lastMoveWasStyleChange = false;
			}else{
				Debug.Log("AI repicking move cuz out of PP");
				PickMove();
			}
		}
	}

}
