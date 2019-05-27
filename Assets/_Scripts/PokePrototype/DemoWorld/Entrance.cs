using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour {
	public string levelToLoad;
	public string exitPoint;

	private PlayerMovementController player;
	private DemoWorldLevelManager levelManager;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.name == "Player"){
			print("Player entrance col");
			player.entrancePoint = exitPoint;
			levelManager.LoadLevel(levelToLoad, 0.6f);
			//Application.LoadLevel(levelToLoad);
		}
	}

	void Start () {
		levelManager = GameObject.Find("LevelManager").GetComponentInParent<DemoWorldLevelManager>();
		player = FindObjectOfType<PlayerMovementController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
