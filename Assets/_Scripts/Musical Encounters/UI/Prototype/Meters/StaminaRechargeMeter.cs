using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRechargeMeter : MonoBehaviour {

	public UnityEngine.UI.Image[] unfilledSegments;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.updatedStaminaRechargeMeter, DidUpdateStaminaRechargeMeter);
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.updatedStaminaRechargeMeter, DidUpdateStaminaRechargeMeter);
	}
	
	// Update is called once per frame
	void DidUpdateStaminaRechargeMeter (object sender, object arg) {
		var meterAmount = (int)arg;
		FillUpToMeterAmount(meterAmount);
	}

	void FillUpToMeterAmount(int meterAmount) {
		for(var i = 0 ; i < unfilledSegments.Length ; i++) {
			if(i < meterAmount) {
				unfilledSegments[i].enabled = false;
			} else {
				unfilledSegments[i].enabled = true;
			}
		}
	}
}
