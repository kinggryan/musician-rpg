using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool waitingForClick = false;
    public int index = 0;
    public bool startEncounterAfterDia;
    public void StartDialogueEvents(){
        StartCoroutine(DisplayDialogue(0));
    }
    IEnumerator DisplayDialogue(int i){
        if(i > dialogueEvents.Length - 1){
            CloseDialogue();
        }else{
            index = i;
            Text text = dialogueController.dialogueText;
            text.text = dialogueEvents[i].dialogue;
            if(dialogueEvents[i].waitForClick){
                Debug.Log("Waiting for click");
                waitingForClick = true;
                index = i + 1;
                yield break;
            }else{
                yield return new WaitForSeconds(dialogueEvents[i].waitTime);
                i++;
                StartCoroutine(DisplayDialogue(i));
            }
        }
    }

    void CloseDialogue(){
        Text text = dialogueController.dialogueText;
        text.text = "";
        if(startEncounterAfterDia){
            NPCMusician npc = GetComponent<NPCMusician>();
            npc.ForceStartEncounter();
            npc.dialogueFirst = false;
        }
    }

    void Start(){
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
