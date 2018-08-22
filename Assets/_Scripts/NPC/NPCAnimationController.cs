using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartPlaying() {
		animator.SetBool("playing", true);
	}

	public void UpdateGroove(float groove) {
		animator.SetFloat("groove", groove);
	}

	public void SetBPM(float bpm) {
		animator.speed = bpm / 120f;
	}
}
