using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

	public SongPlayer songPlayer;
	public PlayerLoopManager loopManager;

	private bool songPlaying = false;
	
	// Update is called once per frame
	void Update () {
		// if(Input.GetButtonDown("StartSong") && !songPlaying) {
		// 	songPlaying = true;
		// 	songPlayer.StartSong();
		// }

		if(songPlaying) {
			if(Input.GetButtonDown("Loop")) {
				loopManager.StartRecording();
			} else if(Input.GetButtonUp("Loop")) {
				loopManager.FinishRecording();
			}

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
				ChangePlayerLoop(newPlayerLoopIndex);
			}
		}
	}

	void ChangePlayerLoop(int loopIndex) {
		songPlayer.ChangePlayerLoop(loopIndex);
	}

	void DidStartSong() {
		songPlaying = true;
	}
}
