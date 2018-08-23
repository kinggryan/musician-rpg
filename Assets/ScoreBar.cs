using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBar : MonoBehaviour {



	public void ScaleScoreBar(float scale){
		this.transform.localScale = new Vector3 (1, scale, 1);
	}

}
