using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIconVPDisplay : MonoBehaviour {

	public GameObject happyEmote;
	public GameObject sadEmote;

	private int victoryPoints;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.updatedVictoryPoints, DidScoreVictoryPoints);
	}
	
	void Start() {
		happyEmote.SetActive(false);
		sadEmote.SetActive(false);
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.updatedVictoryPoints, DidScoreVictoryPoints);
	}

	// Update is called once per frame
	void DidScoreVictoryPoints(object sender, object arg) {
		var numPoints = (int)arg;
		if(numPoints > victoryPoints) {
			StartCoroutine(FlashIcon(happyEmote));
		} else if (numPoints < victoryPoints) {
			StartCoroutine(FlashIcon(sadEmote));
		}
		victoryPoints = numPoints;
	}

	IEnumerator FlashIcon(GameObject icon) {
		icon.SetActive(true);
		yield return new WaitForSeconds(3);
		icon.SetActive(false);
	}
}
