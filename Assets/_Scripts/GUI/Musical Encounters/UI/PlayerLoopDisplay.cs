using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopDisplay : MonoBehaviour {

	public Color[] loopColors;
	public UnityEngine.UI.Image[] loopDisplays;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetLoopNumberForIndex(int loopNumber, int loopIndex) {
		loopDisplays[loopIndex].color = loopColors[loopNumber];
	}
}
