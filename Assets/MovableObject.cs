using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Sequencer;

public class MovableObject : MonoBehaviour
{
    private PlayerJamMenu playerJamMenu;
    Rigidbody2D rbody;
    bool isListening = false;
    [SerializeField]
    string moveSong;
    [SerializeField]
    string leftPattern;
    [SerializeField]
    string rightPattern;
    [SerializeField]
    string upPattern;
    [SerializeField]
    string downPattern;
    [SerializeField]
    private float movementSpeed = 2;
    GameObject player;
    private Animator animator;
    private bool levitating = false;

    void Start()
    {
        playerJamMenu = Object.FindObjectOfType<PlayerJamMenu>();
        player = GameObject.Find("Player");
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isListening){
            bool isPlaying = playerJamMenu.jamController.isPlaying;
            string activeSong = playerJamMenu.soloSong;
            if("Songs/" + moveSong == activeSong && isPlaying){
                string activeMove = playerJamMenu.getActiveMoveName();
                animator.SetBool("Levitating",true);
                //levitating = false;
                if(activeMove == leftPattern){
                    MoveLeft();
                }else if(activeMove == rightPattern){
                    MoveRight();
                }else if(activeMove == upPattern){
                    MoveUp();
                }else if(activeMove == downPattern){
                    MoveDown();
                }else{
                    StopMovement();
                }
            }else{
                StopMovement();
                animator.SetBool("Levitating",false);
            }
        }
    }

    public void OnListeningAreaEnter(Collider2D other){
        if(other == player.GetComponent<Collider2D>()){
            if(!isListening){
                isListening = true;
            }
        }
    }

    public void OnListeningAreaExit(Collider2D other){
        if(other == player.GetComponent<Collider2D>()){
            if(isListening){
                isListening = false;
                StopMovement();
                animator.SetBool("Levitating",false);
            }
        }
    }

    void StopMovement(){
        Debug.Log("Stopping");
        rbody.velocity = new Vector2(0, 0);
    }

    void MoveLeft(){
        Debug.Log("Moving Left");
        rbody.velocity = new Vector2(movementSpeed * -1, 0);
    }

    void MoveRight(){
        Debug.Log("Moving Right");
        rbody.velocity = new Vector2(movementSpeed, 0);
    }

    void MoveUp(){
        Debug.Log("Moving UP");
        rbody.velocity = new Vector2(0, movementSpeed);
    }

    void MoveDown(){
        Debug.Log("Moving Down");
        rbody.velocity = new Vector2(0, movementSpeed * -1);
    }
}
