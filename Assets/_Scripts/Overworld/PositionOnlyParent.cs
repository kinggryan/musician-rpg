using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOnlyParent : MonoBehaviour {

	[SerializeField]
	private Transform positionParent;
	private Vector3 relativePosition;

	// Use this for initialization
	void Start () {
		relativePosition = transform.position - positionParent.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = positionParent.position + relativePosition;
	}
}
