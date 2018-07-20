using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBPMCounter : MonoBehaviour {

	public List<double> playerBeats;
	private int numPlayerBeats = 8;
	public double bpm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pulse")) {
			playerBeats.Add(Time.time);
			if(playerBeats.Count > numPlayerBeats) {
				playerBeats.RemoveAt(0);
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
