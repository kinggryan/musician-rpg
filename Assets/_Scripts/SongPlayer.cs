using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour {

	private double songStartDSPTime;
	public double bpm;
	public AudioLoop[] audioLoop;

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
		var nextBeat = GetCurrentBeat() + 1;
		var nextBeatDSPTime = ConvertBeatToDSPTime(nextBeat);
		audioLoop[0].PlayLoop(nextBeatDSPTime);

		// TODO: Tell sound event to play on this dsp time?
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
