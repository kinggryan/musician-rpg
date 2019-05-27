using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour {

	private float creationBuffer = 0;

	void Start(){
		GameObject[] boxes = GameObject.FindGameObjectsWithTag("Dialogue");
		if (boxes.Length >= 2){
				print("dialogue destroyed");
				Destroy(gameObject);
			}
	}
	
	void Update() {

		if (creationBuffer < 0.1f){
		creationBuffer += Time.deltaTime;
		} else if (Input.GetKeyDown("space") || Input.GetButtonDown("BButton")){
				Destroy(gameObject);
				print("Dialogue destroyed");
			
		}
	}

}
