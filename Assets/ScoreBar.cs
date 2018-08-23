using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBar : MonoBehaviour {

	private Transform transform;


	public void ScaleScoreBar(float scale){
		transform.localScale = new Vector3 (1, scale, 1);
	}

}
