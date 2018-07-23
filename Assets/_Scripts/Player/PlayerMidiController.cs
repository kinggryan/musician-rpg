using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMidiController : MonoBehaviour {

	public MIDIPlayer midiPlayer;
	public CharacterAnimationManager animationManager;
	public PlayerPowerArrow volumeArrow;
	public PlayerPowerArrow gateArrow;
	public PlayerPowerArrow instrumentArrow;

	public PlayerMouseSpringInput gateMouseInput;
	public PlayerMouseSpringInput volumeMouseInput;
	public PlayerMouseSpringInput instrumentMouseInput;
	public PowerCircleAnimationController circleAnimator;
	
	float songBPM = 240f;

	float targetBPM;
	float currentBPM;

	int maxGate = 80;
	int minGate = 78;

	float maxVolume = 2.5f;
	float minVolume = 0.25f;

	int[] playerInstruments = new int[]{1,2,3,4,5,6,7,8};

	float savedGateValue = 0.5f;
	float savedVolumeValue = 0.5f;
	float savedInstrumentValue = 0.5f;


	// Use this for initialization
	void Start () {
		// mouseInput.maxDistance = 250;
		// mouseInput.tension = 2;

		targetBPM = songBPM;
		currentBPM = songBPM;

		volumeArrow.mouseInput = volumeMouseInput;
		gateArrow.mouseInput = gateMouseInput;
		instrumentArrow.mouseInput = instrumentMouseInput;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateInstrument();
		UpdateGate();
		UpdateVolume();

		// Mathf.L
		currentBPM = Mathf.Lerp(currentBPM, targetBPM, 5*Time.deltaTime);
		midiPlayer.playbackRate = Mathf.Clamp(currentBPM / songBPM,0.3f,1.2f);
	}

	void UpdateGate() {
		if(Input.GetButtonDown("Loop2")) {
			gateArrow.isPowerActive = true;
			gateMouseInput.SetAnchorWithValue(savedGateValue);

		} else if(Input.GetButtonUp("Loop2")) {
			gateArrow.isPowerActive = false;
		}

		if(Input.GetButton("Loop2")) {
			savedGateValue = gateMouseInput.GetMouseValue();
			int veloGate = maxGate - Mathf.RoundToInt((maxGate - minGate)*((savedGateValue + 1)/2f));
			// Debug.Log("Gate: " + veloGate);
			midiPlayer.trackGateVelocity = veloGate;
		}
	}

	void UpdateVolume() {
		// Set the displays for each power
		if(Input.GetButtonDown("Loop3")) {
			volumeArrow.isPowerActive = true;
			volumeMouseInput.SetAnchorWithValue(savedVolumeValue);

		} else if(Input.GetButtonUp("Loop3")) {
			volumeArrow.isPowerActive = false;
		}

		if(Input.GetButton("Loop3")) {
			savedVolumeValue = volumeMouseInput.GetMouseValue();
			float volume = minVolume + (maxVolume - minVolume)*((savedVolumeValue + 1)/2f);
			midiPlayer.playerVolume = volume;
		}
	}

	void UpdateInstrument() {
		// Set the displays for each power
		if(Input.GetButtonDown("Loop1")) {
			instrumentArrow.isPowerActive = true;
			instrumentMouseInput.SetAnchorWithValue(savedInstrumentValue);
		} else if(Input.GetButtonUp("Loop1")) {
			instrumentArrow.isPowerActive = false;
		}

		if(Input.GetButton("Loop1")) {
			savedInstrumentValue = instrumentMouseInput.GetMouseValue();
			var instrumentIndex = Mathf.RoundToInt((playerInstruments.Length-1)*((savedInstrumentValue + 1)/2f));
			int instrument = playerInstruments[instrumentIndex];
			Debug.Log("Changing instrument to " + instrument);
			midiPlayer.synthBank.currentPlayerInstrument = instrument;
		}
	}

	void DidChangeBPM(double bpm) {
		targetBPM = (float)bpm;
		
		if(!midiPlayer.IsPlaying()) {
			StartSongWithBPM(targetBPM);
		}
	}

	void StartSongWithBPM(float bpm) {
		currentBPM = bpm;
		circleAnimator.StartSong(bpm);
		animationManager.SetBPM(bpm);
		animationManager.DidStartSong();
		animationManager.UpdateGroove(1f);
		midiPlayer.Play();
	}
}
