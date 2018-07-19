using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseSpringInput {

	Vector2 anchorPos;
	public float maxDistance;
	public float tension;

	public void SetAnchor() {
		anchorPos = Input.mousePosition;
	}

	// Returns a value between -1 and 1
	public float GetMouseValue() {
		var vertDistance = Input.mousePosition.y - anchorPos.y;
		var adjustedValue = Mathf.Sign(vertDistance)*Mathf.Abs(Mathf.Pow(vertDistance / maxDistance, tension));
		return Mathf.Clamp(adjustedValue, -1, 1);
	}
}
