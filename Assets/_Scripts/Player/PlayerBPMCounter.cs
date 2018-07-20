using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBPMCounter : MonoBehaviour {

	public PlayerCountoffDisplay countoffDisplay;
	public List<double> playerBeats;
	private int maxNumPlayerBeats = 8;
	private int minNumPlayerBeats = 4;
	public double bpm;

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pulse")) {
			countoffDisplay.NextBeat();
			
			playerBeats.Add(Time.time);
			if(playerBeats.Count > maxNumPlayerBeats) {
				playerBeats.RemoveAt(0);
			}
			if(playerBeats.Count > minNumPlayerBeats) {
				bpm = CalculateBPM();
				BroadcastMessage("DidChangeBPM",bpm,SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	double CalculateBPM() {
		double averageTimeBetweenBeats = 0;
		for(var i = 1 ; i < playerBeats.Count ; i++) {
			averageTimeBetweenBeats += playerBeats[i] - playerBeats[i-1];
		}

		averageTimeBetweenBeats /= playerBeats.Count-1;
		return 60 / averageTimeBetweenBeats;
	}
}
