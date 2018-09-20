using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {

	public Animator transitionAnimator;

	private bool transitionComplete;
	private string sceneName;
	private bool returnToOverworld = false;

	// Use this for initialization
	public void LoadMusicalEncounterScene(string sceneName) {
		// Do the animation
		// Load the scene
		// StartCoroutine(TransitionAsync(sceneName));
		TransitionSync(sceneName);
	}

	public void ReturnToOverworld() {
		TransitionSyncToOverworld();
	}

	IEnumerator TransitionAsync(string sceneName) {
		transitionAnimator.SetTrigger("EndLevel");
		transitionComplete = false;
		var loadOp = SceneManager.LoadSceneAsync(sceneName);
		while(!loadOp.isDone && !transitionComplete) 
			yield return null;
	}

	void TransitionSync(string sceneName) {
		this.sceneName = sceneName;
		transitionAnimator.SetTrigger("EndLevel");
	}

	void TransitionSyncToOverworld() {
		returnToOverworld = true;
		transitionAnimator.SetTrigger("EndLevel");
	}

	public void LevelTransitionComplete() {
		transitionComplete = true;
		if(returnToOverworld) {
			returnToOverworld = false;
			var levelManager = Object.FindObjectOfType<LevelManager>();
			levelManager.ReturnToLevel();
		} else if(sceneName != "") {
			SceneManager.LoadScene(sceneName);
			sceneName = "";
		} else {
			Debug.LogError("There was a problem with the scene name....don't know where to return to");
		}
	}
}
