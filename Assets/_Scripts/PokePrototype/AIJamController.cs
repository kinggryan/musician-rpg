using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIJamController : MonoBehaviour {

	private CharacterJamController characterJamController;
	private JamController jamController;
	public Text currentMoveDisplay;
	public Text dialogueText;
	private AIMIDIController aiMidiController;
	private bool lastMoveWasStyleChange;

	// Use this for initialization
	void Awake () {
		characterJamController = GetComponent<CharacterJamController>();
		jamController = Object.FindObjectOfType<JamController>();
		aiMidiController = Object.FindObjectOfType<AIMIDIController>();
	}

	public void MakeMove(){
		StartCoroutine(WaitThenPickMove());
	}

	public IEnumerator WaitThenPickMove(){
		yield return new WaitForSeconds(2);
		PickMove();
	}

	void PickMove(){
		Move[] moveSet = characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves;
		int totalPp = 0;
		foreach(Move moveToCheck in moveSet){
			totalPp ++;
		}
		if(totalPp <= 0){
			Debug.Log("AI Changing Moveset");
			int newMoveSet = Random.Range(0,characterJamController.moveSets.moveSets.Length);
			characterJamController.ChangeMoveSet(newMoveSet);
			dialogueText.text = "NPC changed styles!";
			lastMoveWasStyleChange = true;
		}else if(!lastMoveWasStyleChange){
			int move = Random.Range(0,characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves.Length + 1);
			if (move < moveSet.Length){
				Debug.Log("AI Selecting Move");
				if(moveSet[move].Pp > 0){
					characterJamController.SelectMove(move);
					currentMoveDisplay.text = moveSet[move].name;
					aiMidiController.SetCurrentLoopWithName(moveSet[move].loopName);
					dialogueText.text = "NPC used " + characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves[move].name + "!";
					lastMoveWasStyleChange = false;
				}else{
					Debug.Log("AI repicking move cuz out of PP");
					PickMove();
				}
			}else{
				Debug.Log("AI Changing Moveset");
				int newMoveSet = Random.Range(0,characterJamController.moveSets.moveSets.Length);
				characterJamController.ChangeMoveSet(newMoveSet);
				dialogueText.text = "NPC changed styles!";
				lastMoveWasStyleChange = true;
			}
		}else{
			int move = Random.Range(0,characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves.Length);
			Debug.Log("AI Selecting Move");
			if(moveSet[move].Pp > 0){
				characterJamController.SelectMove(move);
				currentMoveDisplay.text = moveSet[move].name;
				aiMidiController.SetCurrentLoopWithName(moveSet[move].loopName);
				dialogueText.text = "NPC used " + characterJamController.moveSets.moveSets[characterJamController.currentMoveSet].moves[move].name + "!";
				lastMoveWasStyleChange = false;
			}else{
				Debug.Log("AI repicking move cuz out of PP");
				PickMove();
			}
		}
	}

}
