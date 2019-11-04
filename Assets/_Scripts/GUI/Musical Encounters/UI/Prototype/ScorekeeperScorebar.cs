using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorekeeperScorebar : MonoBehaviour, IScorekeeperListener {

	public float score;
	public float maxScore = 1;
	UnityEngine.UI.Slider slider;

	// Use this for initialization
	public void DidChangeScore(float score) {
		this.score = score;
		UpdateSlider();
	}

	public void DidSetMaxScore(float maxScore) {
		this.maxScore = maxScore;
		UpdateSlider();
	}

	public void DidWin() { }
	public void DidLose() { }

	void UpdateSlider() {
		Debug.Log("Score " + score + "/" + maxScore);
		slider.value = Mathf.Clamp(score/maxScore, 0, 1);
	}

	void Awake() {
		slider = GetComponent<UnityEngine.UI.Slider>();
		var scorekeeper = Object.FindObjectOfType<Scorekeeper>();
		scorekeeper.AddListener(this);
	}
}
