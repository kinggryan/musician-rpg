using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteParticleSpawner : MonoBehaviour {

	private ParticleSystem p;

	// Use this for initialization
	void Awake () {
		p = GetComponent<ParticleSystem>();
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.playerAndNPCRhythmMatchedOnPulse, DidPulse);
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.playerAndNPCRhythmMatchedOnPulse, DidPulse);
	}

	void DidPulse(object sender, object arg) {
		p.Emit(1);
	}
}
