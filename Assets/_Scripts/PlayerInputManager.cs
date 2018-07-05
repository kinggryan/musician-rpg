using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

	public SongPlayer songPlayer;

	private bool songPlaying = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("StartSong") && !songPlaying) {
			songPlaying = true;
			songPlayer.StartSong();
		}

		if(songPlaying) {
			var newPlayerLoopIndex = -1;
			if(Input.GetButtonDown("Loop1")) {
				newPlayerLoopIndex = 0;
			} else if (Input.GetButtonDown("Loop2")) {
				newPlayerLoopIndex = 1;
			} else if (Input.GetButtonDown("Loop3")) {
				newPlayerLoopIndex = 2;
			} else if (Input.GetButtonDown("Loop4")) {
				newPlayerLoopIndex = 3;
			}
			
			if(newPlayerLoopIndex >= 0) {
				songPlayer.ChangePlayerLoop(newPlayerLoopIndex);
			}
		}
	}
}
