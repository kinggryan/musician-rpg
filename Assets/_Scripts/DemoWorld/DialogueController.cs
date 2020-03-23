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
    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color notificationColor;

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
            StartCoroutine(DisplayNextItemInQueue(timeToWait, defaultColor)); 
        }

    }

    public void UpdateNotification(string textToDisplay, float timeToWait){
        timeToWait = dialogueWaitTime;
        Debug.Log("Dialogue: " + textToDisplay);
        dialogueQueue.Add(textToDisplay);
        if(dialogueQueue.Count == 1){
            StartCoroutine(DisplayNextItemInQueue(timeToWait, notificationColor)); 
        }

    }

    IEnumerator DisplayNextItemInQueue(float timeToWait,Color color){
        playerJamMenu.gameObject.SetActive(false);
        dialogueText.text = dialogueQueue[0];
        dialogueText.color = color;
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
            StartCoroutine(DisplayNextItemInQueue(2, color));
        }
    }
}
