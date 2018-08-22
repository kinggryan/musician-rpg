using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class tracks the game components needed to "score" how well the player is doing
/// </summary>
public class Scorekeeper : MonoBehaviour, IPlayerControllerListener, ISongUpdateListener, IAIListener {

	private struct SongRecord {
		public int startBeat;
		public AudioLoop loop;
	}

	private List<SongRecord> playerSongRecord = new List<SongRecord>();
	private List<SongRecord> npcSongRecord = new List<SongRecord>();

	// To make this work in the most dynamic way possible
	// it should have a history of what the NPC and the player have played
	// also the song structure
	// This way it can compare all of these elements together at any point in time, making it more flexible to scoring adjustments

	void Awake() {
		var playerController = Object.FindObjectOfType<PlayerMidiController>();
		playerController.AddListener(this);
		foreach(var ai in Object.FindObjectsOfType<AIMIDIController>()) {
			ai.AddListener(this);
		}
		var songStructureManager = Object.FindObjectOfType<SongStructureManager>();
		songStructureManager.RegisterSongUpdateListener(this);
	}

	// IAIListener
	public void DidChangeAILoop(AIMIDIController ai, AudioLoop loop) {

	}

	// IPlayerControllerListener
	public void DidChangeLoop(AudioLoop newLoop) {

	}

	// ISongUpdateListener
	public void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {

	}
}
