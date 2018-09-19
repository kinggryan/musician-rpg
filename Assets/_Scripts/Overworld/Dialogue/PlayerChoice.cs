using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoice : MonoBehaviour {

	UnityEngine.UI.Image image;
	public Sprite defaultSprite;
	public Sprite highlightedSprite;

	// Use this for initialization
	void Awake () {
		image = GetComponent<UnityEngine.UI.Image>();
	}
	
	public void Highlight() {
		image.sprite = highlightedSprite;
	}

	public void Unhighlight() {
		image.sprite = defaultSprite;
	}
}
