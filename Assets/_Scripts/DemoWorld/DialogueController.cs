using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public Text dialogueText;
    private PlayerJamMenu playerJamMenu;
    private JamController jamController;
    public List<string> dialogueQueue;
    public float dialogueWaitTime = 1;

    void Awake(){
        dialogueText = GetComponent<Text>();    
        playerJamMenu = FindObjectOfType<PlayerJamMenu>();
        jamController = FindObjectOfType<JamController>();
        dialogueText.text = "";
        dialogueQueue = new List<string>();
    }
    public void UpdateDialogue(string textToDisplay, float timeToWait){
        timeToWait = dialogueWaitTime;
        Debug.Log("Dialogue: " + textToDisplay);
        dialogueQueue.Add(textToDisplay);
        if(dialogueQueue.Count == 1){
            StartCoroutine(DisplayNextItemInQueue(timeToWait)); 
        }

    }

    IEnumerator DisplayNextItemInQueue(float timeToWait){
        playerJamMenu.gameObject.SetActive(false);
        dialogueText.text = dialogueQueue[0];
        string itemToRemove = dialogueQueue[0];
        yield return new WaitForSeconds(timeToWait);
        dialogueQueue.Remove(itemToRemove);
        dialogueQueue.TrimExcess();
        if(dialogueQueue.Count <= 0){
            //Debug.Log("Dia Queue Empty");
            if(!jamController.ai.isNPCTurn){
                dialogueText.text = "";
                playerJamMenu.gameObject.SetActive(true);
            }
        }else{
            //Debug.Log("Displaying next dia");
            StartCoroutine(DisplayNextItemInQueue(2));
        }
    }
}
