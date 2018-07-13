using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopManager : MonoBehaviour {

	enum Mode {
		None,
		RecordingLoop,
		PlayingLoop
	}

	// This class is responsible for recording the player's loop
	// For now, let's just assume all player loops are 4 beats long
	public List<int> playerLoops = new List<int>();
	
	SongPlayer player;
	PlayerLoopDisplay display;
	Mode mode = Mode.None;
	int currentRecordingLoopIndex = -1;
	int currentRecordingSelectedLoopNumber = -1;
	int loopLengthBeats = 2;

	// Use this for initialization
	void Awake () {
		player = GetComponent<SongPlayer>();
		display = GetComponent<PlayerLoopDisplay>();
	}
	
	public void StartRecording() {
		mode = Mode.RecordingLoop;
		currentRecordingLoopIndex = -1;
		playerLoops.Clear();
	}

	public void FinishRecording() {
		if(playerLoops.Count > 0) {
			mode = Mode.PlayingLoop;
			player.ChangePlayerLoop(playerLoops[0]);
		} else {
			mode = Mode.None;
			player.StopPlayerLoops();
		}
	}

	public void DidPlayPlayerTrack(int playerLoopNumber) {
		if(mode == Mode.RecordingLoop) {
			currentRecordingSelectedLoopNumber = playerLoopNumber;
		}
	}

	int PlayerLoopIndexForBeatNumber(int beatNum) {
		return beatNum/loopLengthBeats % playerLoops.Count;
	}

	int PlayerLoopNumberForBeatNumber(int beatNum) {
		return playerLoops[PlayerLoopIndexForBeatNumber(beatNum)];
	}

	void DidStartNextBeat(SongPlayer.BeatUpdateInfo beatInfo) {
		// Don't do anything unless we startd the beat before the next loop
		var newBeatNum = beatInfo.currentBeat;
		if(newBeatNum % loopLengthBeats == loopLengthBeats - 1) {
			// schedule the next player loop
			switch(mode) {
				// If playing, play the next loop
				case Mode.PlayingLoop: {
					// Get the loop for the next beat and change to it - the current beat is already playing
					var loopNumber = PlayerLoopNumberForBeatNumber(newBeatNum + 1);
					if(loopNumber >= 0)
						player.ChangePlayerLoop(loopNumber);
					break;
				}
				// If recording, increment the recording beat and finish automatically if we've recorded 4 beats
				case Mode.RecordingLoop: {
					currentRecordingLoopIndex++;
					
					// If we reached the next beat, then set the current recording beat
					if(currentRecordingSelectedLoopNumber >= 0) {
						Debug.Log(Time.time + " adding loop number " + currentRecordingSelectedLoopNumber);
						playerLoops.Add(currentRecordingSelectedLoopNumber);
						display.SetLoopNumberForIndex(currentRecordingSelectedLoopNumber, currentRecordingLoopIndex);
					}
					break;
				}
			}
		}
	}
}
