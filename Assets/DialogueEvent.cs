using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueEvent : MonoBehaviour
{
    public DiaInstance[] dialogueEvents;
    [System.Serializable]
    public class DiaInstance {
        public string dialogue;
        public float waitTime;
        public bool waitForClick;
    }
    private DialogueController dialogueController;
    private PlayerMovementController playerMovementController;
    private bool waitingForClick = false;
    public int index = 0;
    public bool startEncounterAfterDia;
    public UnityEvent runOnClose;
    [SerializeField]
    private float bufferTime;
    public bool dialogueInProgress = false;
    public void StartDialogueEvents(){
        dialogueInProgress = true;
        playerMovementController.LockMovement(true);
        StartCoroutine(DisplayDialogue(0));
    }
    private float textSpeed = 0.05f;
    IEnumerator DisplayDialogue(int i){
        if(i > dialogueEvents.Length - 1){
            CloseDialogue();
        }else{
            index = i;
            // Text text = dialogueController.dialogueText;
            // text.text = dialogueEvents[i].dialogue;
            StartCoroutine(AnimateText(i, textSpeed));
            // if(dialogueEvents[i].waitForClick){
            //     Debug.Log("Waiting for click");
            //     StartCoroutine(WaitForClickBuffer(bufferTime));
            //     index = i + 1;
            //     yield break;
            // }else{
            //     yield return new WaitForSeconds(dialogueEvents[i].waitTime);
            //     i++;
            //     StartCoroutine(DisplayDialogue(i));
            // }
        }
        yield break;
    }

    IEnumerator AnimateText(int i, float speed){
        string dialogue = dialogueEvents[i].dialogue;
        Text text = dialogueController.dialogueText;
        for (int charIndex = 0; charIndex <= dialogue.Length; charIndex++){
            // if(Input.anyKeyDown){
            //     text.text = dialogue;
            //     StartCoroutine(AdvanceDialogue(i));
            //     yield break;
            // }
            text.text = dialogue.Substring(0, charIndex);
            yield return new WaitForSeconds(speed);
        }
        StartCoroutine(AdvanceDialogue(i));
    }

    IEnumerator AdvanceDialogue(int i){
        if(dialogueEvents[i].waitForClick){
            Debug.Log("Waiting for click");
            StartCoroutine(WaitForClickBuffer(bufferTime));
            index = i + 1;
            yield break;
        }else{
            yield return new WaitForSeconds(dialogueEvents[i].waitTime);
            i++;
            StartCoroutine(DisplayDialogue(i));
        }
    } 

    IEnumerator WaitForClickBuffer(float timeToWait){
        yield return new WaitForSeconds(timeToWait);
        waitingForClick = true;
    }

    void CloseDialogue(){
        Text text = dialogueController.dialogueText;
        text.text = "";
        if(startEncounterAfterDia){
            NPCMusician npc = GetComponent<NPCMusician>();
            npc.ForceStartEncounter();
            npc.dialogueFirst = false;
        }
        if(runOnClose != null){
            runOnClose.Invoke();
        }
        playerMovementController.LockMovement(false);
        StartCoroutine(DialogueNoLongerInProgrress());
    }

    IEnumerator DialogueNoLongerInProgrress(){
        yield return new WaitForSeconds(1);
        dialogueInProgress = false;
    }

    void Start(){
        playerMovementController = Object.FindObjectOfType<PlayerMovementController>();
        dialogueController = Object.FindObjectOfType<DialogueController>();
    }

    

    void Update(){
        if(Input.anyKeyDown){
            OnClick();
        }
    }

    void OnClick(){
        if(waitingForClick){
            Debug.Log("Advancing Dia cuz click");
            waitingForClick = false;
            StartCoroutine(DisplayDialogue(index));
        }
    }
}
