using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerArrow : MonoBehaviour {

	public PlayerMouseSpringInput mouseInput;
	public bool isPowerActive;

	float maxScale = 2f;
	float minScale = 0f;
	float scaleMultipler;

	// Use this for initialization
	void Start () {
		scaleMultipler = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
		// Every step, if active, scale based on the mouse value
		if(isPowerActive) {
			var scaleValue = minScale + (maxScale - minScale)*(mouseInput.GetMouseValue()+1f)/2f;
			transform.localScale = new Vector3(transform.localScale.x, scaleValue*scaleMultipler, 1f);
		}
	}
}
