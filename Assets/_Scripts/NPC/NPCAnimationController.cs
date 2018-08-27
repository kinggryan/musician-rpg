using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour, IScorekeeperListener, IPlayerControllerListener {

	Animator animator;

	private float score = 1f;
	private float maxScore = 1f;

	// Use this for initialization
	void Awake() {
		var scorekeeper = Object.FindObjectOfType<Scorekeeper>();
		scorekeeper.AddListener(this);
		var player = Object.FindObjectOfType<PlayerMidiController>();
		player.AddListener(this);
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void DidChangeScore(float score) {
		this.score = score;
		UpdateGroove(score/maxScore);
	}

	public void DidSetMaxScore(float maxScore) {
		this.maxScore = maxScore;
		UpdateGroove(score/maxScore);
	}

	public void StartPlaying() {
		animator.SetBool("playing", true);
	}

	public void UpdateGroove(float groove) {
		// TODO: Fix this
		animator.SetFloat("groove", groove);
	}

	public void SetBPM(float bpm) {
		animator.speed = bpm / 120f;
	}

	public void DidStartSongWithBPM(float bpm) {
		SetBPM(bpm);
		StartPlaying();
	}

	public void DidChangeLoop(AudioLoop newLoop, int index) { }

	public void DidWin() {
		animator.SetBool("playing", false);
	}

	public void DidLose() {
		animator.SetBool("playing", false);
	}
}
