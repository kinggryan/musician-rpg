using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSort : MonoBehaviour {

	private SpriteRenderer renderer;
	private GameObject player;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		float thisHeight = transform.position.y;
		float playerHeight = player.transform.position.y;

		if (thisHeight > playerHeight){
			renderer.sortingLayerName = "Details1.1";
		}else{
			renderer.sortingLayerName = "Foreground Details";
		}
	}
}
