using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using MusicianRPG;

public class AIMIDIController : MonoBehaviour {

	// TODO : This may get moved around
	public string midiFilePath = "Midis/Groove.mid";

	public float volumeFollow;
	public float volumeFollowRndm;

	public float gateFollowTime;
	public float gateFollowRndm;
	public float moveMinInterval;
	public float moveMaxInterval;
	public float leadInterval;
	public float playerVolume;
	public Color gateTextColor;
	public Color dynamicsTextColor;
	public Color moveColor;
	

	public int trackGateVelocity {
        get { return gate.gateVelocity; }
        set { 
            gate.gateVelocity = value; 
			gateVal = value;
        }
    }
    public float volume {
        get { return volumeFilter.volumeMultiplier; }
        set {
            volumeFilter.volumeMultiplier = value;
			currentVolume = value;
        }
    }
	public int bufferSize = 1024;

	public float currentVolume;
	public int gateVal;
    //Private 
    
	public MIDISongPlayer midiPlayer;
	public AIFeedback aiFeedback;
	
	public bool isLeading;
	
	public bool mute;
	private float moveInterval;
	private float moveTimer;
	private bool moving;
	private float targetVolume;

	private MidiStreamer midiStreamer;
    private MIDITrackGate gate;
    private MIDIVolumeFilter volumeFilter;
	private bool dynMatchBuffer;
	
	
	private float volumeTimer;
	private float leadTimer;
	public float dynamicsMatchTimer;
	
    private bool isPlaying;

	private int currentPlayerGate;
	private float gateMatchTimer;
	private bool gateMatchBuffer;

    void Start()
    {
		// Add yourself to the midi player
		midiStreamer = midiPlayer.CreateNewMidiFileStreamer(new List<string>(){midiFilePath});

		volumeFilter = new MIDIVolumeFilter();
		midiStreamer.AddFilter(volumeFilter);
		volumeFilter.activeChannel = 1;

		gate = new MIDITrackGate();
		midiStreamer.AddFilter(gate);
		gate.activeChannel = 1;

		moveInterval = Random.Range(moveMinInterval, moveMaxInterval);

        trackGateVelocity = 79;
        volume = 1;
	}

	public void playerGateChange(int playerGate){
		currentPlayerGate = playerGate;
		int newGateVelo;
		if (playerGate == 80){
			newGateVelo = 78;
		}else if (playerGate == 79){
			newGateVelo = 79;
		}else {
			newGateVelo = 80;
		}
		StartCoroutine(WaitThenChangeGate(newGateVelo, gateFollowTime + Random.Range(-1 * (gateFollowRndm / 2), gateFollowRndm / 2)));
	}

	private IEnumerator WaitThenChangeGate(int newGateVelo, float timeToWait){
		yield return new WaitForSeconds(timeToWait);
		trackGateVelocity = newGateVelo;

	}

	void MakeVolumeMove(){
		moveTimer = 0;
		moveInterval = Random.Range(moveMinInterval, moveMaxInterval);
		targetVolume = Random.Range(.2f,2);
		Debug.Log("Making vol move from " + volume + " to " + targetVolume);
		moving = true;
	}

	void MakeGateMove(){
		int newGateVelo = Random.Range(78, 81);
		trackGateVelocity = newGateVelo;
	}

	void MakeMove(){
		aiFeedback.ChangeAvatarColorForDuration(moveColor, 1);
		MakeVolumeMove();
		MakeGateMove();
	}

	void SwitchLead(){
		gateMatchTimer = 0;
		dynamicsMatchTimer = 0;
		leadTimer = 0;
		if (isLeading){
			aiFeedback.DisplayText("Your Turn!", 2, Color.white);
			isLeading = false;
		}else{
			aiFeedback.DisplayText("I'll lead now!", 2, Color.white);
			isLeading = true;
		}
	}

	bool gatesMatched (){
		if (currentPlayerGate == 78 && trackGateVelocity == 80){
			return true;
		}else if (currentPlayerGate == 79 && trackGateVelocity == 79){
			return true;
		}else if (currentPlayerGate == 80 && trackGateVelocity == 78){
			return true;
		}else{
			return false;
		}
	}

	bool dynamicsMatched(){
		if (Mathf.Abs(volume - playerVolume) <= 0.1f){
			return true;
		}else{
			return false;
		}
	}

	//I was trying to make all the junk in the update neater by using this funciton but couldn't get it to work for some reason
	void CheckIfMatchedAndGiveFeedBack(bool valuesAreMatched, float feedbackTimer, bool bufferBool, string textToDislplay){
		if(valuesAreMatched && !bufferBool){
		feedbackTimer += Time.deltaTime;
			if (feedbackTimer >= 2){
				bufferBool = true;
				Debug.Log("Values Matched");
				aiFeedback.DisplayText(textToDislplay, 1, Color.white);
			}else if(!valuesAreMatched){
				bufferBool = false;
				feedbackTimer = 0;
			}else{
				feedbackTimer = 0;
			}
		}

		
	}


	void Update(){
		leadTimer += Time.deltaTime;
		if (leadTimer >= leadInterval){
			SwitchLead();
		}

		if (mute){
            volumeFilter.volumeMultiplier = 0;
        }
		if (isLeading){
			
			if(dynamicsMatched() && !dynMatchBuffer){
				dynamicsMatchTimer += Time.deltaTime;
				if (dynamicsMatchTimer >= 2){
					dynMatchBuffer = true;
					Debug.Log("Dynamics Matched");
					aiFeedback.DisplayText("Tasty!", 1, dynamicsTextColor);
				}
			}else if(!dynamicsMatched()){
				dynMatchBuffer = false;
				dynamicsMatchTimer = 0;
			}else{
				dynamicsMatchTimer = 0;
			}

			if(gatesMatched() && !gateMatchBuffer){
				gateMatchTimer += Time.deltaTime;
				if (gateMatchTimer >= 2){
					gateMatchBuffer = true;
					Debug.Log("Gates Matched");
					aiFeedback.DisplayText("Nice!", 1, gateTextColor);
				}
			}else if(!gatesMatched()){
				gateMatchBuffer = false;
				gateMatchTimer = 0;
			}else{
				gateMatchTimer = 0;
			}

			moveTimer += Time.deltaTime;
			if (moving){
				volume = Mathf.Lerp(volume, targetVolume, moveTimer);
				if (moveTimer >= 1){
					moveTimer = 0;
					moving = false;
				}
			}else if (moveTimer >= moveInterval && !moving){
				MakeMove();
			}
		}else if(!isLeading){
			if(dynamicsMatched() && !dynMatchBuffer){
				dynamicsMatchTimer += Time.deltaTime;
				if (dynamicsMatchTimer >= 2){
					dynMatchBuffer = true;
					Debug.Log("Dynamics Matched");
					aiFeedback.DisplayText("Cool!", 1, dynamicsTextColor);
				}
			}else if(!dynamicsMatched()){
				dynMatchBuffer = false;
				dynamicsMatchTimer = 0;
			}else{
				dynamicsMatchTimer = 0;
			}

			if(gatesMatched() && !gateMatchBuffer){
				gateMatchTimer += Time.deltaTime;
				if (gateMatchTimer >= 2){
					gateMatchBuffer = true;
					Debug.Log("Gates Matched");
					aiFeedback.DisplayText("Sick!", 1, gateTextColor);
				}
			}else if(!gatesMatched()){
				gateMatchBuffer = false;
				gateMatchTimer = 0;
			}else{
				gateMatchTimer = 0;
			}	
			if (volume != playerVolume){
				float random = Random.Range(0, volumeFollowRndm);
				volumeTimer += Time.deltaTime / (volumeFollow + random);
				volume = Mathf.Lerp(volume, playerVolume, volumeTimer);
			}else{
				volumeTimer = 0;
			}
		}
	}

}
