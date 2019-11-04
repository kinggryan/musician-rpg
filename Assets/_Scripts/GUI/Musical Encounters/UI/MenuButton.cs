using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

	public string sceneToLoad;

	// Use this for initialization
	public void LoadScene() {
		SceneManager.LoadScene(sceneToLoad);
	}
}
