using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVolumeGateBars : MonoBehaviour {

	public UnityEngine.UI.Slider volumeSlider;
	public UnityEngine.UI.Slider gateSlider;
	public UnityEngine.UI.Image volumeSelectedIcon;
	public UnityEngine.UI.Image gateSelectedIcon;
	

	// Use this for initialization
	void Awake() {
		NotificationBoard.AddListener(PlayerMidiController.Notifications.changedSelectedDynamicControl, ChangeSelectedDynamicsControl);
		NotificationBoard.AddListener(PlayerMidiController.Notifications.changedVolume, DidUpdatePlayerVolume);
		NotificationBoard.AddListener(PlayerMidiController.Notifications.changedGate, DidUpdatePlayerGate);
	}
	
	void Start() {
		gateSelectedIcon.enabled = true;
		volumeSelectedIcon.enabled = false;
	}

	// Update is called once per frame
	void DidUpdatePlayerVolume(object playerController, object volume) {
		var volCast = (float)volume;
		volumeSlider.value = volCast;
	}

	void DidUpdatePlayerGate(object playerController, object gate) {
		var gateCast = (float)gate;
		gateSlider.value = gateCast;
	}

	void ChangeSelectedDynamicsControl(object playerController, object controlIndex) {
		// 0 volume
		// 1 gate
		var controlIndexCast = (int)controlIndex;
		if(controlIndexCast == 0) {
			gateSelectedIcon.enabled = false;
			volumeSelectedIcon.enabled = true;
		} else if(controlIndexCast == 1) {
			gateSelectedIcon.enabled = true;
			volumeSelectedIcon.enabled = false;
		}
	}
}
