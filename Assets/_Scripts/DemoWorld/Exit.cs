using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
	public string pointName;

	private PlayerMovementController player;

	void Start () {
		player = FindObjectOfType<PlayerMovementController>();
		if(player.entrancePoint == pointName){
			player.gameObject.transform.position = new Vector3(transform.position.x,transform.position.y,player.gameObject.transform.position.z);
		}
	}
	
	
}
