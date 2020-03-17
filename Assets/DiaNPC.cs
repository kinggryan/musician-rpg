using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaNPC : MonoBehaviour
{
    private DialogueEvent dialogue;
    private bool hasSpoken = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = GetComponent<DialogueEvent>();
    }
    void OnTriggerEnter2D(Collider2D other){
        if(!hasSpoken){
            dialogue.StartDialogueEvents();
            hasSpoken = true;
        }
    }
}
