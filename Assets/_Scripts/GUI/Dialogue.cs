using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class DialogueText {
	
	public string text;
	
}
public class Dialogue : MonoBehaviour {

	public DialogueText[] dialogue;
	public int threshold;

	public int dialogueIndex;

	public GameObject dialogueBox;

	public Transform canvas;
	public GameObject player;
	

	private GameObject box;

	public bool playerInArea = false;

	void Start(){
		player = GameObject.Find("Player");
		canvas = GameObject.Find("Canvas").transform;
	}
	
	void OnTriggerEnter2D(Collider2D other){
		playerInArea = true;
		if (other == player.GetComponent<Collider2D>()){
			playerInArea = true;
		}
	}



	void OnTriggerExit2D(Collider2D other){
		
		if (other == player.GetComponent<Collider2D>()){
			playerInArea = false;
			
		}
	}

	void Update () {
		if (Input.GetKeyDown("space")) {
			if (playerInArea){
				if (GameObject.Find("DialogueBox(Clone)") == null){
				print("dialogue started");
				StartDialogue();
				}
			}
            print("space key was pressed");
		}
        
	}

	void StartDialogue(){
		
		box = Instantiate(dialogueBox, new Vector3(400, 50, -2), Quaternion.identity, canvas) as GameObject;
		if (dialogueIndex  < threshold){
			GameObject.Find("DialogueText").GetComponent<Text>().text = dialogue[dialogueIndex].text;
		}else{
			int randomIndex = Random.Range(0, dialogue.Length - threshold) + threshold;
			print("randomIndex: " + randomIndex);
			GameObject.Find("DialogueText").GetComponent<Text>().text = dialogue[randomIndex].text;
		}
		dialogueIndex++;
		print("Dialogue created");
		

	}

	
}
