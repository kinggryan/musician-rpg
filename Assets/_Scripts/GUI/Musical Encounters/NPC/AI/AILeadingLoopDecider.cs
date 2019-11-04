using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILeadingLoopDecider : AILoopDecider {

	public AILeadingLoopDecider(AILoopDecider decider) : base(decider) {}
	public AILeadingLoopDecider(List<AudioLoop> loops, List<AudioLoop> songSpecificLoops, SongSection[] songStructure) : base(loops, songSpecificLoops, songStructure) {}

	public override AudioLoop ChooseLoopToPlay() {
		// If the song has specific a loop to play, change loops
		var songSpecificLoopToPlay = GetSongSpecifiedLoopForNextBeat();
		if(songSpecificLoopToPlay != null)
			return songSpecificLoopToPlay;

		if(currentBeatNumber % 16 != 0) {
			return null;
		}

		// Get the player loops over the past N beats
		var songPhrases = GetSongPhrasesForBeatRange(currentBeatNumber, currentBeatNumber+16);

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

		// Debug.Log("Best Loop: " + bestLoop.name);

		return bestLoop;
	}

	public override AILoopDecider UpdateState() {
		if(ShouldSwapLead()) {
			return new AIFollowingLoopDecider(this);
		}
		return null;
	}
}
