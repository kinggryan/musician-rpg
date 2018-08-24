using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class tracks the game components needed to "score" how well the player is doing
/// </summary>
public class Scorekeeper : MonoBehaviour, IPlayerControllerListener, ISongUpdateListener, IAIListener {

	public float score  {
		get {
			return _score;
		}
        set { 
			_score = value;
			foreach(var listener in listeners) {
				listener.DidChangeScore(score);
			}
		}
	}
	public float maxEmotionPoints = 2;
	public float maxRhythmPoints = 4;
	public float noEmotionMatchPunishment = 1;
	public float noRhythmMatchPunishment = 1;
	public float minNoOfRhythmStringMatches = 2;
	public float maxScore = 10;
	public int scoreEveryNumBeats = 8;
	public int stalenessPunishment = 2;

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
	private const int scoreEveryNumBeatsOffset = 0;

	public float _score;

	private int lastBeat;
	private bool loopChangedSinceLastInterval;
	private int stalenessCounter;

	private List<IScorekeeperListener> listeners = new List<IScorekeeperListener>();

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
		score = maxScore/2;
		foreach(var listener in listeners) {
			listener.DidSetMaxScore(maxScore);
		}
	}

	public void AddListener(IScorekeeperListener listener) {
		listeners.Add(listener);
	}

	public void RemoveListener(IScorekeeperListener listener) {
		listeners.Remove(listener);
	}

	// IAIListener
	public void DidChangeAILoop(AIMIDIController ai, AudioLoop loop) {
		currentNPCLoop = loop;
		loopChangedSinceLastInterval = true;
	}

	// IPlayerControllerListener
	public void DidChangeLoop(AudioLoop newLoop, int index) {
		currentPlayerLoop = newLoop;
		loopChangedSinceLastInterval = true;
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

		if(beatInfo.currentBeat > scoreEveryNumBeats && beatInfo.currentBeat % scoreEveryNumBeats == scoreEveryNumBeatsOffset && beatInfo.currentBeat != lastBeat) {
			lastBeat = beatInfo.currentBeat;
			Debug.Log("currentBeat: " + beatInfo.currentBeat + "beatInfo.currentBeat % scoreEveryNumBeats " + beatInfo.currentBeat % scoreEveryNumBeats);
			GetScoreForBeatRange(beatInfo.currentBeat - scoreEveryNumBeats, beatInfo.currentBeat);
		}
	}

	public void DidStartSongWithBPM(float bpm) {}

	private void GetScoreForBeatRange(int startBeat, int endBeat) {
		
		HashSet<string> playerEmotions = GetEmotionsForRecordsInRange(playerSongRecord, startBeat, endBeat);
		HashSet<string> npcEmotions = GetEmotionsForRecordsInRange(npcSongRecord, startBeat, endBeat);
		
		int noOfRhythmStringMatches = GetNoOfRhythmStringMatchesInRange(startBeat, endBeat);
		int noOfEmotionMatches = GetNoOfEmotionMatchesInRange(startBeat, endBeat);
		int noOfBeats = endBeat - startBeat;
		int noOfEmotions = playerEmotions.Count + npcEmotions.Count;

		float adjustedEmotionScore = noOfEmotionMatches * maxEmotionPoints/noOfEmotions;float adjustedRhythmScore = (noOfRhythmStringMatches - minNoOfRhythmStringMatches) * maxRhythmPoints/noOfBeats;

		Debug.Log("Rhythm String Matches: " + noOfRhythmStringMatches + "\n Emotion matches: " + noOfEmotionMatches + "\n Emotion score: " + adjustedEmotionScore + "\n Rhythm Score: " + adjustedRhythmScore);

		score += adjustedEmotionScore;
		score += adjustedRhythmScore;

		if(noOfRhythmStringMatches < minNoOfRhythmStringMatches){
			score -= noRhythmMatchPunishment;
			Debug.Log("No Rhythm Match");
		}
		if(noOfEmotionMatches <= 0){
			score -= noEmotionMatchPunishment;
			Debug.Log("No Emotion Match");
		}
		if(!loopChangedSinceLastInterval){
			stalenessCounter++;
			score -= stalenessCounter * stalenessPunishment;
			Debug.Log("Stale Loop");
		}else{
			stalenessCounter = 0;
		}
		loopChangedSinceLastInterval = false;


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
		//Debug.Log("Player Rhythm String: " + playerRhythmString + "\nNPC Rhythm String: " + npcRhythmString + "\n" + noOfRhythmStringMatches + " matches");
		return noOfRhythmStringMatches;
	}

	private int GetNoOfEmotionMatchesInRange(int startBeat, int endBeat){
		HashSet<string> playerEmotions = GetEmotionsForRecordsInRange(playerSongRecord, startBeat, endBeat);
		HashSet<string> npcEmotions = GetEmotionsForRecordsInRange(npcSongRecord, startBeat, endBeat);
		int noOfEmotionMatches = NumMatchedEmotions(playerEmotions,npcEmotions);
		//Debug.Log("Player Emotions: " + playerEmotions + "\nNPC Emotions: " + npcEmotions + "\n" + noOfEmotionMatches + " matches");
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
		//Debug.Log("got Rhythm String from range " + startBeat + " - " + endBeat + ": " + GetRhythmStringForSongRecords(records, endBeat));
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
		//Debug.Log("got Rhythm String from record: " + rhythmString);
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
