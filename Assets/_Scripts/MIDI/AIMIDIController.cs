using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;

public class AIMIDIController : MonoBehaviour {


	public float volumeFollow;
	public float volumeFollowRndm;	
	public float moveMinInterval;
	public float moveMaxInterval;
	public float leadInterval;
	public float playerVolume;
	

	public int trackGateVelocity {
        get { return gate.gateVelocity; }
        set { 
            gate.gateVelocity = value; 
            midiSequencer.ApplyMidiFilterToTracks(filterGroup);
        }
    }
    public float volume {
        get { return volumeFilter.volumeMultiplier; }
        set {
            volumeFilter.volumeMultiplier = value;
            midiSequencer.ApplyMidiFilterToTracks(filterGroup);
			currentVolume = value;
        }
    }
	public int bufferSize = 1024;

	public float currentVolume;
    //Private 
    
	public MIDIPlayer midiPlayer;
	public AIFeedback aiFeedback;
	
	public bool isLeading;
	
	public bool mute;
	private float moveInterval;
	private float moveTimer;
	private bool moving;
	private float targetVolume;
	private float[] sampleBuffer;

	private MidiSequencer midiSequencer;
    private StreamSynthesizer midiStreamSynthesizer;

	private MIDIFilterGroup filterGroup;
    private MIDITrackGate gate = new MIDITrackGate();
    private MIDIVolumeFilter volumeFilter;
	private bool matched;
	
	
	private float volumeTimer;
	private float leadTimer;
	public float dynamicsMatchTimer;
	
    private bool isPlaying;

    void Start()
    {
        midiStreamSynthesizer = midiPlayer.midiStreamSynthesizer;
		volumeFilter = midiPlayer.opponentVolumeFilter;
        sampleBuffer = new float[midiStreamSynthesizer.BufferSize];

		moveInterval = Random.Range(moveMinInterval, moveMaxInterval);

		//aiFeedback = GetComponent<AIFeedback>();

        midiSequencer = midiPlayer.midiSequencer;
        volumeFilter.activeChannel = 1;
		gate.activeChannel = 1;

        filterGroup = midiPlayer.filterGroup;
        trackGateVelocity = 79;
        volume = 1;
	}

	void MakeVolumeMove(){
		moveTimer = 0;
		moveInterval = Random.Range(moveMinInterval, moveMaxInterval);
		targetVolume = Random.RandomRange(.2f,2);
		Debug.Log("Making vol move from " + volume + " to " + targetVolume);
		moving = true;
	}

	void SwitchLead(){
		leadTimer = 0;
		if (isLeading){
			aiFeedback.DisplayText("Your Turn!", 2);
			isLeading = false;
		}else{
			aiFeedback.DisplayText("I'll lead now!", 2);
			isLeading = true;
		}
	}

	void Update(){
		//Debug.Log("volume difference" + Mathf.Abs(volume - playerVolume));
		leadTimer += Time.deltaTime;
		if (leadTimer >= leadInterval){
			SwitchLead();
		}
		
		if(midiSequencer == null){
			Debug.LogError("AI MIDI Sequencer missing!");
		}
		if(midiStreamSynthesizer == null){
			Debug.LogError("AI Stream Synth missing!");
		}

		if (mute){
            volumeFilter.volumeMultiplier = 0;
            midiSequencer.ApplyMidiFilterToTracks(filterGroup);
        }
		if (isLeading){
			if(Mathf.Abs(volume - playerVolume) <= 0.1f && !matched){
				dynamicsMatchTimer += Time.deltaTime;
				if (dynamicsMatchTimer >= 2){
					matched = true;
					Debug.Log("Dynamics Matched");
					aiFeedback.DisplayText("Tasty!", 1);
				}
			}else if(Mathf.Abs(volume - playerVolume) > 0.1f){
				matched = false;
				dynamicsMatchTimer = 0;
			}else{
				dynamicsMatchTimer = 0;
			}

			moveTimer += Time.deltaTime;
			if (moving){
				volume = Mathf.Lerp(volume, targetVolume, moveTimer);
				if (moveTimer >= 1){
					moveTimer = 0;
					moving = false;
				}
			}else if (moveTimer >= moveInterval && !moving){
				MakeVolumeMove();
			}
		}else if (volume != playerVolume && !isLeading){
			float random = Random.Range(0, volumeFollowRndm);
			volumeTimer += Time.deltaTime / (volumeFollow + random);
			volume = Mathf.Lerp(volume, playerVolume, volumeTimer);
			//volumeFilter.volumeMultiplier = volume;
			//midiSequencer.ApplyMidiFilterToTracks(filterGroup);
		}else{
			volumeTimer = 0;
		}
	}

}
