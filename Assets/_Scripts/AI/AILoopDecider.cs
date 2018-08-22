using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class decides which loops to play for the AI
/// </summary>
public abstract class AILoopDecider {

	/// <summary>
	/// This struct is used to track the player's loops as they play them
	/// </summary>
	protected struct PlayerSongRecord {
		public int startBeat;
		public AudioLoop loop;
	}

	protected List<AudioLoop> knownLoops;
	protected List<PlayerSongRecord> playerSongRecord = new List<PlayerSongRecord>();
	protected SongPhrase currentSongPhrase;
	protected int currentBeatNumber;
	protected int numBeatsPerAILoop = 16;
	
	private AudioLoop currentPlayerLoop;

	public AILoopDecider(List<AudioLoop> loops) {
		this.knownLoops = loops;
	}

	public abstract AudioLoop ChooseLoopToPlay();

	public void DidStartPlayerLoop(AudioLoop playerLoop) {
		// This should be tracked in some way
		currentPlayerLoop = playerLoop;
	}

	public void DidStartNewSongPhrase(SongPhrase songPhrase) {
		// This should also be tracked
		currentSongPhrase = songPhrase;
	}

	public void DidStartNextBeat() {
		if(currentPlayerLoop != null && currentBeatNumber >= 0) {
			var recordEntry = new PlayerSongRecord();
			recordEntry.startBeat = currentBeatNumber;
			recordEntry.loop = currentPlayerLoop;
			playerSongRecord.Add(recordEntry);
		}
		currentBeatNumber++;
	}

	protected List<PlayerSongRecord> GetPlayerSongRecordForBeatRange(int startBeat, int endBeat) {
		var recordInRange = new List<PlayerSongRecord>();
		PlayerSongRecord previousRecord = new PlayerSongRecord();

		foreach(var recordEntry in playerSongRecord) {
			if(recordEntry.startBeat > startBeat && previousRecord.loop != null && previousRecord.startBeat < startBeat) {
				recordInRange.Add(previousRecord);
			}
			if(recordEntry.startBeat >= startBeat && recordEntry.startBeat < endBeat) {
				recordInRange.Add(recordEntry);
			} 
			previousRecord = recordEntry;
		}

		return recordInRange;
	}

	protected RhythmString GetRhythmStringForSongRecords(List<PlayerSongRecord> songRecords, int endBeat) {
		if(songRecords.Count == 0)
			return new RhythmString("");
		
		var rhythmString = new RhythmString("");
		for(var i = 1; i < songRecords.Count; i++) {
			var currentRecord = songRecords[i];
			var previousRecord = songRecords[i-1];
			rhythmString = rhythmString.AppendRhythmString(previousRecord.loop.rhythmString.GetRhythmStringForBeatRange(previousRecord.startBeat, currentRecord.startBeat));
		}

		var finalRecord = songRecords[songRecords.Count-1];
		rhythmString = rhythmString.AppendRhythmString(finalRecord.loop.rhythmString.GetRhythmStringForBeatRange(finalRecord.startBeat,endBeat));
		return rhythmString;
	}

	// to make a decision about what loops to play
	// you need to know
	// - What the current player pattern is (over the course of some number of bars)
	//	- so when the player starts a loop, we need to hear that
	// - What the current song phrase is (for emotion tags and rhythm string)

}
