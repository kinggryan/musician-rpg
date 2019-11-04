using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInstrument : MonoBehaviour
{
    public GameObject playerWithInstrument;
    public SpriteRenderer playerWithoutInstrument;
    private PlayerJamMenu playerJamMenu;
    private PlayerMovementController playerMovementController;

    public bool instrumentIsOut = false;

    void Start(){
        GameObject.DontDestroyOnLoad(gameObject);
        playerJamMenu = Object.FindObjectOfType<PlayerJamMenu>();
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
        playerWithInstrument.SetActive(true);
        playerJamMenu.gameObject.SetActive(true);
        playerWithoutInstrument.enabled = false;
        playerMovementController.LockMovement(true);
        instrumentIsOut = true;
    }

    bool soloPlay(){
        return playerJamMenu.jamController.soloPlay;
    }

    void PutAwayInstrument(){
        playerWithInstrument.SetActive(false);
        playerJamMenu.gameObject.SetActive(false);
        playerWithoutInstrument.enabled = true;
        instrumentIsOut = false;
        playerMovementController.LockMovement(false);
        if(soloPlay()){
            playerJamMenu.StopSong();
            playerJamMenu.startMuted = true;
        }else{
            playerJamMenu.startMuted = true;
            playerJamMenu.MutePlayer();
            
        }

    }
}
