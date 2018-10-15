using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovementController : MonoBehaviour {

	enum State {
		Walking,
		Idle
	}

	public delegate void MovementCompleteCallback();

	const float distanceToPlayerForMusicalEncounter = 10f;
	const float distanceToTargetThreshold = 0.1f;

	NavMeshAgent agent;
	[SerializeField]
	Transform player;
	
	MovementCompleteCallback movementCompleteCallback;
	Vector3 conversationalPosition;
	State state = State.Idle;

	// Use this for initialization
	void Awake () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.Walking) {
			var flattenedPosition = new Vector3(transform.position.x, 0, transform.position.z);
			var flattenedDestination = new Vector3(agent.destination.x, 0, agent.destination.z);
			if(Vector3.Distance(flattenedPosition, flattenedDestination) <= distanceToTargetThreshold) {
				state = State.Idle;
				if(movementCompleteCallback != null) 
					movementCompleteCallback();
				movementCompleteCallback = null;
				// TODO: Rotate to look at the player or whatever
			}
		}
	}

	public void MoveToMusicalEncounterPosition(MovementCompleteCallback callback) {
		// To move to a valid musical encounter position
		// Find a point on the navmesh which is closest to player.position + distanceToPlayerForMusicalEncounter*Vector3.right;
		// Move to that position
		// When movement is complete, callback
		NavMeshHit info;
		NavMesh.SamplePosition(player.transform.position + distanceToPlayerForMusicalEncounter*Vector3.right,out info, Mathf.Infinity, NavMesh.AllAreas);
		agent.SetDestination(info.position);
		movementCompleteCallback = callback;
		state = State.Walking;
		conversationalPosition = transform.position;
	}

	public void ReturnToConversationalPosition(MovementCompleteCallback callback) {
		// To move to a valid musical encounter position
		// Find a point on the navmesh which is closest to player.position + distanceToPlayerForMusicalEncounter*Vector3.right;
		// Move to that position
		// When movement is complete, callback
		NavMeshHit info;
		NavMesh.SamplePosition(conversationalPosition, out info, Mathf.Infinity, NavMesh.AllAreas);
		agent.SetDestination(info.position);
		movementCompleteCallback = callback;
		state = State.Walking;
	}
}
