﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIJamController : MonoBehaviour {

	public bool isNPCTurn;

	public CharacterJamController characterJamController;
	private JamController jamController;
	public Text currentMoveDisplay;
	public AIMIDIController aiMidiController;
	private bool lastMoveWasStyleChange = true;
	private DialogueController dialogueController;
	public int currentBeat = 1;
	private int moveFrequency = 16;
	

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

	// void Update(){
	// 	if(isNPCTurn && dialogueController.dialogueQueue.Count <= 0){
	// 		PickMove();
	// 	}
	// }

	public IEnumerator WaitThenPickMove(){
		yield return new WaitForSeconds(5);
		if(jamController.inEncounter){
			PickMove();
		}
	}

	public void OnBeat(){
		if(currentBeat < moveFrequency){
			currentBeat ++;
		}else{
			currentBeat = 1;
		}
//		Debug.Log("AI Beat " + currentBeat);
		if(jamController.inEncounter){
			if(currentBeat == moveFrequency - 5){
				PickMove();
			}
		}
	}

	public void ResetBeatCounter(){
		currentBeat = 1;
	}

	bool NPCOutOfPP(){
//		Debug.Log("Checking if NPC has PP");
		int currentTotalPP = CurrentTotalPP();
		if(currentTotalPP > 0){
			return false;
		}else{
			return true;
			Debug.Log("NPC out of PP!");
		}
	}

	int CurrentTotalPP(){
		int currentTotalPP = 0;
		MoveSets moveSets = characterJamController.moveSets;
		foreach(MoveSet moveSet in moveSets.moveSets){
			currentTotalPP += PPInMoveSet(moveSet);
		}
		return currentTotalPP;
	}

	int PPInMoveSet(MoveSet moveSet){
		int ppInMoveSet = 0;
		foreach(Move move in moveSet.moves){
				ppInMoveSet += move.Pp;
			}
		return ppInMoveSet;
	}

	public void SetFirstMove(){
		characterJamController.SelectMove(0);
		Move[] moveSet = characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves;
		aiMidiController.SetCurrentLoopWithName(moveSet[0].loopName);
	}

	void PickMove(){
		if(jamController.firstMove){
			Debug.Log("First move!!");
			jamController.StartSong();
			jamController.firstMove = false;
		}
		Move[] moveSet = characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves;
		int move = Random.Range(0,characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves.Length);
			Debug.Log("AI Selecting Move");
			
				characterJamController.SelectMove(move);
				//currentMoveDisplay.text = moveSet[move].name;
				aiMidiController.SetCurrentLoopWithName(moveSet[move].loopName);
				//dialogueController.UpdateDialogue("NPC used " + characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves[move].name + "!", 2);
				lastMoveWasStyleChange = false;
				jamController.aiCurrentPower = moveSet[move].power;
				//MakeMove();
			
	}

	// void PickMove(){
	// 	if(jamController.firstMove){
	// 		Debug.Log("First move!!");
	// 		jamController.StartSong();
	// 		jamController.firstMove = false;
	// 	}
	// 	Move[] moveSet = characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves;
	// 	int totalPp = 0;
	// 	foreach(Move moveToCheck in moveSet){
	// 		totalPp ++;
	// 	}
	// 	if(NPCOutOfPP()){
	// 		dialogueController.UpdateDialogue("NPC out of jams!", 2);
	// 		Debug.Log("NPC out of moves!");
	// 		//jamController.EndTurn();
	// 	}else if(totalPp <= 0){
	// 		Debug.Log("AI Changing Moveset cuz Out Of PP");
	// 		int newMoveSetIndex = Random.Range(0,characterJamController.moveSets.moveSets.Length);
	// 		MoveSet newMoveSet = characterJamController.moveSets.moveSets[newMoveSetIndex];
	// 		if(PPInMoveSet(newMoveSet) > 0){
	// 			characterJamController.ChangeMoveSet(newMoveSetIndex);
	// 			dialogueController.UpdateDialogue("NPC changed styles!", 2);
	// 			lastMoveWasStyleChange = true;
	// 			MakeMove();
	// 		}else{
	// 			Debug.Log("AI tried to pick moveset with no PP");
	// 			PickMove();
	// 		}
	// 	}else if(!lastMoveWasStyleChange){
	// 		int move = Random.Range(0,characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves.Length + 1);
	// 		if (move < moveSet.Length){
	// 			Debug.Log("AI Selecting Move");
	// 			if(moveSet[move].Pp > 0){
	// 				characterJamController.SelectMove(move);
	// 				currentMoveDisplay.text = moveSet[move].name;
	// 				aiMidiController.SetCurrentLoopWithName(moveSet[move].loopName);
	// 				dialogueController.UpdateDialogue("NPC used " + characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves[move].name + "!", 2);
	// 				lastMoveWasStyleChange = false;
	// 				jamController.aiCurrentPower = moveSet[move].power;
	// 				MakeMove();
	// 			}else{
	// 				Debug.Log("AI repicking move cuz out of PP");
	// 				PickMove();
	// 			}
	// 		}else{
	// 			Debug.Log("AI Changing Moveset");
	// 			int newMoveSet = Random.Range(0,characterJamController.moveSets.moveSets.Length);
	// 			characterJamController.ChangeMoveSet(newMoveSet);
				
	// 			dialogueController.UpdateDialogue("NPC changed styles!", 2);
	// 			lastMoveWasStyleChange = true;
	// 			jamController.aiCurrentPower = moveSet[move].power;
	// 			MakeMove();
	// 		}
	// 	}else{
	// 		int move = Random.Range(0,characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves.Length);
	// 		Debug.Log("AI Selecting Move");
	// 		if(moveSet[move].Pp > 0){
	// 			characterJamController.SelectMove(move);
	// 			currentMoveDisplay.text = moveSet[move].name;
	// 			aiMidiController.SetCurrentLoopWithName(moveSet[move].loopName);
	// 			dialogueController.UpdateDialogue("NPC used " + characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves[move].name + "!", 2);
	// 			lastMoveWasStyleChange = false;
	// 			jamController.aiCurrentPower = moveSet[move].power;
	// 			MakeMove();
	// 		}else{
	// 			Debug.Log("AI repicking move cuz out of PP");
	// 			PickMove();
	// 		}
	// 	}
	// }

}
