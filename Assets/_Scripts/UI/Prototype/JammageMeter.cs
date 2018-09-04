using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammageMeter : GenericMeter {

	// Use this for initialization
	override protected void Awake () {
		base.Awake();

		NotificationBoard.AddListener(RPGGameplayManger.Notifications.updatedJammage, DidUpdateJammage);
		// TODO: Set this value in a real way
		maxValue = 16; 
	}

	void DidUpdateJammage(object sender, object arg) {
		value = (int)arg;
	}
}
