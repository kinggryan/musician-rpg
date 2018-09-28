using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RPGGameplayManger : MonoBehaviour, ISongUpdateListener, IAIListener {

	public static class Notifications {
		public struct SetJammageTargetRangeArgs {
			public int lowerTarget;
			public int upperTarget;
		}
		/// <summary>
		/// The argument for this notification is a struct
		/// </summary>
		public static string setJammageTargetRange = "setJammageTargetRange";
		/// <summary>
		/// The argument for this notification is an integer - the new jammage 
		/// </summary>
		public static string updatedJammage = "updatedJammage";
		/// <summary>
		/// This notification is called when the player's loop is set for the next beat of the song.
		/// The argument is of type SetPlayerLoopForBeatArgs
		/// </summary>
		public static string setPlayerLoopForBeat = "setPlayerLoopForBeat";
		public struct SetPlayerLoopForBeatArgs {
			public PlayerMove playerMove;
			public int beatNumber;
			public int playerMoveIndex;
		}
		/// <summary>
		/// This notification is called when a song phrase is complete and the player's previous phrase loops are locked in
		/// The argument is of type SetPreviousPhrasePlayerLoopsArgs
		/// </summary>
		public static string setPreviousPhrasePlayerLoops = "setPreviousPhrasePlayerLoops";
		public struct SetPreviousPhrasePlayerLoopsArgs {
			public List<PlayerMove> playerMoves;
			public List<int> playerMoveIndices;
		}
		/// <summary>
		/// The argument for this notification is an integer - the new number of victory points
		/// </summary>
		public static string updatedVictoryPoints = "updatedVictoryPoints";
		/// <summary>
		/// There is no argument for this notification. It is called when the player receives the rhythm match bonus.
		/// </summary>
		public static string gotRhythmMatchBonus = "gotRhythmMatchBonus";
		/// <summary>
		/// Posted when the player and npc match on a quarter or eighth note
		/// </summary>
		public static string playerAndNPCRhythmMatchedOnPulse = "playerAndNPCRhythmMatchedOnPulse";
		/// <summary>
		/// Posted when the player passes a turn perfectly
		/// </summary>
		public static string playerGotPerfectTurn = "playerGotPerfectTurn";
		/// <summary>
		/// Posted when the player wins
		/// </summary> 
		public static string playerWon = "playerWon";
		/// <summary>
		/// Posted when the player loses
		/// </summary>
		public static string playerLost = "playerLost";
	}

	public class EncounterEffect {
		/// <summary>
		/// This function is called after the standard updates for the move take place but BEFORE the turn completion methods are called
		/// </summary>
		public virtual void DoNextMove(RPGGameplayManger gameplayManger) {}
		public virtual void FinishedTurn(RPGGameplayManger gameplayManger) {}
		public virtual bool ShouldBeRemoved(RPGGameplayManger gameplayManger) { return false; }
	}

	private struct SongRecord {
		public int startBeat;
		public AudioLoop loop;
	}

	private struct RhythmStringBonusBracket {
		public float maxMatchRatio;
		public float bonusMultiplier;
		public RhythmStringBonusBracket(float max, float bonus) {
			maxMatchRatio = max;
			bonusMultiplier = bonus;
		}
	}

	private SongStructureManager songStructureManager;

	// The list of the player's moves
	public List<PlayerMove> playerMoves = new List<PlayerMove>();

	private AudioLoop currentNPCLoop;
	private List<SongRecord> npcSongRecord = new List<SongRecord>();

	private List<EncounterEffect> encounterEffects = new List<EncounterEffect>();

	private int	numMovesPerTurn = 8;
	private int numBeatsPerMove = 2;
	
	private int currentPlayerMoveIndex = 0;
	private List<int> currentTurnLoops = new List<int>();
	private List<int> previousTurnLoops = new List<int>();

	protected int victoryPoints = 0;
	private int victoryPointGainPerPassedTurn = 2;
	private int victoryPointLossPerFailedTurn = 1;
	private int minVPsToWin = 8;

	private int jammage = 0;
	private int jammageMatchMultiplier = 2;

	// The following are only used in jammage mode = target
	private int jammageTargetLower = 10;
	private int jammageTargetUpper = 14;
	private int minJammageTarget = 8;
	private int maxJammageTarget = 28;
	private int maxJammageTargetRange = 6;
	private int currentJammageTargetRange = 6;

	private int phraseCompleteStaminaBonus = 4;

	// Rhythm bonus stuff
	RhythmStringBonusBracket[] bonusBrackets = new RhythmStringBonusBracket[2]{
		new RhythmStringBonusBracket(0.45f,1),
		new RhythmStringBonusBracket(1f,2)
	};

	// Initialization

	void Awake() {
		NotificationBoard.AddListener(PlayerMidiController.Notifications.changedSelectedLoop, DidChangeCurrentPlayerLoop);
		NotificationBoard.AddListener(SongStructureManager.Notifications.didStartSong, DidStartSong);
		songStructureManager = Object.FindObjectOfType<MidiSongStructureManager>();
		songStructureManager.RegisterSongUpdateListener(this);

		var npc = Object.FindObjectOfType<AIMIDIController>();
		npc.AddListener(this);

		// Initialize the player controller's loop names
		var playerController = Object.FindObjectOfType<PlayerMidiController>();
		var loopNames = new List<string>();
		foreach(var move in playerMoves) {
			loopNames.Add(move.loopName);
		}
		playerController.loopNames = loopNames.ToArray();
	}

	void Start() {
		NotificationBoard.SendNotification(Notifications.updatedJammage, this, jammage);

		var args = new Notifications.SetJammageTargetRangeArgs();
		args.lowerTarget = jammageTargetLower;
		args.upperTarget = jammageTargetUpper;
		NotificationBoard.SendNotification(Notifications.setJammageTargetRange, this, args);

		// encounterEffects.Add(new StatusForcedVariety());
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(PlayerMidiController.Notifications.changedSelectedLoop, DidChangeCurrentPlayerLoop);
		NotificationBoard.RemoveListener(SongStructureManager.Notifications.didStartSong, DidStartSong);
	}

	// Public functions
	public void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {
		// Add to NPC song record
		var newSongRecordEntry = new SongRecord();
		newSongRecordEntry.loop = currentNPCLoop;
		newSongRecordEntry.startBeat = beatInfo.currentBeat;
		npcSongRecord.Add(newSongRecordEntry);

		if(beatInfo.currentBeat % numBeatsPerMove == 0) {
			DoNextMove(beatInfo.currentBeat / numBeatsPerMove, beatInfo);
		}

		CheckForPlayerAndNPCRhythmMatchesOnPulses(beatInfo.currentBeat);
	}

	public void DidFinishSong() { 
		if(victoryPoints >= minVPsToWin) {
			NotificationBoard.SendNotification(Notifications.playerWon, this, null);
		} else {
			NotificationBoard.SendNotification(Notifications.playerLost, this, null);
		}

		// In whatever tracks musical encounters, update information about the current musical encounter
		// Determine the success level for the number of VPs we have
		var successLevel = GetSuccessLevel();
		MusicalEncounterManager.CompletedMusicalEncounter(successLevel);

		// DEBUG
		StartCoroutine(ReturnToOverworld());
	}

	// IAIListener
	public void DidChangeAILoop(AIMIDIController ai, AudioLoop loop) {
		currentNPCLoop = loop;
	}

	public void DidChangeLead(bool aiIsLeading) {}

	// Private Methods

	private IEnumerator ReturnToOverworld() {
		yield return new WaitForSeconds(3f);
		var transitionManager = Object.FindObjectOfType<TransitionManager>();
		transitionManager.ReturnToOverworld();
	}

	void DidStartSong(object sender, object arg) {
		UpdateStaminaAndJammageWithNextPlayerMove(currentPlayerMoveIndex, 0);
	}

	void DidChangeCurrentPlayerLoop(object sender, object arg) {
		currentPlayerMoveIndex = (int)arg;
	}

	void DoNextMove(int moveNumber, SongStructureManager.BeatUpdateInfo beatInfo) {
		// If this is the start of a new section, we actually set the jammage target to something that is possible - ie don't account for doubling.
		if(beatInfo.beatsUntilNextSection == beatInfo.currentSection.beatLength) {
			// Set the jammage target to within a smaller.
			currentJammageTargetRange = maxJammageTargetRange;
			jammageTargetLower = Random.Range(minJammageTarget,maxJammageTarget/2);
			jammageTargetUpper = jammageTargetLower + currentJammageTargetRange;
		}	

		// Start the next turn if no moves have been done
		if(currentTurnLoops.Count == 0) {
			StartNextTurn();
		}

		// If this is the start of a new section, we should reset the song history if that's the mode we're doing
		if(beatInfo.beatsUntilNextSection == beatInfo.currentSection.beatLength) {
			// Reset the previous turn loops if this is the start of a new turn
			previousTurnLoops.Clear();
			var previousTurnLoopsNotificationInfo = new Notifications.SetPreviousPhrasePlayerLoopsArgs();
			NotificationBoard.SendNotification(Notifications.setPreviousPhrasePlayerLoops, this, previousTurnLoopsNotificationInfo);
		}

		// Do the beat update
		UpdateStaminaAndJammageWithNextPlayerMove(currentPlayerMoveIndex, moveNumber);

		// For all of the effects, let them know that a new turn started
		foreach(var effect in encounterEffects) {
			effect.DoNextMove(this);
		}

		// If the turn is complete, handle what we should do for completing it
		if(IsCurrentTurnComplete()) {
			CompleteTurn();
		}

		// Then remove any effects that should be removed
		for(var i = 0 ; i < encounterEffects.Count ; ) {
			if(encounterEffects[i].ShouldBeRemoved(this)) {
				encounterEffects.RemoveAt(i);
			} else {
				i++;
			}
		}
	}

	void UpdateStaminaAndJammageWithNextPlayerMove(int playerMoveIndex, int beatNumber) {
		currentTurnLoops.Add(playerMoveIndex);
		var currentPlayerMove = playerMoves[playerMoveIndex];
		var thisTurnJammage = currentPlayerMove.jammageGain;

		// If the current loop matches the previous loop in the turn, add to the stamina recharge meter
		var previousTurnMoveIndex = currentTurnLoops.Count - 1;
		if(previousTurnMoveIndex < previousTurnLoops.Count) {
			var previousTurnMove = previousTurnLoops[previousTurnMoveIndex];
			if(previousTurnMove == playerMoveIndex) {
				thisTurnJammage *= jammageMatchMultiplier;
			}
		}

		jammage += thisTurnJammage;

		// Send all the notifications for things that happened
		var notificationInfo = new Notifications.SetPlayerLoopForBeatArgs();
		notificationInfo.playerMove = currentPlayerMove;
		notificationInfo.playerMoveIndex = playerMoveIndex;
		notificationInfo.beatNumber = beatNumber;
		NotificationBoard.SendNotification(Notifications.setPlayerLoopForBeat, this, notificationInfo);
		NotificationBoard.SendNotification(Notifications.updatedJammage, this, jammage);
	}

	void CompleteTurn() {
		// Do the victory point adding/subtracting
		var bonusMultiplier = GetBonusMultiplierForThisTurn();
		Debug.Log("MULTIPLIER: " + bonusMultiplier);

		if(jammage >= jammageTargetLower && jammage <= jammageTargetUpper) {
			victoryPoints += Mathf.FloorToInt(bonusMultiplier * victoryPointGainPerPassedTurn);
			// A perfect turn is if the player gets to the upper limit without going over.
			if(jammage == jammageTargetUpper) {
				NotificationBoard.SendNotification(Notifications.playerGotPerfectTurn, this, null);
			}
		} else {
			// The bonus multiplier is still favorable for VP loss - 
			// it reduces the amount of VPs you lose when you fail the turn
			victoryPoints -= Mathf.FloorToInt(victoryPointLossPerFailedTurn / bonusMultiplier);
		}

		// Update the status effects before we reset all of the turn's parameters
		// Note: Should we do this before calculating VPs? 
		foreach(var effect in encounterEffects) {
			effect.FinishedTurn(this);
		}

		// Reset everything that needs resetting
		jammage = 0;
		jammageTargetLower = Random.Range(minJammageTarget,maxJammageTarget - currentJammageTargetRange);
		jammageTargetUpper = jammageTargetLower + currentJammageTargetRange;
		currentJammageTargetRange = Mathf.Max(0, currentJammageTargetRange - (Random.value < 0.5f ? 1 : 2));
		if(previousTurnLoops.Count == 0) {
			previousTurnLoops = new List<int>(currentTurnLoops);
		}
		currentTurnLoops.Clear();
	
		// Send notifications about everything that just updated
		var previousTurnLoopsNotificationInfo = new Notifications.SetPreviousPhrasePlayerLoopsArgs();
		previousTurnLoopsNotificationInfo.playerMoves = new List<PlayerMove>();
		previousTurnLoopsNotificationInfo.playerMoveIndices = new List<int>();
		foreach(var index in previousTurnLoops) {
			previousTurnLoopsNotificationInfo.playerMoves.Add(playerMoves[index]);
			previousTurnLoopsNotificationInfo.playerMoveIndices.Add(index);
		}
		NotificationBoard.SendNotification(Notifications.setPreviousPhrasePlayerLoops, this, previousTurnLoopsNotificationInfo);
		NotificationBoard.SendNotification(Notifications.updatedVictoryPoints, this, victoryPoints);
		if(bonusMultiplier > 1) {
			NotificationBoard.SendNotification(Notifications.gotRhythmMatchBonus, this, bonusMultiplier);
		}
		npcSongRecord.Clear();
	}

	void StartNextTurn() {
		var args = new Notifications.SetJammageTargetRangeArgs();
		args.lowerTarget = jammageTargetLower;
		args.upperTarget = jammageTargetUpper;
		NotificationBoard.SendNotification(Notifications.setJammageTargetRange, this, args);
	}

	void CheckForPlayerAndNPCRhythmMatchesOnPulses(int beatNumber) {
		// Right now and in one eighth note, do this check
		StartCoroutine(CheckForRhythmMatch(0,beatNumber,false));
		StartCoroutine(CheckForRhythmMatch(0.5f / (float)songStructureManager.bpm * 60, beatNumber, true));
	}

	IEnumerator CheckForRhythmMatch(float inTime, int beatNumber, bool checkUpbeat) {
		yield return new WaitForSeconds(inTime);

		if(npcSongRecord.Count > 0) {
			// Now do the check
			var npcRhythmSubstring = npcSongRecord[npcSongRecord.Count-1].loop.rhythmString.GetRhythmStringForBeat(beatNumber);
			var playerRhythmSubstring = playerMoves[currentPlayerMoveIndex].loop.rhythmString.GetRhythmStringForBeat(beatNumber);
			var numMatches = 0;
			if(!checkUpbeat) {
				numMatches = playerRhythmSubstring.GetNumRhythmStringMatchesOnDownbeat(npcRhythmSubstring);
			} else {
				numMatches = playerRhythmSubstring.GetNumRhythmStringMatchesOnUpbeat(npcRhythmSubstring);
			}
			if(numMatches > 0) {
				NotificationBoard.SendNotification(Notifications.playerAndNPCRhythmMatchedOnPulse,this,null);
			}
		}	
	}

	bool IsCurrentTurnComplete() {
		return currentTurnLoops.Count == numMovesPerTurn;
	}

	float GetBonusMultiplierForThisTurn() {
		// TODO: Want to return if the player and NPC rhytmically matched enough
		// Get the player's rhythm string over that time
		var playerRhythmString = new RhythmString("");
		foreach(var playerMoveIndex in currentTurnLoops) {
			var loop = playerMoves[playerMoveIndex].loop;
			var beatStart = playerMoveIndex*numBeatsPerMove;
			var beatEnd = beatStart+numBeatsPerMove;
			playerRhythmString = playerRhythmString.AppendRhythmString(loop.rhythmString.GetRhythmStringForBeatRange(beatStart,beatEnd));
		}	

		// Get NPC rhythm string
		var npcRhythmString = new RhythmString("");
		foreach(var record in npcSongRecord) {
			npcRhythmString = npcRhythmString.AppendRhythmString(record.loop.rhythmString.GetRhythmStringForBeat(record.startBeat));
		}

		// Compare the two rhythm strings
		var numMatches = playerRhythmString.GetNumRhythmStringMatches(npcRhythmString);
		var maxNumMatches = npcRhythmString.GetMaxNumRhythmStringMatches();
		var matchRatio = (float)numMatches/maxNumMatches;

		Debug.Log("MATCH RATIO: " + matchRatio);

		foreach(var bracket in bonusBrackets) {
			if(matchRatio <= bracket.maxMatchRatio) {
				return bracket.bonusMultiplier;
			}
		}

		return 1f;
	}

	private MusicalEncounterManager.SuccessLevel GetSuccessLevel() {
		// TODO: This should be calculatd in a way that scales appropriately for a given song
		var successRatio = victoryPoints / 32f;
		if(successRatio >= 1f) {
			return MusicalEncounterManager.SuccessLevel.Perfect;
		} else if (successRatio >= 0.75f) {
			return MusicalEncounterManager.SuccessLevel.Excel;
		} else if (successRatio >= 0.5f) {
			return MusicalEncounterManager.SuccessLevel.Pass;
		} else if (successRatio >= 0.25f) {
			return MusicalEncounterManager.SuccessLevel.PartialFailure;
		} else {
			return MusicalEncounterManager.SuccessLevel.TotalFailure;
		}
	}
}
