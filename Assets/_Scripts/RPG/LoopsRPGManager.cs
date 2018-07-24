using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopsRPGManager : MonoBehaviour {

	public enum MusicalRole {
		Melody,
		Harmony,
		Rhythm
	};

	public MusicalRole playerRole { get; private set; }
	MusicalRole npcRole;
	
	int playerStamina;
	int npcStamina;

	int currentJammage;
	int maxJammage = 20;
	int startJammage = 10;

	LoopsRPGManagerDebugDisplay display;

	void Awake() {
		display = GetComponent<LoopsRPGManagerDebugDisplay>();
		playerRole = MusicalRole.Harmony;
		npcRole = MusicalRole.Melody;
		
	}

	void Start() {
		currentJammage = startJammage;
		UpdateUI();
	}

	// Use this for initialization
	public void MakeNPCMove() {
		var maxRange = npcStamina > 0 ? 3 : 1;
		var randomValue = Random.Range(0, maxRange);
		switch(randomValue) {
			case 0: NPCChill(); break;
			case 1: NPCVirtuoso(); break;
			case 2: NPCChangeRoles(); break;
		}
		MoveCompleted();
	}

	void NPCChill() {
		npcStamina++;
		currentJammage--;
	}

	void NPCVirtuoso() {
		npcStamina--;
		currentJammage -= 4;
	}

	void NPCChangeRoles() {
		npcStamina--;
		if(npcRole != playerRole) {
			npcRole = playerRole;
		} else {
			if(npcRole == MusicalRole.Harmony) {
				npcRole = MusicalRole.Rhythm;
			} else if(npcRole == MusicalRole.Rhythm) {
				npcRole = MusicalRole.Melody;
			} else {
				npcRole = MusicalRole.Harmony;
			}
		}
	}

	void MoveCompleted() {
		if(currentJammage <= 0 || currentJammage > maxJammage) {
			LoseJam();
		}
		UpdateUI();
	}

	void UpdateUI() {
		display.UpdateUI(npcRole,playerRole, (float)currentJammage / maxJammage, playerStamina, npcStamina);
		// Debug.Log("Jammage " + currentJammage + "| npc role " + npcRole + "| player Role " + playerRole + "| npc stamina " + npcStamina + "| player stamina " + playerStamina);
	}

	void LoseJam() {
		Debug.Log("You lost...");
	}

	/// Public Functions ///
	public void PlayerChill() {
		playerStamina++;
		currentJammage++;
		MoveCompleted();
	}

	public void PlayerVirtuoso() {
		if(playerStamina > 0) {
			playerStamina--;
			currentJammage += 4;
		}
		else {
			currentJammage -= 2;
		}
		MoveCompleted();
	}

	public void PlayerChangeRoles(MusicalRole role) {
		playerRole = role;
		if(playerStamina > 0) 
			playerStamina--;
		else
			currentJammage -= 2;

		MoveCompleted();
	}
}
