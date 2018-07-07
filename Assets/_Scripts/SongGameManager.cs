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

	private CharacterAnimationManager characters;
	
	void Awake() {
		characters = Object.FindObjectOfType<CharacterAnimationManager>();
	}

	// Update is called once per frame
	void Update () {
		if(songStarted) {
			// Calculate how much the groove should change, based on the rhythm matching of NPC vs PC rhythm
			var playerScore = AudioLoopGameData.ScorePlayerStringAgainstNPCString(currentSongRhythmString,currentPlayerRhythmString);
			var currentGrooveModifier = grooveDecayRate + grooveIncreaseRate*playerScore;
			currentGroove = Mathf.Clamp(currentGroove + currentGrooveModifier*Time.deltaTime, 0, 1);

			// Update the UI, use a lerp to make it smoother
			grooveBar.value = Mathf.Lerp(grooveBar.value,currentGroove,20*Time.deltaTime);
			characters.UpdateGroove(currentGroove);
		}
	}
}
