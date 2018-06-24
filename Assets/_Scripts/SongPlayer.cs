using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour {

	[System.Serializable]
	public struct SongPhrase {
		public AudioLoop loop;
		public string key;
		public int numTimesToPlay;
	}

	private double songStartDSPTime;
	public double bpm;
	public AudioLoop[] playerAudioLoops;
	public SongPhrase[] songPhrases;

	public double currentPlayerLoopEndBeat;
	public int currentPlayerLoopIndex;
	
	private double currentSongPhraseEndBeat;
	private double currentSongBeat;
	private int currentSongPhraseIndex = -1;
	private int currentSongPhraseNumRepeatsRemaining = 0;

	// Update is called once per frame
	void Update () {
		// If we incremented to the next beat
		var previousBeat = currentSongBeat;
		currentSongBeat = GetCurrentBeat();
		// Debug.Log("Cur beat " + currentSongBeat + " current song phrase end beat " + currentSongPhraseEndBeat);
		if(currentSongBeat > previousBeat) {
			if(currentSongBeat == currentSongPhraseEndBeat - 1)
				PlayNextPhrase();
			if(currentSongBeat == currentPlayerLoopEndBeat - 1)
				ContinuePlayerLoop();
		}
	}

	public void StartSong() {
		songStartDSPTime = AudioSettings.dspTime;
		PlayNextPhrase();
	}

	public void ChangePlayerLoop(int loopIndex) {
		// We want to start this player loop at the next beat
		var beatToStartAt = currentPlayerLoopEndBeat > 0 ? currentPlayerLoopEndBeat : currentSongPhraseEndBeat;
		var beatToStartAtDSPTime = ConvertBeatToDSPTime(beatToStartAt);
		playerAudioLoops[loopIndex].PlayLoop(beatToStartAtDSPTime);
		currentPlayerLoopEndBeat = beatToStartAt + playerAudioLoops[loopIndex].numBeats;
		currentPlayerLoopIndex = loopIndex;
	}

	double GetCurrentBeat() {
		return System.Math.Floor(ConvertDSPTimeToBeat(AudioSettings.dspTime));
	}

	double ConvertDSPTimeToBeat(double dspTime) {
		return bpm / 60 * (dspTime - songStartDSPTime);
	}

	double ConvertBeatToDSPTime(double beat) {
		return (beat / bpm * 60) + songStartDSPTime;
	}

	void PlayNextPhrase() {
		// Keep looping this loop if we should repeat it more, otherwise go to the next loop
		if(currentSongPhraseNumRepeatsRemaining > 0) {
			currentSongPhraseNumRepeatsRemaining--;
		} else {
			currentSongPhraseIndex++;
			if(currentSongPhraseIndex >= songPhrases.Length)
				currentSongPhraseNumRepeatsRemaining = songPhrases[currentSongPhraseIndex].numTimesToPlay - 1;
		}

		// Song is done if outside range
		if(currentSongPhraseIndex >= songPhrases.Length) {
			Debug.Log("Finished song!");
		} else {
			// Continue to the next phrase
			var nextPhrase = songPhrases[currentSongPhraseIndex];
			var nextPhraseStartDSPTime = ConvertBeatToDSPTime(currentSongPhraseEndBeat);
			currentSongPhraseEndBeat += nextPhrase.loop.numBeats;
			Debug.Log("Playing phrase "+ nextPhrase + " on beat " + currentSongPhraseEndBeat);
			nextPhrase.loop.PlayLoop(nextPhraseStartDSPTime);
		}
	}

	// Continues looping the current player loop
	void ContinuePlayerLoop() {
		var nextPhraseStartDSPTime = ConvertBeatToDSPTime(currentPlayerLoopEndBeat);
		currentPlayerLoopEndBeat += playerAudioLoops[currentPlayerLoopIndex].numBeats;
		// Debug.Log("Playing phrase "+ nextPhrase + " on beat " + currentSongPhraseEndBeat);
		playerAudioLoops[currentPlayerLoopIndex].PlayLoop(nextPhraseStartDSPTime);
	}
}
