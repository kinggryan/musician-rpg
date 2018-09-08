using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBPMCounter : MonoBehaviour {

	private struct CalculateBPMResult {
		public double bpm;
		public bool wasInTime;
	}

	public PlayerCountoffDisplay countoffDisplay;
	public List<double> playerBeats;
	public double bpm;

	private int maxNumPlayerBeats = 8;
	private int minNumPlayerBeats = 4;

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
		// Get a list of all the times between beats so we can do any calculations that we have to do
		var timeBetweenBeats = new List<double>();
		for(var i = 1 ; i < playerBeats.Count ; i++) {
			timeBetweenBeats.Add(playerBeats[i] - playerBeats[i-1]);
		}

		// If there are more than 5 items, remove any outliers
		double averageTimeBetweenBeats = 0;
		double lowerBound = 0;
		double upperBound = Mathf.Infinity;
		if(playerBeats.Count > 5) {
			// // Sort the player beats
			// timeBetweenBeats.Sort();
			// var q1 = timeBetweenBeats[timeBetweenBeats.Count / 4];
			// var q3 = timeBetweenBeats[3*timeBetweenBeats.Count / 4];
			// var iqr = q3-q1;
			// lowerBound = q1-1.5*iqr;
			// upperBound = q3+1.5*iqr;
		}

		foreach(var timeBetweenBeat in timeBetweenBeats) {
			if(timeBetweenBeat >= lowerBound && timeBetweenBeat <= upperBound)
				averageTimeBetweenBeats += timeBetweenBeat;
			else 
				Debug.Log("Removed outlier: " + averageTimeBetweenBeats + " which is outside range " + lowerBound + "," + upperBound);
		}

		averageTimeBetweenBeats /= timeBetweenBeats.Count;
		return 60 / averageTimeBetweenBeats;
	} 
}
