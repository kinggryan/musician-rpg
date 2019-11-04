using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoWorldLevelManager : MonoBehaviour {

    static DemoWorldLevelManager instance = null;

    public GameObject colorFadePanel;
    public Animator fadePanel;
    
    private float transitionTime;
    
    private bool timerOn = false;
    public float transitionTimer;
    private string levelToLoad;
    
    private GameObject canvas;
    public PersistentObjectTracker persistentObjectTracker;

    public GameObject transitionPanel;
    private SceneFader sceneFader;
    
    void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print ("Duplicate Spells Destroyed!");
        }
        else 
        {
            instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
        canvas = GameObject.Find("Canvas");
        sceneFader = GetComponent<SceneFader>();
    }

    void Start(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void LoadLevel(string name, float loadTransitionTime)
    {
        Debug.Log("Level load requested for " + levelToLoad + " Time on = " + timerOn + ".");
        if(name != "Overworld"){
//            persistentObjectTracker.UpdatePersistentObjects();
        }
        levelToLoad = name;
        fadePanel.SetTrigger("EndLevel");
        
        //transitionTime = loadTransitionTime;
        
        //timerOn = true;
        
        //FadeTransition(transitionTime);
        
    }

    public void OnFadeComplete(){
        SceneManager.LoadScene(levelToLoad);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log("Scene Loaded");
        fadePanel.SetTrigger("StartLevel");
    }
    
    public void QuitRequest()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }
    
    

    
}
