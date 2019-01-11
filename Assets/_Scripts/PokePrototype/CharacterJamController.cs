using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJamController : MonoBehaviour {

	// Use this for initialization
	public MoveSets moveSets;

	public int currentMoveSet = 0;
	public Move currentMove;
	public ParticleSystem noteParticles;
	private JamController jamController;
	private EmotionManager emoManager;

	void Awake(){
		moveSets = GetComponent<MoveSets>();
		jamController = Object.FindObjectOfType<JamController>();
		emoManager = FindObjectOfType<EmotionManager>();
	}
	
	public void ChangeMoveSet(int moveSetToSelect){
		currentMoveSet = moveSetToSelect;
		jamController.EndTurn();
	}
	public void SelectMove(int moveToSelect){
		currentMove = moveSets.moveSets[currentMoveSet].moves[moveToSelect];
		currentMove.Pp = currentMove.Pp - 1;
		PlayNoteParticles(currentMove.emo);
		jamController.newMove = currentMove;
		jamController.UpdateScore();
		jamController.EndTurn();
	}

	void PlayNoteParticles(EmotionManager.Emo emo){
		noteParticles.startColor = emoManager.GetEmoColor(emo);
		noteParticles.Play();
	}

}
