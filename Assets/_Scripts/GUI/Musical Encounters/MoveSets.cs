using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Move {
	public string name;
	public Sprite icon;
	public EmotionManager.Emo emo;
	public int power;
	public int Pp;
	public string loopName;
	public string style;
	public string equipKey;
	public string description;
}
[System.Serializable]
public class MoveSet {
	public string name;
	public Move[] moves;
}

public class MoveSets : MonoBehaviour {

	public MoveSet[] moveSets;
	private EmotionManager emoManager;

	// Use this for initialization
	void Start () {
		emoManager = FindObjectOfType<EmotionManager>();
	}
	
}
