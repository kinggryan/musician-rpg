using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// This class handles the transition to and from musical encounters, without dealing with the actual gameplay of the encounter
// There should only be one in a scene
// </summary>
public class MusicalEncounterManager: MonoBehaviour {

	public enum SuccessLevel {
		TotalFailure,
		PartialFailure,
		Pass,
		Excel,
		Perfect
	}

	struct MusicalEncounterInfo {
		public string songFileName;
		// Other metadata about the song -  npc, environment, etc
		public SuccessLevel successLevel;
	}

	// Oddly, the countoff controller is what's needed to really start the song. 
	public AutomaticCountoffController countoffController;
	public PlayerMidiController playerMidiController;
	public SongStructureManager songStructureManager;
	public AIMIDIController aiMIDIController;
	// TODO: In our current configuration, there should really only be one canvas
	public Canvas[] jamCanvas;

	private MusicalEncounterInfo currentEncounterInfo;

	public  string GetCurrentMusicalEncounterSongFile() {
		return currentEncounterInfo.songFileName;
	}

	// TODO: This should be modified once we have the unified NPC manager to take an NPC manager rather than the component pieces
	public void StartedMusicalEncounter(string songFileName, PlayerCountoffDisplay countoffDisplay) {
		currentEncounterInfo.songFileName = songFileName;
		songStructureManager.LoadSong(songFileName);
		aiMIDIController.LoadSong();
		countoffController.enabled = true;
		countoffController.countoffDisplay = countoffDisplay;
		playerMidiController.enabled = true;
		foreach(var c in jamCanvas)
			c.enabled = true;
	}

	public void CompletedMusicalEncounter(SuccessLevel successLevel) {
		currentEncounterInfo.successLevel = successLevel;
		countoffController.enabled = false;
		playerMidiController.enabled = false;
		foreach(var c in jamCanvas)
			c.enabled = false;
	}
}
