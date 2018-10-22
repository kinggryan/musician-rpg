using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloJamController : MonoBehaviour {

	public string musicalEncounterSongfileName;
	private MusicalEncounterManager musicalEncounterManager;
	[SerializeField]
	private SpeechBubbleCountoffDisplay countoffDisplay;
	public GameObject jamInterface;
	private bool isSoloJamming = false;

	private CameraController cameraController;

	// Use this for initialization
	void Start () {
		musicalEncounterManager = UnityEngine.Object.FindObjectOfType<MusicalEncounterManager>();
		cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
		countoffDisplay = GetComponent<SpeechBubbleCountoffDisplay>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E)){
			if(!isSoloJamming){
				StartMusicalEncounter();
			}else{
				EndMusicalEncounter();
			}
		}		
	}

	void StartMusicalEncounter() {
		// Load the game scene
		// var player = UnityEngine.Object.FindObjectOfType<PlayerController>();

		cameraController.TransitionToMusicalEncounterCam();

		// TODO: The dialoguemanager should talk to an NPC controller, rather than smaller components of that controller
		// e.g. dialogue/movement shouldn't be interacted with individually but by a class that owns them
		// Don't display the UI elements until the npc has 
		musicalEncounterManager.StartedMusicalEncounter(musicalEncounterSongfileName,countoffDisplay);
		isSoloJamming = true;
	}

	void EndMusicalEncounter(){
		cameraController.TransitionToNormalCam();
		musicalEncounterManager.CompletedMusicalEncounter(MusicalEncounterManager.SuccessLevel.None);
		isSoloJamming = false;
	}
}
