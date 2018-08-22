using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILeadingLoopDecider : AILoopDecider {

	public AILeadingLoopDecider(AILoopDecider decider) : base(decider) {}
	public AILeadingLoopDecider(List<AudioLoop> loops, SongSection[] songStructure) : base(loops,songStructure) {}

	public override AudioLoop ChooseLoopToPlay() {
		// Get the player loops over the past N beats
		var songPhrases = GetSongPhrasesForBeatRange(currentBeatNumber - 16, currentBeatNumber);

		// Find the most common emotion tags
		var targetEmotionTags = SongPhrase.GetMostCommonEmotionsInPhrases(songPhrases);

		var maxScore = Mathf.NegativeInfinity;
		AudioLoop bestLoop = null;

		// Iterate through our own loops
		foreach(var loop in knownLoops) {
			// determine an "emotion match" score for each loop
			var emotionScore = loop.NumMatchedEmotions(targetEmotionTags);

			if(emotionScore > maxScore) {
				maxScore = emotionScore;
				bestLoop = loop;
			}
		}

		Debug.Log("Best Loop: " + bestLoop.name);

		return bestLoop;
	}

	public override AILoopDecider UpdateState() {
		if(ShouldSwapLead()) {
			return new AIFollowingLoopDecider(this);
		}
		return null;
	}
}
