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

	void Awake(){
		moveSets = GetComponent<MoveSets>();
		jamController = Object.FindObjectOfType<JamController>();
	}
	
	public void ChangeMoveSet(int moveSetToSelect){
		currentMoveSet = moveSetToSelect;
		jamController.EndTurn();
	}
	public void SelectMove(int moveToSelect){
		noteParticles.Play();
		currentMove = moveSets.moveSets[currentMoveSet].moves[moveToSelect];
		currentMove.Pp = currentMove.Pp - 1;
		jamController.activeMove = currentMove;
		jamController.UpdateScore();
		jamController.EndTurn();
	}

}
