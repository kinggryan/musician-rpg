using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RPGGameplayManger {
	public class StatusForcedVariety : RPGGameplayManger.EncounterEffect {

		int turnLifetime = 3;

		// Use this for initialization
		public override void DoNextMove(RPGGameplayManger gameplayManger) {
			turnLifetime--;
		}

		public override bool ShouldBeRemoved(RPGGameplayManger gameplayManger) {
			return turnLifetime <= 0;
		}
	}
}
