using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class tracks the game components needed to "score" how well the player is doing
/// </summary>
public class Scorekeeper : MonoBehaviour, IPlayerControllerListener, ISongUpdateListener, IAIListener {

	public float score  {
		get {return score;}
        set { scoreBar.ScaleScoreBar(value / maxScore);}
	}
	public float maxEmotionPoints = 2;
	public float maxRhythmPoints = 4;
	public float noEmotionMatchPunishment = 1;
	public float noRhythmMatchPunishment = 1;
	public float minNoOfRhythmStringMatches = 2;
	public float maxScore = 10;

	public ScoreBar scoreBar;

	private struct SongRecord {
		public int startBeat;
		public AudioLoop loop;
	}

	[HideInInspector]
	public bool playerIsLeading = false;

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
			GetScoreForBeatRange(beatInfo.currentBeat - scoreEveryNumBeats, beatInfo.currentBeat);
		}
	}

	private void GetScoreForBeatRange(int startBeat, int endBeat) {
		HashSet<string> playerEmotions = GetEmotionsForRecordsInRange(playerSongRecord, startBeat, endBeat);
		HashSet<string> npcEmotions = GetEmotionsForRecordsInRange(npcSongRecord, startBeat, endBeat);
		
		int noOfRhythmStringMatches = GetNoOfRhythmStringMatchesInRange(startBeat, endBeat);
		int noOfEmotionMatches = GetNoOfRhythmStringMatchesInRange(startBeat,endBeat);
		int noOfBeats = endBeat - startBeat;
		int noOfEmotions = playerEmotions.Count + npcEmotions.Count;

		score += noOfEmotionMatches * maxEmotionPoints/noOfEmotions;
		score += noOfRhythmStringMatches * maxRhythmPoints/noOfBeats;

		if(noOfRhythmStringMatches < minNoOfRhythmStringMatches){
			score -= noRhythmMatchPunishment;
		}
		if(noOfEmotionMatches <= 0){
			score -= noEmotionMatchPunishment;
		}


		// Do the scoring
		// var score = 0;
		// if(playerIsLeading)
		// 	score = ScoreForPlayerLeading();
		// else
		// 	score = ScoreForPlayerFollowing();


	}

	private int GetNoOfRhythmStringMatchesInRange(int startBeat, int endBeat){
		RhythmString playerRhythmString = GetRhythmStringForRecordsInRange(playerSongRecord, startBeat, endBeat);
		RhythmString npcRhythmString = GetRhythmStringForRecordsInRange(npcSongRecord, startBeat, endBeat);
		int noOfRhythmStringMatches = playerRhythmString.GetNumRhythmStringMatches(npcRhythmString);
		return noOfRhythmStringMatches;
	}

	private int GetNoOfEmotionMatchesInRange(int startBeat, int endBeat){
		HashSet<string> playerEmotions = GetEmotionsForRecordsInRange(playerSongRecord, startBeat, endBeat);
		HashSet<string> npcEmotions = GetEmotionsForRecordsInRange(npcSongRecord, startBeat, endBeat);
		int noOfEmotionMatches = NumMatchedEmotions(playerEmotions,npcEmotions);
		return noOfEmotionMatches;
	}

	private int ScoreForPlayerLeading() {
		// TODO: Implement a scoring algorithm
		return 0;
	}

	private int ScoreForPlayerFollowing() {
		// TODO: Implement a scoring algorithm
		return 0;
	}

	private int NumMatchedEmotions(HashSet<string> playerEmotions, HashSet<string> npcEmotions) {
		var count = 0;
		foreach(string emotion in npcEmotions) {
			if(playerEmotions.Contains(emotion))
				count++;
		}
		return count;
	}

	/* *** *** *** *** *** *** *** *** *** *** *** *** *** ***
						Utility Methods
	 *** *** *** *** *** *** *** *** *** *** *** *** *** *** */

	private RhythmString GetRhythmStringForRecordsInRange(List<SongRecord> songRecords, int startBeat, int endBeat) {
		var records = GetRecordsInBeatRange(songRecords, startBeat, endBeat);
		return GetRhythmStringForSongRecords(records, endBeat);
	}

	private RhythmString GetRhythmStringForSongRecords(List<SongRecord> songRecords, int endBeat) {
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

	private HashSet<string> GetEmotionsForRecordsInRange(List<SongRecord> records, int startBeat, int endBeat) {
		var recordsInRange = GetRecordsInBeatRange(records, startBeat, endBeat);
		return GetEmotionsForRecords(recordsInRange);
	}

	private HashSet<string> GetEmotionsForRecords(List<SongRecord> records) {
		var loops = new List<AudioLoop>();
		foreach(var record in records) {
			loops.Add(record.loop);
		}

		return AudioLoop.GetAllEmotionsInLoops(loops);
	}

	/// <summary>
	/// Returns all loops from the records that would be playing between the startBeat (inclusive) and the endBeat (exclusive)
	/// </summary>
	private List<SongRecord> GetRecordsInBeatRange(List<SongRecord> records, int startBeat, int endBeat) {
		var recordInRange = new List<SongRecord>();
		var previousRecord = new SongRecord();

		foreach(var recordEntry in records) {
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
	
}
