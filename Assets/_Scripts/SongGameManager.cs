using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongGameManager : MonoBehaviour {

	public UnityEngine.UI.Slider grooveBar;
	public string currentSongRhythmString;
	public string currentPlayerRhythmString;
	public float grooveIncreaseRate;
	public float grooveDecayRate;
	public float currentGroove;
	public bool songStarted = false;
	
	// Update is called once per frame
	void Update () {
		if(songStarted) {
			var currentGrooveModifier = grooveDecayRate + grooveIncreaseRate*AudioLoopGameData.ScorePlayerStringAgainstNPCString(currentSongRhythmString,currentPlayerRhythmString);
			currentGroove += currentGrooveModifier*Time.deltaTime;
			grooveBar.value = currentGroove;
		}
	}
}
