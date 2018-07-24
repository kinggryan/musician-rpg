using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LoopsRPGManager))]
public class LoopsRPGMusicGameLogicTranslator : MonoBehaviour {
	
	const int numBeatsPerPlayerMove = 16;
	const int playerMoveBeatOffset = 8;
	const int numBeatsPerNPCMove = 16;
	const int virtuosoLoopCount = 3;

	LoopsRPGManager rpgManager;
	int numPlayerTracksPerMove = 4;
	List<int> playerMoveTracks = new List<int>();

	int currentSongBeat = 0;
	int currentPlayerTrack = 0;

	// Maps the player's track indices to musical roles
	LoopsRPGManager.MusicalRole[] playerTrackRoles = {LoopsRPGManager.MusicalRole.Rhythm, LoopsRPGManager.MusicalRole.Melody, LoopsRPGManager.MusicalRole.Harmony};

	void Awake() {
		rpgManager = GetComponent<LoopsRPGManager>();
	}

	void DidPlayPlayerTrack(int playerTrackIndex) {
		// Debug.Log("Adding player track " + playerTrackIndex + " at time " + Time.time);
		currentPlayerTrack = playerTrackIndex;	
	}

	void AddToPlayerMoveSet() {
		playerMoveTracks.Add(currentPlayerTrack);
		if(playerMoveTracks.Count >= 4) {
			InterpretPlayerMove(playerMoveTracks.ToArray());
			playerMoveTracks.Clear();
		}
	}

	void DidStartNextBeat(SongPlayer.BeatUpdateInfo beatInfo) {
		currentSongBeat++;
		if(currentSongBeat % numBeatsPerNPCMove == 0) {
			rpgManager.MakeNPCMove();
		}
		if(currentSongBeat % 4 == 0) {
			AddToPlayerMoveSet();
		}
	}

	void InterpretPlayerMove(int[] moveTracks) {
		// Get the track roles for these moves
		var moveRoles = new List<LoopsRPGManager.MusicalRole>();
		var uniqueTracks = new HashSet<int>();
		foreach(var trackIndex in moveTracks) {
			moveRoles.Add(playerTrackRoles[trackIndex]);
			uniqueTracks.Add(trackIndex);
		}

		var finalRoleOfMove = moveRoles[moveRoles.Count-1];

		// If three different loops were involved in the move, the player goes all virtuosic
		if(uniqueTracks.Count >= virtuosoLoopCount) {
			rpgManager.PlayerVirtuoso();
		}
		// If the last role is different than the player's role, switch to that role
		if(finalRoleOfMove != rpgManager.playerRole) {
			rpgManager.PlayerChangeRoles(finalRoleOfMove);
		}
		// If only two loops were played and we ended in the same role, the player chilled
		if(finalRoleOfMove == rpgManager.playerRole && uniqueTracks.Count <= 2) {
			rpgManager.PlayerChill();
		}
	}
}
