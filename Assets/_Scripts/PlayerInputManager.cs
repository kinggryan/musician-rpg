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
		if(songPlaying) {
			if(Input.GetButtonDown("Loop1")) {
				Debug.Log("button pressed");
				songPlayer.PlayClipNextBeat();
			}
		} else {
			if(Input.GetButtonDown("Loop1")) {	
				songPlaying = true;
				songPlayer.StartSong();
			}
		}
	}
}
