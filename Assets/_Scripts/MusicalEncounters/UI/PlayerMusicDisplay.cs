using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMusicDisplay : MonoBehaviour {

	public Animator[] trackAnimators;
	int currentPlayingTrackIndex = -1;

	// Use this for initialization
	void Start () {
		StopPlayingAllTracks();
		DeselectAllTracks();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetTrackPlaying(int trackIndex, bool on) {
		if(trackIndex < 0 || trackIndex >= trackAnimators.Length) {
			Debug.LogError("Error: Track index " + trackIndex + " is out of bounds.");
			return;
		}

		trackAnimators[trackIndex].SetBool("playing",on);
	}

	void SetTrackSelected(int trackIndex, bool on) {
		if(trackIndex < 0 || trackIndex >= trackAnimators.Length) {
			Debug.LogError("Error: Track index " + trackIndex + " is out of bounds.");
			return;
		}

		trackAnimators[trackIndex].SetBool("selected",on);
	}

	void StopPlayingAllTracks() {
		for(var i = 0 ; i < trackAnimators.Length ; i++)
			SetTrackPlaying(i, false);
	}

	void DeselectAllTracks() {
		for(var i = 0 ; i < trackAnimators.Length ; i++)
			SetTrackSelected(i, false);
	}

	public void DidPlayPlayerTrack(int trackIndex) {
		// Set playing to false for all tracks, true for given track
		// Don't mess with selected
		if(trackIndex != currentPlayingTrackIndex) {
			StopPlayingAllTracks();
			DeselectAllTracks();
			SetTrackPlaying(trackIndex, true);
			currentPlayingTrackIndex = trackIndex;
		}
	}

	public void DidStopPlayingTracks() {
		StopPlayingAllTracks();
	}

	public void DidQueuePlayerTrack(int trackIndex) {
		// Set selected to false for all tracks, true for given track
		// Don't mess with playing
		DeselectAllTracks();
		SetTrackSelected(trackIndex, true);
	}
}
