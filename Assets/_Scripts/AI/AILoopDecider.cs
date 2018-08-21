using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class decides which loops to play for the AI
/// </summary>
public abstract class AILoopDecider {

	protected List<AudioLoop> knownLoops;
	protected List<AudioLoop> previousPlayerLoops = new List<AudioLoop>();
	protected SongPhrase currentSongPhrase;
	protected int currentBeatNumber;
	protected int numBeatsPerAILoop = 16;

	public AILoopDecider(List<AudioLoop> loops) {
		this.knownLoops = loops;
	}

	public abstract AudioLoop ChooseLoopToPlay();

	public void DidStartPlayerLoop(AudioLoop playerLoop) {
		// This should be tracked in some way
		previousPlayerLoops.Add(playerLoop);
	}

	public void DidStartNewSongPhrase(SongPhrase songPhrase) {
		// This should also be tracked
		currentSongPhrase = songPhrase;
	}

	public void DidStartNextBeat() {
		currentBeatNumber++;
	}

	protected List<AudioLoop> GetPlayerLoopsBetweenBeats(int startBeat, int endBeat) {
		var loopsInRange = new List<AudioLoop>();
		var checkBeat = 0;

		// TODO: Make this work for loops which are playing while the start beat starts.
		// For now, let's just assume this won't happen
		foreach(var loop in previousPlayerLoops) {
			if(checkBeat >= startBeat) {
				loopsInRange.Add(loop);
			}
			checkBeat += loop.beatDuration;
			if(checkBeat >= endBeat) {
				break;
			}
		}

		return loopsInRange;
	}

	// to make a decision about what loops to play
	// you need to know
	// - What the current player pattern is (over the course of some number of bars)
	//	- so when the player starts a loop, we need to hear that
	// - What the current song phrase is (for emotion tags and rhythm string)

}
