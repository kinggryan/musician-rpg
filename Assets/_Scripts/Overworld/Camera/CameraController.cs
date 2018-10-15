using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	public struct CameraState {
		public Vector3 localPosition;
		public float fov;
		public Quaternion localRotation;

		public CameraState(Vector3 localPosition, float fov, Quaternion localRotation) {
			this.localPosition = localPosition;
			this.fov = fov;
			this.localRotation = localRotation;
		}
	}

	private CameraState musicalEncounterState = new CameraState(new Vector3(4.45f,20.78f,-9.08f), 24, Quaternion.AngleAxis(68.56001f,Vector3.right));
	
	private CameraState normalState;
	private float lerpRate = 5;

	private CameraState currentState;

	[SerializeField]
	private Camera[] cameras;

	// Use this for initialization
	void Start () {
		normalState = new CameraState();
		normalState.localPosition = transform.localPosition;
		normalState.fov = cameras[0].fieldOfView;
		normalState.localRotation = transform.localRotation;

		currentState = normalState;
	}
	
	// Update is called once per frame
	void Update () {
		// if(Input.GetKeyDown("t")) {
		// 	TransitionToMusicalEncounterCam();
		// }

		// Lerp
		foreach(var cam in cameras) {
			cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, currentState.localPosition, lerpRate*Time.deltaTime);
			cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, currentState.localRotation, lerpRate*Time.deltaTime);
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentState.fov, lerpRate*Time.deltaTime);
		}
		
	}

	public void TransitionToMusicalEncounterCam() {
		currentState = musicalEncounterState;
	}

	public void TransitionToNormalCam() {
		currentState = normalState;
	}
}
