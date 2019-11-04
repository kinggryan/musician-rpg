using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChangedText : MonoBehaviour {

	private Animator animator;
	private UnityEngine.UI.Text text;

	void Awake() {
		animator = GetComponent<Animator>();
		text = GetComponent<UnityEngine.UI.Text>();
		NotificationBoard.AddListener(Scorekeeper.Notifications.scoreUpdated, ScoreUpdated);
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(Scorekeeper.Notifications.scoreUpdated, ScoreUpdated);
	}

	// Use this for initialization
	void ScoreUpdated(object scorekeeper, object args) {
		var scoreArgs = (Scorekeeper.Notifications.ScoreUpdatedArgs)args;
		var strToPrint = "";
		if(scoreArgs.rhythmPointGain > 0) {
			strToPrint += "+rhythm\n";
		} else if(scoreArgs.rhythmPointGain < 0) {
			strToPrint += "-rhythm\n";
		}
		if(scoreArgs.emotionPointGain > 0) {
			strToPrint += "+emotion\n";
		} else if(scoreArgs.emotionPointGain < 0) {
			strToPrint += "-emotion\n";
		}
		if(scoreArgs.wasStale) {
			strToPrint += "-stale";
		}
		PrintPhrase(strToPrint);
	}
	
	// Update is called once per frame
	void PrintPhrase(string phrase) {
		animator.SetTrigger("Show");
		text.text = phrase;
	}
}
