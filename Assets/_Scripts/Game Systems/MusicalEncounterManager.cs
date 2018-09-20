using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MusicalEncounterManager {

	public enum SuccessLevel {
		TotalFailure,
		PartialFailure,
		Pass,
		Excel,
		Perfect
	}

	struct MusicalEncounterInfo {
		public string songFileName;
		// Other metadata about the song -  npc, environment, etc
		public SuccessLevel successLevel;
	}

	private static MusicalEncounterInfo currentEncounterInfo;

	public static void StartedMusicalEncounter(string songFileName) {
		currentEncounterInfo.songFileName = songFileName;
	}

	public static void CompletedMusicalEncounter(SuccessLevel successLevel) {
		currentEncounterInfo.successLevel = successLevel;
	}

	public static string GetCurrentMusicalEncounterSongFile() {
		return currentEncounterInfo.songFileName;
	}
}
