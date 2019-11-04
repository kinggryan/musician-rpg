using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseSpringInput:MonoBehaviour {

	public float tension;

	private float maxDistance = 100f;
	private Vector2 anchorPos;
	private Vector2 currentPos;

	public void SetAnchorWithValue(float value) {
		anchorPos = new Vector2(0, GetDistanceFromValue(value));
		currentPos = anchorPos;
	}

	public void Update() {
		currentPos += new Vector2(0, Input.GetAxis("Mouse Y"));
		if(Vector2.Distance(currentPos,anchorPos) > maxDistance) {
			var dir = currentPos - anchorPos;
			currentPos = maxDistance*dir.normalized + anchorPos;
		}
	}

	// Returns a value between -1 and 1
	public float GetMouseValue() {
		var vertDistance = Mathf.Abs(currentPos.y - anchorPos.y);
		var adjustedValue = Mathf.Sign(currentPos.y - anchorPos.y)*Mathf.Abs(Mathf.Pow(vertDistance / maxDistance, 1/tension));
		return Mathf.Clamp(adjustedValue, -1, 1);
	}

	private float GetDistanceFromValue(float value) {
		// value = (distance / maxDistance)^tension
		// distance/maxDistance = root(value, tension)
		// distance = root(value, tension)*maxDistance
		var rootVal = Mathf.Pow(value, tension);
		return Mathf.Sign(value)*Mathf.Abs(rootVal)*maxDistance;
	}
}
