using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectTracker : MonoBehaviour {

	static PersistentObjectTracker instance = null;

	public bool[] isObjectDestroyed;

	public PersistentObject[] persistentObjects;

	public string[] persistentObjectNames;
	void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print ("Duplicate SoundEngine Destroyed!");
        }
        else 
        {
            instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }

	void Start(){
		persistentObjects = FindObjectsOfType<PersistentObject>();
		isObjectDestroyed = new bool[persistentObjects.Length];
		persistentObjectNames= new string[persistentObjects.Length];
		UpdatePersistentObjects();
		InitPersistentObjectNames();
	}

	void InitPersistentObjectNames(){
		int i = 0;
		foreach(PersistentObject persistentObject in persistentObjects){
			persistentObjectNames[i] = persistentObject.gameObject.name;
			i++;
		}
	}

	public void UpdatePersistentObjects(){
//		print("Updating Persistent Objects");
		int i = 0;
		foreach (bool persistentObject in isObjectDestroyed)
    	{
			if (persistentObjects[i] == null){
				isObjectDestroyed[i] = true;

			}
			//print("PO: " + persistentObjects[i]);
			//print("Destroyed?: " + isObjectDestroyed[i]);
			i++   ;
    	}
		
	}

	public void CheckIfObjectDestroyed(){
		print("Checking Persistent Objects");
		int i = 0;
		foreach (bool persistentObject in isObjectDestroyed)
    	{
			persistentObjects[i] = GameObject.Find(persistentObjectNames[i]).GetComponent<PersistentObject>();
			if (isObjectDestroyed[i] && persistentObjects[i] != null){
				Destroy(persistentObjects[i].gameObject);
				print("Persistent Object Destroyed");
				
			}
			//print("PO: " + persistentObjects[i]);
			//print("Destroyed?: " + isObjectDestroyed[i]);
			i++;
    	}
		
	}

	void OnLevelWasLoaded(int level) {
        if (level == 0){
            print("Overworld Loaded");
			CheckIfObjectDestroyed();
		}
        
    }

}
