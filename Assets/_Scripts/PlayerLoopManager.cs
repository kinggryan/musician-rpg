﻿using System.Collections;
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
	public int[] playerLoops = {-1, -1, -1 ,-1};
	
	SongPlayer player;
	Mode mode = Mode.None;
	int currentRecordingBeat = -1;
	int currentRecordingSelectedLoopNumber = -1;
	int loopLengthBeats = 2;
	int fullRecordingBeats = 8;

	// Use this for initialization
	void Awake () {
		player = GetComponent<SongPlayer>();
	}
	
	public void StartRecording() {
		mode = Mode.RecordingLoop;
		currentRecordingBeat = -1;
		for(var i = 0 ; i < playerLoops.Length ; i++) {
			playerLoops[i] = -1;
		}
	}

	public void FinishRecording() {
		mode = Mode.PlayingLoop;
	}

	public void DidPlayPlayerTrack(int playerLoopNumber) {
		if(mode == Mode.RecordingLoop) {
			currentRecordingSelectedLoopNumber = playerLoopNumber;
		}
	}

	int PlayerLoopNumberForBeatNumber(int beatNum) {
		return playerLoops[beatNum/loopLengthBeats % playerLoops.Length];
	}

	void DidStartNextBeat(int newBeatNum) {
		// Don't do anything unless we startd the beat before the next loop
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
					currentRecordingBeat++;
					if(currentRecordingBeat == fullRecordingBeats) {
						FinishRecording();
					} else {
						// If we reached the next beat, then set the current recording beat
						playerLoops[currentRecordingBeat] = currentRecordingSelectedLoopNumber;
					}
					break;
				}
			}
		}
	}
}
