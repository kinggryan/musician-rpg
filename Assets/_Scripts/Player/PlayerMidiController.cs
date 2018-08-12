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
	public PowerCircleAnimationController[] circleAnimator;
	public KeyControlDisplay keyControlDisplay;
	public bool mouseControls;
	public Metronome metro;
	
	

	public enum Chord {
		I,
		i,
		ii,
		iiDim,
		iii,
		bIII,
		IV,
		iv,
		V,
		v,
		vi,
		VI,
		viiDim,
		bVII
	}

	public Chord chord;
	
	public float keyVolumeIncrement;
	public int keyControlIndex = 1;
	public int currentInstIndex;
	
	float songBPM = 240f;

	float targetBPM;
	float currentBPM;

	int maxGate = 80;
	int minGate = 78;

	float maxVolume = 2.5f;
	float minVolume = 0.25f;

	int[] playerInstruments = new int[]{125,127,76,94,98,102,101,108,103,118,110,111,91,88};

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
		midiPlayer.synthBank.currentPlayerInstrument = playerInstruments[6];
		
		currentInstIndex = 6;
	}
	
	// Update is called once per frame
	void Update () {
		if(mouseControls){
			UpdateInstrument();
			UpdateGate();
			UpdateVolume();
		}else{
			UpdateKeyControls();
		}
		UpdateChord();

		// Mathf.L
		currentBPM = Mathf.Lerp(currentBPM, targetBPM, 5*Time.deltaTime);
		midiPlayer.playbackRate = Mathf.Clamp(currentBPM / songBPM,0.3f,1.2f);
	}

	void UpdateChord(){
		if(Input.GetKeyDown(KeyCode.Z)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 0;
		} else if(Input.GetKeyDown(KeyCode.A)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 1;
		} else if(Input.GetKeyDown(KeyCode.X)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 2;
		} else if(Input.GetKeyDown(KeyCode.S)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 3;
		} else if(Input.GetKeyDown(KeyCode.C)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 4;
		} else if(Input.GetKeyDown(KeyCode.D)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 5;
		} else if(Input.GetKeyDown(KeyCode.V)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 6;
		} else if(Input.GetKeyDown(KeyCode.F)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 7;
		} else if(Input.GetKeyDown(KeyCode.B)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 8;
		} else if(Input.GetKeyDown(KeyCode.G)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 9;
		} else if(Input.GetKeyDown(KeyCode.N)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 10;
		} else if(Input.GetKeyDown(KeyCode.H)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 11;
		} else if(Input.GetKeyDown(KeyCode.M)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 12;
		} else if(Input.GetKeyDown(KeyCode.J)){
			Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
			midiPlayer.chordChange = 13;
		}
		
	}

	void UpdateKeyControls(){
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			Debug.Log("LEFT ARROW PRESSED");
			if(keyControlIndex > 0){
				keyControlIndex--;
				Debug.Log("keyControlIndex: " + keyControlIndex);
				keyControlDisplay.UpdateDisplayValues();
			}
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			Debug.Log("RIGHT ARROW PRESSED");
			if(keyControlIndex < 2){
				keyControlIndex++;
				Debug.Log("keyControlIndex: " + keyControlIndex);
				keyControlDisplay.UpdateDisplayValues();
			}
		}
		if(keyControlIndex == 0){
			UpdateInstrument();
		}else if(keyControlIndex == 1){
			UpdateGate();
		}else if(keyControlIndex == 2){
			UpdateVolume();
		}
	}

	void UpdateGate() {
		if(mouseControls){
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
		}else{
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				Debug.Log("GATE UP");
				if(midiPlayer.trackGateVelocity < maxGate){
					midiPlayer.trackGateVelocity++;
				}
				Debug.Log("Gate level:" + midiPlayer.trackGateVelocity);
				keyControlDisplay.UpdateDisplayValues();				
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				Debug.Log("GATE DOWN");
				if(midiPlayer.trackGateVelocity > minGate){
					midiPlayer.trackGateVelocity--;
				}
				Debug.Log("Gate level:" + midiPlayer.trackGateVelocity);
				keyControlDisplay.UpdateDisplayValues();
			}
		}
	}

	void UpdateVolume() {
		// Set the displays for each power
		if(mouseControls){
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
		}else{
			
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				float currentVolume = midiPlayer.playerVolume;
				Debug.Log("VOLUME DOWN");
				if(currentVolume > minVolume){
					currentVolume -= keyVolumeIncrement;
				}
				Debug.Log("Volume level:" + currentVolume);				
				midiPlayer.playerVolume = currentVolume;
				keyControlDisplay.UpdateDisplayValues();
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				float currentVolume = midiPlayer.playerVolume;
				Debug.Log("VOLUME UP");
				if(currentVolume < maxVolume){
					currentVolume += keyVolumeIncrement;
				}
				Debug.Log("Volume level:" + currentVolume);				
				midiPlayer.playerVolume = currentVolume;
				keyControlDisplay.UpdateDisplayValues();
			}
			
		}
		
	}

	void UpdateInstrument() {
		// Set the displays for each power
		if (mouseControls){
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
		else{
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				//Setting Currentinstrument to the midiPLayer instrument
				int currentInstrument = 1000;
				for (int i = 0; i < playerInstruments.Length; i++){
					if (playerInstruments[i] == midiPlayer.synthBank.currentPlayerInstrument){
						currentInstrument = i;
						continue;
					}
				}
				if (currentInstrument == 1000){
					Debug.LogError("Current instrument doesn't match midi instrument!");
				}else{
					Debug.Log("INST DOWN");
					if(currentInstrument > 0){
						currentInstrument--;
					}
					Debug.Log("Current Inst:" + currentInstrument);
					Debug.Log("playerInst: " + playerInstruments[currentInstrument]);			
					midiPlayer.synthBank.currentPlayerInstrument = playerInstruments[currentInstrument];
					currentInstIndex = currentInstrument;
					keyControlDisplay.UpdateDisplayValues();
				}
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				//Setting Currentinstrument to the midiPLayer instrument
				int currentInstrument = 1000;
				for (int i = 0; i < playerInstruments.Length; i++){
					if (playerInstruments[i] == midiPlayer.synthBank.currentPlayerInstrument){
						currentInstrument = i;
						continue;
					}
				}
				if (currentInstrument == 1000){
					Debug.LogError("Current instrument doesn't match midi instrument!");
				}else{
					Debug.Log("INST UP");
					if(currentInstrument < playerInstruments.Length - 1){
						currentInstrument++;
					}
					Debug.Log("Current Inst:" + currentInstrument);
					Debug.Log("playerInst: " + playerInstruments[currentInstrument]);				
					midiPlayer.synthBank.currentPlayerInstrument = playerInstruments[currentInstrument];
					currentInstIndex = currentInstrument;
					keyControlDisplay.UpdateDisplayValues();
				}
			}
			
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
		for (int i = 0; i < circleAnimator.Length; i++){
			circleAnimator[i].StartSong(bpm);
		}
		metro.StartMetro(bpm);
		animationManager.SetBPM(bpm);
		animationManager.DidStartSong();
		animationManager.UpdateGroove(1f);
		midiPlayer.Play();
	}
}
