using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour {

	private double songStartDSPTime;
	public double bpm;
	public AudioLoop[] audioLoop;

	public double currentLoopEndBeat;

	// Use this for initialization
	void Start () {
		StartSong();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartSong() {
		songStartDSPTime = AudioSettings.dspTime;
		// Tell sound event to play main song?
	}

	public void PlayClipNextBeat() {
		var currentBeat = GetCurrentBeat();
		var beatToStartAt = (currentLoopEndBeat > currentBeat) ? currentLoopEndBeat : currentBeat + 1;
		var beatToStartAtDSPTime = ConvertBeatToDSPTime(beatToStartAt);
		audioLoop[0].PlayLoop(beatToStartAtDSPTime);
		currentLoopEndBeat = beatToStartAt + audioLoop[0].numBeats;
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
}
