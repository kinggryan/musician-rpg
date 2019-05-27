using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour {

	static GameObject instance = null;

	void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print ("Duplicate Thing Destroyed!");
        }
        else 
        {
            instance = gameObject;
        }

        GameObject.DontDestroyOnLoad(gameObject);
        
    }
}
