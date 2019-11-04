using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammageMeter : GenericMeter {

	int jammageThreshold = 100;
	Animator animator;

	// Use this for initialization
	override protected void Awake () {
		base.Awake();

		animator = GetComponent<Animator>();

		NotificationBoard.AddListener(RPGGameplayManger.Notifications.updatedJammage, DidUpdateJammage);
		// NotificationBoard.AddListener(RPGGameplayManger.Notifications.setJammageThreshold, DidUpdateJammageThreshold);
		// TODO: Set this value in a real way
		maxValue = 32; 
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.updatedJammage, DidUpdateJammage);
		// NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.setJammageThreshold, DidUpdateJammageThreshold);
	}

	void DidUpdateJammage(object sender, object arg) {
		var newJammage = (int)arg;
		var oldValue = value;
		value = Mathf.Min(newJammage,jammageThreshold);
		if(newJammage >= jammageThreshold && value > oldValue) {
			// Do some animation
			animator.SetTrigger("flash");
		}
	}

	void DidUpdateJammageThreshold(object sender, object arg) {
		jammageThreshold = (int)arg;
	}
}
