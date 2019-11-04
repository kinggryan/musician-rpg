using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerJammageTargetMeterBar : MeterBar {

	protected override void AddSelfAsListener() {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.setJammageTargetRange, NotificationCallback);
	}

	protected override void RemoveSelfAsListener() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.setJammageTargetRange, NotificationCallback);
	}

	void NotificationCallback(object sender, object arg) {
		var setJammageTargetArgs = (RPGGameplayManger.Notifications.SetJammageTargetRangeArgs)arg;
		UpdateIndicator(setJammageTargetArgs.lowerTarget);
	}
}
