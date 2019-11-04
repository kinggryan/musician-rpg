using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplay : MonoBehaviour {

	public UnityEngine.UI.Text beatNumber;

	void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {
		beatNumber.text = "" + beatInfo.currentBeat;
	}
}
