using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager: MonoBehaviour {

	public SongPlayer songPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Loop1")) {
			songPlayer.PlayClipNextBeat();
		}
	}
}
