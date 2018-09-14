using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour {

	public string menuName;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("backspace")) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene(menuName);
		}
	}
}
