using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCMusician : MonoBehaviour
{
    public string songFileName;
    public bool dialogueFirst = false;
    private JamController jamController;
    private GameObject player;
    public bool playerInNPCArea;
    private bool checkingForInstrument = false;
    private ToggleInstrument toggleInstrument;
    private bool inEncounter = false;
    private DialogueController dialogue;
    [SerializeField]
    private int instrumentIndex;
    
    private int midiChannel = 1;
    private float encounterTimer = 0;
    [SerializeField]
    private float encounterLength = 5;

    private SongSelector songSelector;
    [SerializeField]
    private string winDialogue;
    [SerializeField]
    private string overDialogue;
    [SerializeField]
    private string underDialogue;
    public UnityEvent runOnWin;
    public UnityEvent runOnLose;
    
    private float dialogueTime = 4;
    private AIJamController aiJamController;
    private MoveSets moveSets;
    public float volume;
    public float maxAudibleDistance=20;
    private float minAudibleDistance=0;
    private float distance;
    bool playerInAudibleRange = false;
    bool win = false;
    private Animator animator;

    void Start(){
        jamController = Object.FindObjectOfType<JamController>();
        toggleInstrument = Object.FindObjectOfType<ToggleInstrument>();
        player = GameObject.Find("Player");
        dialogue = Object.FindObjectOfType<DialogueController>();
        songSelector = Object.FindObjectOfType<SongSelector>();
        aiJamController = Object.FindObjectOfType<AIJamController>();
        animator = GetComponent<Animator>();
        moveSets = GetComponent<MoveSets>();
    }

    void Update(){
        distance = Vector2.Distance(transform.position, player.transform.position);
        volume = Mathf.SmoothStep(0, 1, Mathf.Clamp((maxAudibleDistance-(distance-minAudibleDistance))/maxAudibleDistance, 0, 1));
        aiJamController.aiMidiController.volume = volume;
        if(!playerInAudibleRange && volume > 0){
            OnPlayerEnterAudibleRange();
        }else if (playerInAudibleRange && volume <= 0){
            OnPlayerExitAudibleRange();
        }
        if(checkingForInstrument){
            if(toggleInstrument.instrumentIsOut){
                StartEncounter();
            }
        }else if(inEncounter){
            if(!jamController.inEncounter){
                EndEncounter();
            }else if (encounterTimer >= encounterLength){
                win = true;
                EndEncounter();
            }else{
                encounterTimer += Time.deltaTime;
            }
        }
        
    }

    void StartNPCSong(){
        StopSong();
        aiJamController.aiMidiController.midiPlayer.midiSequencer.setProgram(midiChannel, instrumentIndex);
        jamController.LoadAndPlaySong("Songs/" + songFileName, true);
        jamController.isPlaying = true;
        animator.SetBool("Playing", true);
    }

    public void StartEncounter(){
        checkingForInstrument = false;
        jamController.inEncounter = true;
        jamController.scoreManager.UpdateScores();
        inEncounter = true;
        Debug.Log("Starting Encounter");

    }


    void EndEncounter(){
        inEncounter = false;
        Debug.Log("Encounter OVER");
        //StartCoroutine(WaitThenHideJamDisplay());
        toggleInstrument.HideDisplay();
        if(win){
            WinEncounter();
            
        }else{
            LoseEncounter();
        }
        jamController.isPlaying = false;
        jamController.soloPlay = true;
        jamController.player.UnlockPlayerControls();
    }

    void LoseEncounter()
    {
        animator.SetBool("Playing", false);
        if(runOnLose != null){
            runOnLose.Invoke();
        }
    }

    void WinEncounter(){
        //dialogue.UpdateDialogue(winDialogue, dialogueTime);
        toggleInstrument.PutAwayInstrument();
        animator.SetBool("Playing", false);
        StopSong();
        songSelector.AddSongToInventory(songFileName);
        Reset();
        if(runOnWin != null){
            runOnWin.Invoke();
        }
    }

    void Reset(){
        jamController.ResetSong();
        encounterTimer = 0;
    }

    IEnumerator WaitThenHideJamDisplay(){
        yield return new WaitForSeconds(dialogueTime);
        toggleInstrument.HideDisplay();
    }
    void StopSong(){
        jamController.StopSong();
        jamController.isPlaying = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other == player.GetComponent<Collider2D>()){
            if(!playerInNPCArea){
                jamController.soloPlay = false;
                Debug.Log("Player entered NPC area");
                playerInNPCArea = true;
                toggleInstrument.playerInEncounterArea = true;
                if(!dialogueFirst){
                    checkingForInstrument = true;
                }else{
                    GetComponent<DialogueEvent>().StartDialogueEvents();
                }
            }
        }
    }

    public void ForceStartEncounter(){
        toggleInstrument.TakeOutInstrument();
        StartEncounter();
    }

    void OnPlayerEnterAudibleRange(){
        playerInAudibleRange = true;
        aiJamController.characterJamController.moveSets = moveSets;
        aiJamController.characterJamController.noteParticles = GetComponent<ParticleSystem>();
        jamController.ai.SetFirstMove();
        StartNPCSong();
    }
    void OnPlayerExitAudibleRange(){
        playerInAudibleRange = false;
        jamController.ResetSong();
        jamController.soloPlay = true;
        jamController.ResetSong();
        StopSong();
    }

    void OnTriggerExit2D(Collider2D other){
        if(other == player.GetComponent<Collider2D>()){
            if(playerInNPCArea){
                jamController.soloPlay = true;
                Debug.Log("Player left NPC area");
                playerInNPCArea = false;
                toggleInstrument.playerInEncounterArea = false;
                jamController.ResetSong();
            }
        }
    }
}
