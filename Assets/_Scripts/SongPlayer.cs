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
	
	private double currentSongAudioLoopEndBeat;
	private double currentSongBeat;

	// Update is called once per frame
	void Update () {
		// If we incremented to the next beat
		// see if it's the beat before the end beat of the current song loop
		// if so, play the next one at the next 
	}

	public void StartSong() {
		songStartDSPTime = AudioSettings.dspTime;
		PlaySong();
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

	void PlaySong() {
		Debug.Log("Playing song!");
		var currentBeat = 0;
		// foreach(var loop in songAudioLoops) {
		// 	var currentDSPTime = ConvertBeatToDSPTime(currentBeat);
		// 	loop.PlayLoop(currentDSPTime);
		// 	currentBeat += loop.numBeats;
		// 	Debug.Log("Playing " + loop + " at beat " + currentBeat);
		// }
	}

	// void ProgressSong() {
		
	// }
}
