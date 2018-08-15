using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopsRPGManagerDebugDisplay : MonoBehaviour {

	public UnityEngine.UI.Slider jammageBar;
	public UnityEngine.UI.Text playerStaminaText;
	public UnityEngine.UI.Text npcStaminaText;

	public GameObject npcMelody;
	public GameObject npcHarmony;
	public GameObject npcRhythm;

	public GameObject playerMelody;
	public GameObject playerHarmony;
	public GameObject playerRhythm;

	// Use this for initialization
	void DidChangePlayerRole(LoopsRPGManager.MusicalRole role) {
		playerMelody.SetActive(false);
		playerHarmony.SetActive(false);
		playerRhythm.SetActive(false);

		switch(role) {
			case LoopsRPGManager.MusicalRole.Harmony: playerHarmony.SetActive(true); break;
			case LoopsRPGManager.MusicalRole.Melody: playerMelody.SetActive(true); break;
			case LoopsRPGManager.MusicalRole.Rhythm: playerRhythm.SetActive(true); break;
		}
	}

	void DidChangeNPCRole(LoopsRPGManager.MusicalRole role) {
		npcMelody.SetActive(false);
		npcHarmony.SetActive(false);
		npcRhythm.SetActive(false);

		switch(role) {
			case LoopsRPGManager.MusicalRole.Harmony: npcHarmony.SetActive(true); break;
			case LoopsRPGManager.MusicalRole.Melody: npcMelody.SetActive(true); break;
			case LoopsRPGManager.MusicalRole.Rhythm: npcRhythm.SetActive(true); break;
		}
	}

	void DidChangeJammage(float jammagePercent) {
		jammageBar.value = jammagePercent;
	}

	void DidChangePlayerStamina(int playerStamina) {
		playerStaminaText.text = "" + playerStamina;
	}

	void DidChangeNPCStamina(int npcStamina) {
		npcStaminaText.text = "" + npcStamina;
	}

	public void UpdateUI(LoopsRPGManager.MusicalRole npcRole, LoopsRPGManager.MusicalRole playerRole, float jammagePercent, int playerStamina, int npcStamina) {
		DidChangePlayerRole(playerRole);
		DidChangeNPCRole(npcRole);
		DidChangeJammage(jammagePercent);
		DidChangePlayerStamina(playerStamina);
		DidChangeNPCStamina(npcStamina);
	}
}
