using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInstrument : MonoBehaviour
{
    public GameObject playerWithInstrument;
    public SpriteRenderer playerWithoutInstrument;
    private PlayerJamMenu playerJamMenu;
    private Slider[] jamageBars;
    private PlayerMovementController playerMovementController;
    private NumbericJamInterface jamInterface;
    public bool playerInEncounterArea = false;
    public AnimatorSynchroniser animator;

    public bool instrumentIsOut = false;

    void Start(){
        GameObject.DontDestroyOnLoad(gameObject);
        playerJamMenu = Object.FindObjectOfType<PlayerJamMenu>();
        jamageBars = Object.FindObjectsOfType<Slider>();
        playerMovementController = Object.FindObjectOfType<PlayerMovementController>();
        jamInterface = Object.FindObjectOfType<NumbericJamInterface>();
        PutAwayInstrument();
    }
 
    void Update()
    {
        if(Input.GetKeyDown("z") && playerMovementController.hasInstrument == true){
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

    public void TakeOutInstrument(){
        ShowJamInterface();
        //playerWithoutInstrument.enabled = false;
        playerMovementController.LockMovement(true);
        animator.SetBool ("InstrumentOut", true);
        animator.SetInteger ("direction", 3);
        instrumentIsOut = true;
        jamInterface.locked = false;
    }

    void ShowJamInterface(){
        //playerWithInstrument.SetActive(true);
        playerJamMenu.gameObject.SetActive(true);
        if(!soloPlay() && playerInEncounterArea){
            ShowJamageBars();
        }
    }

    void ShowJamageBars(){
        foreach(Slider jamageBar in jamageBars){
            jamageBar.gameObject.SetActive(true);
        }
    }

    void HideJamageBars(){
        foreach(Slider jamageBar in jamageBars){
            jamageBar.gameObject.SetActive(false);
        }
    }

    void HideJamInterface(){
        //playerWithInstrument.SetActive(false);
        animator.SetBool ("InstrumentOut", false);
        animator.SetInteger ("direction", 3);
        //playerJamMenu.gameObject.SetActive(false);
        HideJamageBars();
    }

    public void HideInstrument(){
        //playerWithInstrument.SetActive(false);
        animator.SetBool ("InstrumentOut", false);
        animator.SetInteger ("direction", 3);
        //playerWithoutInstrument.enabled = true;
        instrumentIsOut = false;
        playerMovementController.LockMovement(false);
        jamInterface.locked = true;
    }

    public void HideDisplay(){
        HideJamageBars();
    }

    bool soloPlay(){
        return playerJamMenu.jamController.soloPlay;
    }

    public void PutAwayInstrument(){
        HideJamInterface();
        //playerWithoutInstrument.enabled = true;
        animator.SetBool ("InstrumentOut", false);
        animator.SetInteger ("direction", 3);
        instrumentIsOut = false;
        playerMovementController.LockMovement(false);
        playerJamMenu.jamController.inEncounter = false;
        StopPlaying();
        jamInterface.locked = true;
    }

    void StopPlaying(){
        if(soloPlay()){
            playerJamMenu.StopSong();
            playerJamMenu.startMuted = true;
        }else{
            playerJamMenu.startMuted = true;
            playerJamMenu.MutePlayer();
            
        }
        animator.SetBool("playing", false);
    }
}
