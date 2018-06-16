using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour {

	private double songStartDSPTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartSong() {
		songStartDSPTime = AudioSettings.dspTime;
	}

	void PlayLoopNextBeat() {
		
	}
}
