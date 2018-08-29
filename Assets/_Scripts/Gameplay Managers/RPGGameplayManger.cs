using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameplayManger : MonoBehaviour {

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
			public AudioLoop playerLoop;
			public int beatNumber;
		}
		/// <summary>
		/// This notification is called when a song phrase is complete and the player's previous phrase loops are locked in
		/// The argument is of type SetPreviousPhrasePlayerLoopsArgs
		/// </summary>
		public static string setPreviousPhrasePlayerLoops = "setPreviousPhrasePlayerLoops";
		public struct SetPreviousPhrasePlayerLoopsArgs {
			public List<AudioLoop> playerLoops;
		}
		/// <summary>
		/// The argument for this notification is an integer - the new number of victory points
		/// </summary>
		public static string updatedVictoryPoints = "updatedVictoryPoints";
		/// <summary>
		/// There is no argument for this notification. It is called when the player receives the rhythm match bonus.
		/// </summary>
		public static string gotRhythmMatchBonus = "gotRhythmMatchBonus";
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
