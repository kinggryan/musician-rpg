using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticCountoffController : MonoBehaviour {

	public static class Notifications {
		public static string countoffComplete = "countoffComplete";
	}

	public float bpm;
	public PlayerCountoffDisplay countoffDisplay;

	private bool startedCountoff;
	private float nextBeatTimer;
	private int currentBeat;

	// Update is called once per frame
	void Update () {
		if(startedCountoff) {
			nextBeatTimer -= Time.deltaTime;
			if(nextBeatTimer <= 0)
				Countoff();
		} else {
			if(Input.GetButtonDown("Select")) {
				StartCountoff();
			}
		}
	}

	void StartCountoff() {
		startedCountoff = true;
		nextBeatTimer = 60 / bpm;
		currentBeat = 1;
		countoffDisplay.NextBeat();
	}

	void Countoff() {
		nextBeatTimer += 60 / bpm;
		currentBeat++;
		countoffDisplay.NextBeat();
		if(currentBeat > 4) {
			NotificationBoard.SendNotification(Notifications.countoffComplete, this, bpm);
			currentBeat = 0;
			startedCountoff = false;
			this.enabled = false;
		}
	}
}
