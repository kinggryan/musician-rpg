﻿using System.Collections;
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
	private List<SongPhrase> songPhrases = new List<SongPhrase>();

	private AudioLoop currentNPCLoop;
	private AudioLoop currentPlayerLoop;
	private const int scoreEveryNumBeats = 4;
	private const int scoreEveryNumBeatsOffset = 1;

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

	void Start() {
		var songStructureManager = Object.FindObjectOfType<SongStructureManager>();
		var sections = songStructureManager.songSections;
		foreach(var section in sections) {
			foreach(var phrase in section.phrases) {
				songPhrases.Add(phrase);
			}
		}
	}

	// IAIListener
	public void DidChangeAILoop(AIMIDIController ai, AudioLoop loop) {
		currentNPCLoop = loop;
	}

	// IPlayerControllerListener
	public void DidChangeLoop(AudioLoop newLoop) {
		currentPlayerLoop = newLoop;
	}

	// ISongUpdateListener
	public void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {
		// add a new song record
		if(beatInfo.currentBeat > 0) {
			var playerSongRecordEntry = new SongRecord();
			playerSongRecordEntry.loop = currentPlayerLoop;
			playerSongRecordEntry.startBeat = beatInfo.currentBeat - 1;
			playerSongRecord.Add(playerSongRecordEntry);

			var npcSongRecordEntry = new SongRecord();
			npcSongRecordEntry.loop = currentNPCLoop;
			npcSongRecordEntry.startBeat = beatInfo.currentBeat - 1;
			npcSongRecord.Add(npcSongRecordEntry);
		}

		if(beatInfo.currentBeat > scoreEveryNumBeats && beatInfo.currentBeat % scoreEveryNumBeats == scoreEveryNumBeatsOffset) {
			Score();
		}
	}

	private void Score() {
		// Do the scoring
	}
}
