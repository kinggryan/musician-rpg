using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterParticleController : MonoBehaviour {

	public ParticleSystem successCircle;
	public ParticleSystem turnSuccessPop;
	public ParticleSystem turnFailurePop;

	// public float successCircleSpeedIncrease;
	public float successCircleRateIncrease;

	private int vps = 0;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.updatedVictoryPoints, DidUpdateVictoryPoints);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DidUpdateVictoryPoints(object sender, object arg) {
		var newVPs = (int)arg;

		TurnCompleteWithPointDelta(newVPs - vps);

		vps = newVPs;
	}

	void TurnCompleteWithPointDelta(int delta) {
		if(delta > 0) {
			turnSuccessPop.Play();
			var emission = successCircle.emission;
			emission.rateOverTimeMultiplier += successCircleRateIncrease;
		} else {
			turnFailurePop.Play();
			var emission = successCircle.emission;
			emission.rateOverTimeMultiplier = Mathf.Max(0.2f, emission.rateOverTimeMultiplier - successCircleRateIncrease);
		}
	}

	public void StartSong() {
		successCircle.Play();
	}

	public void EndSong() {
		successCircle.Stop();
	}
}
