using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour {
	public Color goodColor;
	public Color badColor;
	private Image image;

	void Start(){
		image = GetComponent<Image>();
	}


	public void ScaleScoreBar(float scale){
		this.transform.localScale = new Vector3 (1, scale, 1);
		if (scale >= 0.5){
			image.color = goodColor;
		}else{
			image.color = badColor;
		}
	}

}
