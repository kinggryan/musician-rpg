using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public delegate void ReturnToLevelCompletedCallback();

	private string sceneToReturnToName = "";
	private Vector3 playerSpawnPosition;
	private ReturnToLevelCompletedCallback callback;	

	// Use this for initialization
	void Awake () {
		Object.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	public void SetOverworldReturnMap(Vector3 playerSpawnPosition, ReturnToLevelCompletedCallback callback) {
		this.playerSpawnPosition = playerSpawnPosition;
		this.sceneToReturnToName = SceneManager.GetActiveScene().name;
		this.callback = callback;
	}

	public void ReturnToLevel() {
		if(sceneToReturnToName == "") {
			Debug.LogError("Tried to reload scene but no scene name was set");
			return;
		}

		// Put the player where they are supposed to be
		SceneManager.LoadScene(sceneToReturnToName);
		var player = Object.FindObjectOfType<PlayerController>();
		player.transform.position = playerSpawnPosition;
		sceneToReturnToName = "";
	}

	public void ReturnToLevelTransitionComplete() {
		if(sceneToReturnToName == SceneManager.GetActiveScene().name && callback != null)
			callback();
	}
}
