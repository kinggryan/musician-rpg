using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopButton : MonoBehaviour, IPlayerControllerListener {

	public Sprite deselectedImage;
	public Sprite selectedImage;
	public int playerLoopIndex;
	public UnityEngine.UI.Text jammageText;
	public UnityEngine.UI.Text staminaText;

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

		var rpgManager = Object.FindObjectOfType<RPGGameplayManger>();
		jammageText.text = "" + rpgManager.playerMoves[playerLoopIndex].jammageGain;
		if(staminaText != null)
			staminaText.text = "" + rpgManager.playerMoves[playerLoopIndex].staminaCost;
	}
}
