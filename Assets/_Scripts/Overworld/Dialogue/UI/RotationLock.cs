using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
// This class keeps the rotation of the object it is attached to equal to the camera's orientation at all times
//</summary>
public class RotationLock : MonoBehaviour {

	private Quaternion lockedRotation;

	// Use this for initialization
	void Start () {
		lockedRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = lockedRotation;
	}
}
