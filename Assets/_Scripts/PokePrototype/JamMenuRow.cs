using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JamMenuRow : MonoBehaviour {

	// 0 is MoveName, 1 is Power, 2 is PP
	public Text[] rowItems;

	// Use this for initialization
	void Awake () {
		rowItems = GetComponentsInChildren<Text>();
	}
}
