using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// This class handles the transition to and from musical encounters, without dealing with the actual gameplay of the encounter
// There should only be one in a scene
// </summary>
public class MusicalEncounterManager: MonoBehaviour {

	// This class should drive coordination between the various classes
	// Meaning that when the countoff is done, it should actually talk to THIS class, which tells the other classes to begin playing
	// 

	public enum SuccessLevel {
		TotalFailure,
		PartialFailure,
		Pass,
		Excel,
		Perfect,
		None
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
	public EncounterParticleController particleController;
	// TODO: In our current configuration, there should really only be one canvas
	public Canvas[] jamCanvas;

	private MIDISongPlayer songPlayer;
	private MusicalEncounterInfo currentEncounterInfo;
	
	// TODO: This should probably be read from the song file
	private const float songBPM = 240f;

	public void Awake() {
		songPlayer = Object.FindObjectOfType<MIDISongPlayer>();
		NotificationBoard.AddListener(AutomaticCountoffController.Notifications.countoffComplete, CountoffComplete);
	}

	public string GetCurrentMusicalEncounterSongFile() {
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
		songPlayer.Stop();
		playerMidiController.Stop();
		songStructureManager.StopSong();
		countoffController.enabled = false;
		playerMidiController.enabled = false;
		particleController.EndSong();

		foreach(var c in jamCanvas)
			c.enabled = false;
	}

	public void CountoffComplete(object sender, object obj) {
		var bpm = (float)obj;
		StartSongWithBPM(bpm);
	}

	void StartSongWithBPM(float bpm) {
		// animationManager.SetBPM(bpm);
		// animationManager.DidStartSong();
		// animationManager.UpdateGroove(1f);

		songPlayer.playbackRate = bpm / songBPM;
		songPlayer.Play();
		songStructureManager.bpm = bpm;
		songStructureManager.StartSong();

		playerMidiController.StartSongWithBPM(bpm);

		// TODO: Don't turn this on if it's just practice mode
		particleController.StartSong();
	}
}
