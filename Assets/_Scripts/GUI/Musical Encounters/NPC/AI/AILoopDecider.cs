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
	protected struct SongRecord {
		public int startBeat;
		public AudioLoop loop;
	}

	protected List<AudioLoop> knownLoops;
	protected List<AudioLoop> songSpecificLoops;
	protected List<SongRecord> playerSongRecord = new List<SongRecord>();
	protected List<SongPhrase> songPhrases = new List<SongPhrase>();
	protected SongPhrase currentSongPhrase;
	protected int currentBeatNumber;
	const int numBeatsPerAILoop = 16;
	const int numBeatsPerLeadSwitch = 64;
	
	private AudioLoop currentPlayerLoop;

	public AILoopDecider(List<AudioLoop> loops, List<AudioLoop> songSpecificLoops, SongSection[] songStructure) {
		this.knownLoops = loops;
		this.songSpecificLoops = songSpecificLoops;

		// Set all of the phrases using the song structure
		foreach(var section in songStructure) {
			foreach(var phrase in section.phrases) {
				songPhrases.Add(phrase);
			}
		}
	}

	public AILoopDecider(AILoopDecider parentDecider) {
		knownLoops = parentDecider.knownLoops;
		playerSongRecord = parentDecider.playerSongRecord;
		songPhrases = parentDecider.songPhrases;
		currentSongPhrase = parentDecider.currentSongPhrase;
		currentBeatNumber = parentDecider.currentBeatNumber;
		currentPlayerLoop = parentDecider.currentPlayerLoop;
	}

	public abstract AudioLoop ChooseLoopToPlay();
	/// <summary>
	/// This function should be called whenever we want to check to see if the state should change.
	/// Returns null if no change is needed. Otherwise, returns the initialized new state to transition to.
	/// </summary>
	public abstract AILoopDecider UpdateState();

	public void DidStartPlayerLoop(AudioLoop playerLoop) {
		// This should be tracked in some way
		currentPlayerLoop = playerLoop;
	}

	public void DidStartNewSongPhrase(SongPhrase songPhrase) {
		// This should also be tracked
		currentSongPhrase = songPhrase;
	}

	// TODO: Repair this
	public void DidStartNextBeat() {
		if(currentPlayerLoop != null && currentBeatNumber >= 0) {
			var recordEntry = new SongRecord();
			recordEntry.startBeat = currentBeatNumber;
			recordEntry.loop = currentPlayerLoop;
			playerSongRecord.Add(recordEntry);
		}
		currentBeatNumber++;
	}

	protected void Awake() {
		// NotificationBoard.AddListener(SongStructureManager.Notifications.)
	}

	protected List<SongRecord> GetPlayerSongRecordForBeatRange(int startBeat, int endBeat) {
		var recordInRange = new List<SongRecord>();
		var previousRecord = new SongRecord();

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

	protected List<SongPhrase> GetSongPhrasesForBeatRange(int startBeat, int endBeat) {
		var phrasesInRange = new List<SongPhrase>();
		var currentBeat = 0;

		foreach(var phrase in songPhrases) {
			var phraseEndBeat = currentBeat + phrase.TotalBeatLength();
			if((currentBeat >= startBeat && currentBeat < endBeat) || (currentBeat < startBeat && phraseEndBeat >= endBeat)) {
				phrasesInRange.Add(phrase);
			}

			currentBeat += phrase.TotalBeatLength();
		}

		return phrasesInRange;
	}

	protected SongPhrase GetSongPhraseForBeat(int startBeat) {
		var currentBeat = 0;

		foreach(var phrase in songPhrases) {
			var songPhraseStart = currentBeat;
			var songPhraseEnd = currentBeat + phrase.TotalBeatLength();
			if(startBeat >= songPhraseStart && startBeat < songPhraseEnd) {
				return phrase;
			}
			currentBeat += phrase.TotalBeatLength();
		}

		return null;
	}

	protected RhythmString GetRhythmStringForSongRecords(List<SongRecord> songRecords, int endBeat) {
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

	/// <summary>
	/// If the song structure states that there should be an npc loop on the next beat, this function returns that loop.
	/// Otherwise it returns null.
	/// </summary>
	protected AudioLoop GetSongSpecifiedLoopForNextBeat() {
		var phrase = GetSongPhraseForBeat(currentBeatNumber);
		if(phrase != null) {
			return phrase.npcLoop;
		}
		return null;
	}

	protected bool ShouldSwapLead() {
		return currentBeatNumber % numBeatsPerLeadSwitch == numBeatsPerLeadSwitch - 1;
	}

	// to make a decision about what loops to play
	// you need to know
	// - What the current player pattern is (over the course of some number of bars)
	//	- so when the player starts a loop, we need to hear that
	// - What the current song phrase is (for emotion tags and rhythm string)

}
