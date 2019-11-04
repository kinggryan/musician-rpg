using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface should be implemented by classes that want to listen to song progress updates - eg the AI.
/// Each object will have to register as a listener on the song structure manager
/// </summary>
public interface ISongUpdateListener {
	void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo);
	void DidFinishSong();
}
