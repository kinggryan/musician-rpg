using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubble : OverworldDialogueDisplay {

	enum State {
		Hidden, Speaking
	}

	public delegate void AnimationCompleteCallback();

	private Vector3 fullScale;
	private State state = State.Hidden;
	private bool animating = false;
	private AnimationCompleteCallback animationCompleteCallback;

	private const float animationLerpSpeed = 10;
	private const float animationCompleteThreshold = 0.01f;

	[SerializeField]
	private UnityEngine.UI.Image image;

	void Awake() {
		fullScale = transform.localScale;
		// image = GetComponentInChildren<UnityEngine.UI.Image>();
		// text = GetComponentInChildren<UnityEngine.UI.Text>();
	}

	// Use this for initialization
	void Start () {
		image.enabled = false;
		text.enabled = false;
		text.text = "";
		transform.localScale = animationCompleteThreshold * fullScale.normalized;
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
		if(animating) {
			if(state == State.Hidden) {
				transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, animationLerpSpeed * Time.deltaTime);
				if(transform.localScale.magnitude <= animationCompleteThreshold) {
					animating = false;
					image.enabled = false;
					text.enabled = false;
					if(animationCompleteCallback != null)
						animationCompleteCallback();
					animationCompleteCallback = null;
				}
			} else if (state == State.Speaking) {
				transform.localScale = Vector3.Lerp(transform.localScale, fullScale, animationLerpSpeed * Time.deltaTime);
				if(Vector3.Distance(transform.localScale, fullScale) <= animationCompleteThreshold) {
					animating = false;
					if(animationCompleteCallback != null)
						animationCompleteCallback();
					animationCompleteCallback = null;
				}
			}
		}
	}

	public void SetVisible(bool visible, AnimationCompleteCallback callback) {
		animating = true;
		this.state = visible ? State.Speaking : State.Hidden;
		if(visible) {
			image.enabled = true;
			text.enabled = true;
			text.text = "";
		}
		this.animationCompleteCallback = callback;
	}

	public bool GetVisible() {
		return state == State.Speaking;
	}
}
