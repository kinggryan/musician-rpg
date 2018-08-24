using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopButton : MonoBehaviour, IPlayerControllerListener {

	public Sprite deselectedImage;
	public Sprite selectedImage;
	public int playerLoopIndex;

	private UnityEngine.UI.Image button;

	// Use this for initialization
	public void DidChangeLoop(AudioLoop newLoop, int index) {
		if(index == playerLoopIndex) {
			button.sprite = selectedImage;
		} else {
			button.sprite = deselectedImage;
		}
	}

	public void DidStartSongWithBPM(float bpm) {}

	void Start() {
		button = GetComponent<UnityEngine.UI.Image>();
		var playerController = Object.FindObjectOfType<PlayerMidiController>();
		playerController.AddListener(this);
	}
}
