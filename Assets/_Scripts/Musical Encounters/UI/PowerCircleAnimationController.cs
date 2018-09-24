using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PowerCircleAnimationController : MonoBehaviour {

	Animator animator;
	bool isSongPlaying = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.speed = 0.00000001f;
	}
	
	// Update is called once per frame
	void Update () {
		// if(ani)
	}

	public void StartSong(float bpm) {
		isSongPlaying = true;
		SetBPM(bpm);
	}

	public void SetBPM(float bpm) {
		if(isSongPlaying)
			animator.speed = bpm / 120f;
	}
}
