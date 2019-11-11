using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInstrument : MonoBehaviour
{
    public GameObject playerWithInstrument;
    public SpriteRenderer playerWithoutInstrument;
    private PlayerJamMenu playerJamMenu;
    private Slider jammageBar;
    private PlayerMovementController playerMovementController;

    public bool instrumentIsOut = false;

    void Start(){
        GameObject.DontDestroyOnLoad(gameObject);
        playerJamMenu = Object.FindObjectOfType<PlayerJamMenu>();
        jammageBar = Object.FindObjectOfType<Slider>();
        playerMovementController = Object.FindObjectOfType<PlayerMovementController>();
        PutAwayInstrument();
    }
 
    void Update()
    {
        if(Input.GetKeyDown("z")){
            ToggleIfInstrumentIsOut();
        }       
    }

    void ToggleIfInstrumentIsOut(){
        if(instrumentIsOut){
            PutAwayInstrument();
        }else{
            TakeOutInstrument();          
        }
        
    }

    void TakeOutInstrument(){
        ShowJamInterface();
        playerWithoutInstrument.enabled = false;
        playerMovementController.LockMovement(true);
        instrumentIsOut = true;
    }

    void ShowJamInterface(){
        playerWithInstrument.SetActive(true);
        playerJamMenu.gameObject.SetActive(true);
        if(!soloPlay()){
            jammageBar.gameObject.SetActive(true);
        }
    }

    void HideJamInterface(){
        playerWithInstrument.SetActive(false);
        playerJamMenu.gameObject.SetActive(false);
        jammageBar.gameObject.SetActive(false);
    }

    bool soloPlay(){
        return playerJamMenu.jamController.soloPlay;
    }

    void PutAwayInstrument(){
        HideJamInterface();
        playerWithoutInstrument.enabled = true;
        instrumentIsOut = false;
        playerMovementController.LockMovement(false);
        StopPlaying();
    }

    void StopPlaying(){
        if(soloPlay()){
            playerJamMenu.StopSong();
            playerJamMenu.startMuted = true;
        }else{
            playerJamMenu.startMuted = true;
            playerJamMenu.MutePlayer();
            
        }
    }
}
