using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMusicDisplay : MonoBehaviour {

	public UnityEngine.UI.Image[] trackHighlightImages;

	// Use this for initialization
	void Start () {
		TurnOffAllTracks();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetTrackOn(int trackIndex, bool on) {
		if(trackIndex < 0 || trackIndex >= trackHighlightImages.Length) {
			Debug.LogError("Error: Track index " + trackIndex + " is out of bounds.");
			return;
		}

		trackHighlightImages[trackIndex].enabled = on;
	}

	void TurnOffAllTracks() {
		for(var i = 0 ; i < trackHighlightImages.Length ; i++)
			SetTrackOn(i, false);
	}

	public void DidPlayPlayerTrack(int trackIndex) {
		TurnOffAllTracks();
		SetTrackOn(trackIndex, true);
	}
}
