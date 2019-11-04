using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammageThresholdMeter : GenericMeter {

	// Use this for initialization
	override protected void Awake () {
		base.Awake();

		// NotificationBoard.AddListener(RPGGameplayManger.Notifications.setJammageThreshold, DidUpdateJammageThreshold);
		// TODO: Set this value in a real way
		maxValue = 16; 
	}

	void OnDestroy() {
		// NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.setJammageThreshold, DidUpdateJammageThreshold);
	}

	void DidUpdateJammageThreshold(object sender, object arg) {
		value = (int)arg;
	}
}
