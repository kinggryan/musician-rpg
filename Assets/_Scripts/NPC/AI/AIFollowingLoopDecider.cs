using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollowingLoopDecider : AILoopDecider {

	private float emotionScoreWeight = 4;
	private float rhythmScoreWeight = 1;

	public AIFollowingLoopDecider(AILoopDecider decider) : base(decider) {}
	public AIFollowingLoopDecider(List<AudioLoop> loops,  List<AudioLoop> songSpecificLoops, SongSection[] songStructure) : base(loops, songSpecificLoops, songStructure) {}

	public override AudioLoop ChooseLoopToPlay() {
		// If the song has specific a loop to play, change loops
		var songSpecificLoopToPlay = GetSongSpecifiedLoopForNextBeat();
		if(songSpecificLoopToPlay != null)
			return songSpecificLoopToPlay;

		if(currentBeatNumber % 16 != 0) {
			return null;
		}

		// Get the player loops over the past N beats
		var playerPattern = GetPlayerSongRecordForBeatRange(currentBeatNumber - 16, currentBeatNumber);
		var playerLoops = new List<AudioLoop>();
		foreach(var p in playerPattern) {
			playerLoops.Add(p.loop);
		}

		// Find the most common emotion tags
		var targetEmotionTags = AudioLoop.GetMostCommonEmotionTagsInLoops(playerLoops);

		// Find the composite rhythm string
		var appendedRhythmString = GetRhythmStringForSongRecords(playerPattern, currentBeatNumber);

		var maxScore = Mathf.NegativeInfinity;
		AudioLoop bestLoop = null;

		// Iterate through our own loops
		foreach(var loop in knownLoops) {
			// determine an "emotion match" score for each loop
			var emotionScore = loop.NumMatchedEmotions(targetEmotionTags);

			// determine a "rythm match" score for each loop
			var rhythmScore = loop.GetRhythmStringScore(appendedRhythmString);
			Debug.Log("Found rhythm score of " + rhythmScore + " for loop " + loop.name);

			var score = emotionScore*emotionScoreWeight + rhythmScore*rhythmScoreWeight;
			if(score > maxScore) {
				maxScore = score;
				bestLoop = loop;
			}
		}

		Debug.Log("Best Loop: " + bestLoop.name);

		return bestLoop;
	}

	public override AILoopDecider UpdateState() {
		if(ShouldSwapLead()) {
			return new AILeadingLoopDecider(this);
		}
		return null;
	}
}
