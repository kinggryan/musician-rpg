using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollowingLoopDecider : AILoopDecider {

	private float emotionScoreWeight = 4;
	private float rhythmScoreWeight = 1;

	public AIFollowingLoopDecider(List<AudioLoop> loops) : base(loops) {}

	public override AudioLoop ChooseLoopToPlay() {
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

			var score = emotionScore*emotionScoreWeight + rhythmScore*rhythmScoreWeight;
			if(score > maxScore) {
				maxScore = score;
				bestLoop = loop;
			}
		}

		return bestLoop;
	}
}
