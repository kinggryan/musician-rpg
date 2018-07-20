using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour {

	public NPCAnimationController[] npcs;
	public SongStructureDisplay songStructureDisplay;

	public void DidStartSong() {
		foreach(var npc in npcs) {
			npc.StartPlaying();
		}

		if(songStructureDisplay)
			songStructureDisplay.StartSong();
	}

	public void UpdateGroove(float groove) {
		foreach(var npc in npcs) {
			npc.UpdateGroove(groove);
		}
	}

	public void SetBPM(float bpm) {
		foreach(var npc in npcs) {
			npc.SetBPM(bpm);
		}
	}
}
