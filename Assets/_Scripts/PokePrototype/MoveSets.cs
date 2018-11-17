using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move {
	public string name;
	public int emo;
	public int power;
	public int Pp;
}
[System.Serializable]
public class MoveSet {
	public string name;
	public Move[] moves;
}

public class MoveSets : MonoBehaviour {

	public MoveSet[] moveSets;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
