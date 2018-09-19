using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the data about the player's moves
/// </summary>
[System.Serializable]
public class PlayerMove {
	public string loopName;
	// Lazy load the loop
	public AudioLoop loop {
		get {
			if(_loop == null) {
				_loop = AudioLoop.GetLoopForName(loopName);
			}
			return _loop;
		}
	}
	public int staminaCost;
	public int jammageGain;

	private AudioLoop _loop;
}
