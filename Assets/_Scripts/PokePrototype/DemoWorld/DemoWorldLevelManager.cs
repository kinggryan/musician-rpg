using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoWorldLevelManager : MonoBehaviour {

    static DemoWorldLevelManager instance = null;

    public GameObject colorFadePanel;
    
    private float transitionTime;
    
    private bool timerOn = false;
    public float transitionTimer;
    private string levelToLoad;
    
    private GameObject canvas;
    public PersistentObjectTracker persistentObjectTracker;

    public GameObject transitionPanel;
    
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
    }
    
    public void LoadLevel(string name, float loadTransitionTime)
    {
        if(name != "Overworld"){
            persistentObjectTracker.UpdatePersistentObjects();
        }
        transitionTime = loadTransitionTime;
        levelToLoad = name;
        timerOn = true;
        Debug.Log("Level load requested for " + levelToLoad + " Time on = " + timerOn + ".");
        //FadeTransition(transitionTime);
        
    }


    
    public void QuitRequest()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }
    
    void FadeTransition (float fadeTime)
    {
        Debug.Log("Fade Trans Started");
        GameObject newPanel = Instantiate(colorFadePanel, canvas.transform.position, canvas.transform.rotation) as GameObject;
        GameObject transitionPanel = newPanel;
        print("Transition panel = " + transitionPanel);
        //GameObject.DontDestroyOnLoad(transitionPanel);
        transitionPanel.GetComponent<FadeFromColor>().fadeIn = false;
        //transitionPanel.GetComponent<FadeFromColor>().currentColor.a = 0;
        transitionPanel.GetComponent<FadeFromColor>().fadeTime = fadeTime;
        
        transitionPanel.transform.SetParent(canvas.transform, false);
        
    }
    
    void Update ()
    {
        if (timerOn)
        {
            transitionTimer += Time.deltaTime;
        }
        
        if (transitionTimer > transitionTime)
        {
            timerOn = false;
            transitionTimer = 0f;
            Application.LoadLevel(levelToLoad);
            if (!transitionPanel.GetComponent<FadeFromColor>().fadeIn  && transitionPanel != null){
                timerOn = true;
                transitionPanel.GetComponent<FadeFromColor>().fadeIn = true;
                transitionPanel.GetComponent<FadeFromColor>().timeSinceAwake = 0;
            }
            

        }
    }
    
    //UI LOAD BUTTON
    
    public void ButtonLoadTime (float loadTransitionTime)
    {
        transitionTime = loadTransitionTime;
    }
    
    public void ButtonLoadLevel (string name)
    {
        levelToLoad = name;
        timerOn = true;
        Debug.Log("Level load requested for " + levelToLoad + " Time on = " + timerOn + ".");
        FadeTransition(transitionTime);
    }
    

    
}
