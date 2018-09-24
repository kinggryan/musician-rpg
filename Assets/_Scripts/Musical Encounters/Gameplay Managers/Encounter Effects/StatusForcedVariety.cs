using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RPGGameplayManger {
	public class StatusForcedVariety : RPGGameplayManger.EncounterEffect {

		public static class Notifications {
			/// <summary>
			/// This notification is posted when the player repeats a move and it results in a jammage punishment
			/// </summary>
			public static string playerRepeatedMove = "statusForcedVarietyPlayerRepeatedMove";
		}

		int turnLifetime = 3;
		int jammagePunishmentForRepeatedMove = 3;

		// Use this for initialization
		public override void DoNextMove(RPGGameplayManger gameplayManger) {
			var numMoves = gameplayManger.currentTurnLoops.Count;
			if(numMoves > 1 && gameplayManger.currentTurnLoops[numMoves-1] == gameplayManger.currentTurnLoops[numMoves-2]) {
				gameplayManger.jammage -= jammagePunishmentForRepeatedMove;
				NotificationBoard.SendNotification(Notifications.playerRepeatedMove, this, null);
			}
		}

		public override void FinishedTurn(RPGGameplayManger gameplayManger) {
			turnLifetime--;
		}

		public override bool ShouldBeRemoved(RPGGameplayManger gameplayManger) {
			return turnLifetime <= 0;
		}
	}
}
