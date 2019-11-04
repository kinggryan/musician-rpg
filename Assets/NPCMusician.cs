using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMusician : MonoBehaviour
{
    public string songFileName;
    private JamController jamController;
    private GameObject player;
    public bool playerInNPCArea;

    void Start(){
        jamController = Object.FindObjectOfType<JamController>();
        player = GameObject.Find("Player");
    }

    void StartNPCSong(){
        jamController.LoadAndPlaySong(songFileName, true);
    }

    void StopSong(){
        jamController.StopSong();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other == player.GetComponent<Collider2D>()){
            if(!playerInNPCArea){
                jamController.soloPlay = false;
                Debug.Log("Player entered NPC area");
                StartNPCSong();
                playerInNPCArea = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other == player.GetComponent<Collider2D>()){
            if(playerInNPCArea){
                jamController.soloPlay = true;
                Debug.Log("Player left NPC area");
                StopSong();
                playerInNPCArea = false;
                jamController.ResetSong();
            }
        }
    }
}
