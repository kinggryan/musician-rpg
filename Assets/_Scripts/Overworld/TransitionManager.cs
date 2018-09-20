using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {

	public Animator transitionAnimator;

	private bool transitionComplete;
	private string sceneName;

	// Use this for initialization
	public void LoadMusicalEncounterScene(string sceneName) {
		// Do the animation
		// Load the scene
		// StartCoroutine(TransitionAsync(sceneName));
		TransitionSync(sceneName);
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

	public void LevelTransitionComplete() {
		transitionComplete = true;
		if(this.sceneName != "") {
			SceneManager.LoadScene(sceneName);
			sceneName = "";
		}
	}
}
