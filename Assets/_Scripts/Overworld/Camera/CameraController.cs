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

	private Camera camera;

	void Awake() {
		camera = GetComponent<Camera>();
	}

	// Use this for initialization
	void Start () {
		normalState = new CameraState();
		normalState.localPosition = transform.localPosition;
		normalState.fov = camera.fieldOfView;
		normalState.localRotation = transform.localRotation;

		currentState = normalState;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("t")) {
			TransitionToMusicalEncounterCam();
		}

		// Lerp
		transform.localPosition = Vector3.Lerp(transform.localPosition, currentState.localPosition, lerpRate*Time.deltaTime);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, currentState.localRotation, lerpRate*Time.deltaTime);
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, currentState.fov, lerpRate*Time.deltaTime);
	}

	public void TransitionToMusicalEncounterCam() {
		currentState = musicalEncounterState;
	}

	public void TransitionToNormalCam() {
		currentState = normalState;
	}
}
