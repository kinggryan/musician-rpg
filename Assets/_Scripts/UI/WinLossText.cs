using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLossText : MonoBehaviour, IScorekeeperListener {

	UnityEngine.UI.Text text;

	void Awake() {
		text = GetComponent<UnityEngine.UI.Text>();
		text.text = "";
		var scorekeeper = Object.FindObjectOfType<Scorekeeper>();
		scorekeeper.AddListener(this);
	}

	// Use this for initialization
	public void DidChangeScore(float score) {}
	public void DidSetMaxScore(float maxScore) {}

	public void DidWin() {
		text.text = "Nice one! You Won!";
	}

	public void DidLose() {
		text.text = "Better luck next time... you lost....";
	}
}
