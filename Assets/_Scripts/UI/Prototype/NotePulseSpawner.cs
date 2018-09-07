using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePulseSpawner : MonoBehaviour {

	public GameObject noteToSpawn;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.playerAndNPCRhythmMatchedOnPulse, DidPulse);
	}

	void DidPulse(object sender, object arg) {
		GameObject.Instantiate(noteToSpawn,transform.position,Quaternion.identity);
	}
}
