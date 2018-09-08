using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLossText : MonoBehaviour, IScorekeeperListener {

	UnityEngine.UI.Text text;

	void Awake() {
		text = GetComponent<UnityEngine.UI.Text>();
		text.text = "";
		var scorekeeper = Object.FindObjectOfType<Scorekeeper>();
		if(scorekeeper != null)
			scorekeeper.AddListener(this);
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.playerWon, DidGetPlayerWonNotification);
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.playerLost, DidGetPlayerLostNotification);
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.playerWon, DidGetPlayerWonNotification);
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.playerLost, DidGetPlayerLostNotification);
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

	public void DidGetPlayerWonNotification(object sender, object arg) {
		DidWin();
	}

	public void DidGetPlayerLostNotification(object sender, object arg) {
		DidLose();
	}
}
