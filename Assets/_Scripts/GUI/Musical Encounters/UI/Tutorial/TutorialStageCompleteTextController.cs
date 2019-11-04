using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialStageCompleteTextController : MonoBehaviour {

	public string nextLevelText;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.playerWon, DidWin);
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.playerLost, DidLose);
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.playerWon, DidWin);
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.playerLost, DidLose);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DidWin(object sender, object arg) {
		StartCoroutine(LoadNextLevel());
	}

	void DidLose(object sender, object arg) {
		StartCoroutine(LoadNextLevel());
	}

	IEnumerator LoadNextLevel() {
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(nextLevelText);
	}
}
