using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerTracker : MonoBehaviour {

	public GameObject objectToTrack;
	public float lerpAmount;

	private Vector3 relativePosition;

	// Use this for initialization
	void Start () {
		relativePosition = transform.position - objectToTrack.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var targetPosition = objectToTrack.transform.position + relativePosition;
		// transform.position = Vector3.MoveTowards()
	}
}
