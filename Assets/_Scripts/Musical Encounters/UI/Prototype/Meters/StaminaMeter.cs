using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaMeter : GenericMeter {

	// Use this for initialization
	override protected void Awake () {
		base.Awake();

		// NotificationBoard.AddListener(RPGGameplayManger.Notifications.updatedStamina, DidUpdateStamina);
		// NotificationBoard.AddListener(RPGGameplayManger.Notifications.updatedMaxStamina, DidUpdateMaxStamina);
	}

	void OnDestroy() {
		// NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.updatedStamina, DidUpdateStamina);
		// NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.updatedMaxStamina, DidUpdateMaxStamina);
	}

	void DidUpdateStamina(object sender, object arg) {
		value = (int)arg;
	}

	void DidUpdateMaxStamina(object sender, object arg) {
		maxValue = (int)arg;
	}
}
