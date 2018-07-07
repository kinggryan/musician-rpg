﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour {

	public NPCAnimationController[] npcs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DidStartSong() {
		foreach(var npc in npcs) {
			npc.StartPlaying();
		}
	}

	public void UpdateGroove(float groove) {
		foreach(var npc in npcs) {
			npc.UpdateGroove(groove);
		}
	}
}
