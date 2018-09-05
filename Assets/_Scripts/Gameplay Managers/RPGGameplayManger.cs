using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameplayManger : MonoBehaviour, ISongUpdateListener {

	public static class Notifications {
		/// <summary>
		/// The argument for this notification is an integer - the new jammage threshold
		/// </summary>
		public static string setJammageThreshold = "setJammageThreshold";
		/// <summary>
		/// The argument for this notification is an integer - the new jammage 
		/// </summary>
		public static string updatedJammage = "updatedJammage";
		/// <summary>
		/// The argument for this notification is an integer - the new stamina 
		/// </summary>
		public static string updatedStamina = "updatedStamina";
		/// <summary>
		/// The argument for this notification is an integer - the new stamina recharge meter level
		/// </summary>
		public static string updatedStaminaRechargeMeter = "updatedStaminaRechargeMeter";
		/// <summary>
		/// There is no argument for this notification. It is called when the stamina recharges via the recharge meter
		/// </summary>
		public static string staminaRecharged = "staminaRecharged";
		/// <summary>
		/// There is no argument for this notification. 
		/// It is called when the stamina recharge meter is overfilled, causing the player to lose their stamina recharge.
		/// </summary>
		public static string staminaRechargeMeterOverloaded = "staminaRechargeMeterOverloaded";
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
		public static string updatedMaxStamina = "updatedMaxStamina";
		/// <summary>
		/// The argument for this notification is an integer - the new number of victory points
		/// </summary>
		public static string updatedVictoryPoints = "updatedVictoryPoints";
		/// <summary>
		/// There is no argument for this notification. It is called when the player receives the rhythm match bonus.
		/// </summary>
		public static string gotRhythmMatchBonus = "gotRhythmMatchBonus";
	}

	// The list of the player's moves
	public List<PlayerMove> playerMoves = new List<PlayerMove>();

	private int	numMovesPerTurn = 8;
	private int numBeatsPerMove = 2;
	
	private int currentPlayerMoveIndex = 0;
	private List<int> currentTurnLoops = new List<int>();
	private List<int> previousTurnLoops = new List<int>();

	private int victoryPoints = 0;
	private int victoryPointGainPerPassedTurn = 2;
	private int victoryPointLossPerFailedTurn = 1;

	private int jammage = 0;
	private int jammageThreshold = 8;
	private int minJammageThreshold = 8;
	private int maxJammageThreshold = 12;

	private int stamina = 16;
	private int maxStamina = 32;
	private int jammageLossForNotEnoughStamina = 2;

	private int staminaRechargeMeter = 0;
	private int staminaRechargeMeterThreshold = 6;
	private int staminaRechargeMeterMax = 7;
	private int defaultStaminaRechargePerTurn = 4;
	private int staminaRechargeFromMeterPerTurn = 8;

	public void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {
		if(beatInfo.currentBeat % numBeatsPerMove == 0) {
			DoNextMove(beatInfo.currentBeat / numBeatsPerMove);
		}
	}

	public void DidFinishSong() {

	}

	void Awake() {
		NotificationBoard.AddListener(PlayerMidiController.Notifications.changedSelectedLoop, DidChangeCurrentPlayerLoop);
		NotificationBoard.AddListener(SongStructureManager.Notifications.didStartSong, DidStartSong);
		var songStructureManager = Object.FindObjectOfType<MidiSongStructureManager>();
		songStructureManager.RegisterSongUpdateListener(this);

	}

	void Start() {
		NotificationBoard.SendNotification(Notifications.updatedJammage, this, jammage);
		NotificationBoard.SendNotification(Notifications.setJammageThreshold, this, jammageThreshold);
		NotificationBoard.SendNotification(Notifications.updatedStamina, this, stamina);
		NotificationBoard.SendNotification(Notifications.updatedStaminaRechargeMeter, this, staminaRechargeMeter);
		NotificationBoard.SendNotification(Notifications.updatedMaxStamina, this, maxStamina);
	}

	void DidStartSong(object sender, object arg) {
		UpdateStaminaAndJammageWithNextPlayerMove(currentPlayerMoveIndex, 0);
	}

	void DidChangeCurrentPlayerLoop(object sender, object arg) {
		currentPlayerMoveIndex = (int)arg;
	}

	void DoNextMove(int moveNumber) {
		// Do the beat update
		UpdateStaminaAndJammageWithNextPlayerMove(currentPlayerMoveIndex, moveNumber);

		// If the turn is complete, handle what we should do for completing it
		if(IsCurrentTurnComplete()) {
			CompleteTurn();
		}
	}

	void UpdateStaminaAndJammageWithNextPlayerMove(int playerMoveIndex, int beatNumber) {
		currentTurnLoops.Add(playerMoveIndex);
		var currentPlayerMove = playerMoves[playerMoveIndex];
		jammage += currentPlayerMove.jammageGain;
		
		// If the player doesn't have enough stamina, you lose jammage
		if(stamina >= currentPlayerMove.staminaCost) {
			stamina -= currentPlayerMove.staminaCost;
		} else {
			stamina = 0;
			jammage -= jammageLossForNotEnoughStamina;
		}

		// If the current loop matches the previous loop in the turn, add to the stamina recharge meter
		var previousTurnMoveIndex = currentTurnLoops.Count - 1;
		if(previousTurnMoveIndex < previousTurnLoops.Count) {
			var previousTurnMove = previousTurnLoops[previousTurnMoveIndex];
			if(previousTurnMove == playerMoveIndex) {
				staminaRechargeMeter++;
				NotificationBoard.SendNotification(Notifications.updatedStaminaRechargeMeter, this, staminaRechargeMeter);
			}
		}

		// Send all the notifications for things that happened
		var notificationInfo = new Notifications.SetPlayerLoopForBeatArgs();
		notificationInfo.playerMove = currentPlayerMove;
		notificationInfo.playerMoveIndex = playerMoveIndex;
		notificationInfo.beatNumber = beatNumber;
		NotificationBoard.SendNotification(Notifications.setPlayerLoopForBeat, this, notificationInfo);
		NotificationBoard.SendNotification(Notifications.updatedJammage, this, jammage);
		NotificationBoard.SendNotification(Notifications.updatedStamina, this, stamina);
	}

	void CompleteTurn() {
		// Do the victory point adding/subtracting
		var bonusMultiplier = GetBonusMultiplierForThisTurn();
		if(jammage >= jammageThreshold) {
			victoryPoints += Mathf.FloorToInt(bonusMultiplier * victoryPointGainPerPassedTurn);
		} else {
			// The bonus multiplier is still favorable for VP loss - 
			// it reduces the amount of VPs you lose when you fail the turn
			victoryPoints -= Mathf.FloorToInt(victoryPointLossPerFailedTurn / bonusMultiplier);
		}

		// Handle the stamina recharge meter
		stamina += defaultStaminaRechargePerTurn;
		if(staminaRechargeMeter >= staminaRechargeMeterThreshold && staminaRechargeMeter <= staminaRechargeMeterMax) {
			stamina += staminaRechargeFromMeterPerTurn;
		}

		// Reset everything that needs resetting
		staminaRechargeMeter = 0;
		jammage = 0;
		jammageThreshold = Random.Range(minJammageThreshold,maxJammageThreshold);
		previousTurnLoops = new List<int>(currentTurnLoops);
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
		NotificationBoard.SendNotification(Notifications.staminaRecharged, this, stamina);
		NotificationBoard.SendNotification(Notifications.setJammageThreshold, this, jammageThreshold);
		NotificationBoard.SendNotification(Notifications.updatedVictoryPoints, this, victoryPoints);
		if(bonusMultiplier > 1) {
			NotificationBoard.SendNotification(Notifications.gotRhythmMatchBonus, this, bonusMultiplier);
		}
	}

	bool IsCurrentTurnComplete() {
		return currentTurnLoops.Count == numMovesPerTurn;
	}

	float GetBonusMultiplierForThisTurn() {
		// TODO: Want to return if the player and NPC rhytmically matched enough
		return 1f;
	}
}
