using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongStructureDisplay : MonoBehaviour {

	private float rotationRate;
	private bool songPlaying = false;

	void Start() {
		// HACK: This should be dynamic
		SetBPM(120);
	}

	// Update is called once per frame
	void Update () {
		if(songPlaying) {
			transform.Rotate(0,0,Time.deltaTime * rotationRate);
		}
	}

	public void SetBPM(float bpm) {
		// Rotate 1 full rotation per 4 beats
		// TODO: Don't hardcode 4/4
		rotationRate = -360 / 8 * bpm / 60;
	}

	public void StartSong() {
		songPlaying = true;
	}
}
