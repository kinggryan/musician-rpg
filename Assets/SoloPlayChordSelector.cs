using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloPlayChordSelector : MonoBehaviour
{
    public Text major;
    public Text minor;
    public Color selectedColor;
    public Color unselectedColor;
    private int index = 0;
    private JamController jamController;
    public GameObject parent;
    private PlayerJamMenu playerJamMenu;
    void Start()
    {
        jamController = Object.FindObjectOfType<JamController>();
        playerJamMenu = Object.FindObjectOfType<PlayerJamMenu>();
        major.color = selectedColor;
        minor.color = unselectedColor;
        playerJamMenu.controlsDisabled = true;   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("up")){
            Up();    
        }
        if(Input.GetKeyDown("down")){
            Down();    
        }
        if(Input.GetKeyDown("return")){
            Select();
        }
    }

    void Select(){
        if(index == 0){
            jamController.songFileName = "Songs/MajorChord";
        }else if (index == 1){
            jamController.songFileName = "Songs/MinorChord";
        }
        jamController.OnStart();
        StartCoroutine(DisableParent());
    }

    IEnumerator DisableParent(){
        yield return new WaitForSeconds (0.1f);
        playerJamMenu.controlsDisabled = false;
        parent.SetActive(false);
    }

    void Up(){
        if(index == 1){
            index = 0;
            major.color = selectedColor;
            minor.color = unselectedColor; 
        }
    }

    void Down(){
        if(index == 0){
            index = 1;
            minor.color = selectedColor;
            major.color = unselectedColor; 
        }
    }
}
