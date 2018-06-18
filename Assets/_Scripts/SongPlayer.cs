using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour {

	[System.Serializable]
	public struct SongPhrase {
		public AudioLoop loop;
		public string key;
	}

	private double songStartDSPTime;
	public double bpm;
	public AudioLoop[] playerAudioLoops;
	public SongPhrase[] songPhrases;

	public double currentLoopEndBeat;
	
	private double currentSongPhraseEndBeat;
	private double currentSongBeat;
	private int currentSongPhraseIndex = -1;

	// Update is called once per frame
	void Update () {
		// If we incremented to the next beat
		var previousBeat = currentSongBeat;
		currentSongBeat = GetCurrentBeat();
		// Debug.Log("Cur beat " + currentSongBeat + " current song phrase end beat " + currentSongPhraseEndBeat);
		if(currentSongBeat > previousBeat && currentSongBeat == currentSongPhraseEndBeat - 1) {
			PlayNextPhrase();
		}
	}

	public void StartSong() {
		songStartDSPTime = AudioSettings.dspTime;
		PlayNextPhrase();
	}

	public void PlayClipNextBeat() {
		var currentBeat = GetCurrentBeat();
		var beatToStartAt = (currentLoopEndBeat > currentBeat) ? currentLoopEndBeat : currentBeat + 1;
		var beatToStartAtDSPTime = ConvertBeatToDSPTime(beatToStartAt);
		playerAudioLoops[0].PlayLoop(beatToStartAtDSPTime);
		currentLoopEndBeat = beatToStartAt + playerAudioLoops[0].numBeats;
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
		currentSongPhraseIndex++;
		if(currentSongPhraseIndex >= songPhrases.Length) {
			Debug.Log("Finished song!");
		} else {
			var nextPhrase = songPhrases[currentSongPhraseIndex];
			var nextPhraseStartDSPTime = ConvertBeatToDSPTime(currentSongPhraseEndBeat);
			currentSongPhraseEndBeat += nextPhrase.loop.numBeats;
			Debug.Log("Playing phrase "+ nextPhrase + " on beat " + currentSongPhraseEndBeat);
			nextPhrase.loop.PlayLoop(nextPhraseStartDSPTime);
		}
	}

	// void ProgressSong() {
		
	// }
}
