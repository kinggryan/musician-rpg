using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class KeyControlValue : MonoBehaviour {

	public bool isActive;
	public int index;
	public Color color;
	public Animator animator;
	public Animation animation;
	
	void Start(){
		animator = GetComponent<Animator>();
		animation = GetComponent<Animation>();
	}

	public void OnBeatPlayed(float bpm){
		if(isActive){
			
			animator.enabled = true;
		}else {
			animator.enabled = false;
		}
	}
}
