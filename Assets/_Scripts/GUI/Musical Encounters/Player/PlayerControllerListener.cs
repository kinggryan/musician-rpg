using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerControllerListener {
	void DidStartSongWithBPM(float bpm);
	void DidChangeLoop(AudioLoop newLoop, int index);
}
