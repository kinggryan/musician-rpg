using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guru : MonoBehaviour {
	public Dialogue dialogue;
	public GameObject player;
	public int NoOfShrooms;
//	public Spells spells;
//	public SpellUI spellUI;
	public int giveShroomsAtIndex;
	private bool shroomsGiven = false;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		dialogue = GetComponent<Dialogue>();
		// spells = GameObject.Find("Spells").GetComponentInParent<Spells>();
		// spellUI = GameObject.Find("SpellUI").GetComponentInParent<SpellUI>();
	}

	// void GiveShrooms(){
	// 	shroomsGiven = true;
	// 	spells.power += NoOfShrooms;
	// 	spells.SetUIText();
	// 	spellUI.SetUIText();

	// }
	
	void Update () {
		if (Input.GetKeyDown("space") || Input.GetButtonDown("BButton")) {
			if (dialogue.playerInArea){
				if (dialogue.dialogueIndex == giveShroomsAtIndex + 1 && !shroomsGiven){
				print("Shrooms given");
				//GiveShrooms();
				}
			}
            print("space key was pressed");
		}
        
	}
}
