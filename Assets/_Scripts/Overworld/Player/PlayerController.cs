using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public float maxMovementSpeed = 3f;
	public float movementAcceleration = 10f;
	private float conversationRange = 3f;
	CharacterController controller;

	void Awake() {
		controller = GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Move
		var desiredVelocity = maxMovementSpeed*GetMovementInputVector();
		var currentVelocity = controller.velocity;
		currentVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, movementAcceleration*Time.deltaTime);
		controller.SimpleMove(currentVelocity);

		HandleActions();
	}

	void HandleActions() {
		var npcInRange = GetClosestNPCInConversationRange();
		if(npcInRange != null) {
			// Show a little highlight bubble
			if(Input.GetButtonDown("Select")) {
				npcInRange.StartConversation();
			}
		}
		// Highlight NPCs in range
	}

	Vector3 GetMovementInputVector() {
		return new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
	}

	NPCDialogueManager GetClosestNPCInConversationRange() {
		NPCDialogueManager closestNPC = null;
		float closestNPCRange = Mathf.Infinity;
		foreach(var npc in Object.FindObjectsOfType<NPCDialogueManager>()) {
			var distanceToNPC = Vector3.Distance(npc.transform.position, transform.position);
			if(distanceToNPC <= conversationRange && distanceToNPC < closestNPCRange && npc.CanStartConversation()) {
				closestNPC = npc;
				closestNPCRange = distanceToNPC;
			}
		}

		return closestNPC;
	}
}
