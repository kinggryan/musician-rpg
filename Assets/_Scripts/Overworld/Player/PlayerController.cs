using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public float maxMovementSpeed = 3f;
	public float movementAcceleration = 10f;
	CharacterController controller;

	void Awake() {
		controller = GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var desiredVelocity = maxMovementSpeed*GetMovementInputVector();
		var currentVelocity = controller.velocity;
		currentVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, movementAcceleration*Time.deltaTime);
		controller.SimpleMove(currentVelocity);
	}

	Vector3 GetMovementInputVector() {
		return new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
	}
}
