using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class PlayerMidiController : MonoBehaviour, ISongUpdateListener {

	public string[] loopNames;

	public MidiSongStructureManager songStructureManager;
	public MIDISongPlayer midiPlayer;
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

	public Chord chord;
	
	public float keyVolumeIncrement;
	public int keyControlIndex = 1;
	public int currentInstIndex;
	public int outputChannel = 0;

	public string[] playerMidiLoops;
	public string playedRhythmString { get; private set; }
	
	float songBPM = 240f;

	float targetBPM;
	float currentBPM;

	int maxGate = 80;
	int minGate = 78;

	float maxVolume = 2.5f;
	float minVolume = 0.25f;

	int[] playerInstruments = new int[]{88,127,126,94,98,102,101,108,103,118,110,111,91,88};

	float savedGateValue = 0.5f;
	float savedVolumeValue = 0.5f;
	float savedInstrumentValue = 0.5f;
	
	MidiFileStreamer midiStreamer;
	MIDIVolumeFilter volumeFilter;
	MIDITrackGate gateFilter;

	List<AudioLoop> playerLoops = new List<AudioLoop>();
	int currentLoopIndex = 0;

	public void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {
		// When we start the next beat
		// Look at what loop is currently playing
		// Determine the rhythm string for that loop on this beat
		// Append it to the rhythm string
		if(beatInfo.currentBeat > 0) {
			var currentLoop = playerLoops[currentLoopIndex];
			playedRhythmString += currentLoop.GetRhythmStringForBeat(beatInfo.currentBeat-1);
		}
	}

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
		midiPlayer.synthBank.currentPlayerInstrument = playerInstruments[0];

		foreach(var loopName in loopNames) {
			playerLoops.Add(AudioLoop.GetLoopForName(loopName));
		}

		midiStreamer = midiPlayer.CreateNewMidiFileStreamer(playerLoops);
		midiStreamer.outputChannel = outputChannel;
		
		gateFilter = new MIDITrackGate();
		gateFilter.activeChannel = outputChannel;
		gateFilter.gateVelocity = maxGate;
		midiStreamer.AddFilter(gateFilter);

		// Add the volume after the gate filter so it doesn't affect the gate
		volumeFilter = new MIDIVolumeFilter();
		volumeFilter.activeChannel = outputChannel;
		volumeFilter.volumeMultiplier = 1;
		midiStreamer.AddFilter(volumeFilter);
		
		currentInstIndex = 0;
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
		// UpdateChord();
        UpdateCurrentMidiFile();

		// Mathf.L
		// currentBPM = Mathf.Lerp(currentBPM, targetBPM, 5*Time.deltaTime);
		// TODO: We should have some central object that coordinates between these different objects so that they don't need to do things like this
		// currentBPM = 90;
		// songStructureManager.bpm = currentBPM;
		// midiPlayer.playbackRate = Mathf.Clamp(currentBPM / songBPM,0.3f,1.2f);
	}

	// void UpdateChord(){
	// 	if(Input.GetKeyDown(KeyCode.Z)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 0;
	// 	} else if(Input.GetKeyDown(KeyCode.A)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 1;
	// 	} else if(Input.GetKeyDown(KeyCode.X)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 2;
	// 	} else if(Input.GetKeyDown(KeyCode.S)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 3;
	// 	} else if(Input.GetKeyDown(KeyCode.C)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 4;
	// 	} else if(Input.GetKeyDown(KeyCode.D)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 5;
	// 	} else if(Input.GetKeyDown(KeyCode.V)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 6;
	// 	} else if(Input.GetKeyDown(KeyCode.F)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 7;
	// 	} else if(Input.GetKeyDown(KeyCode.B)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 8;
	// 	} else if(Input.GetKeyDown(KeyCode.G)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 9;
	// 	} else if(Input.GetKeyDown(KeyCode.N)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 10;
	// 	} else if(Input.GetKeyDown(KeyCode.H)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 11;
	// 	} else if(Input.GetKeyDown(KeyCode.M)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 12;
	// 	} else if(Input.GetKeyDown(KeyCode.J)){
	// 		Debug.Log("Changing from " + midiPlayer.transposeFilter.transposeRules.name + " to IV");
	// 		midiPlayer.chordChange = 13;
	// 	}
		
	// }

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
				gateFilter.gateVelocity = veloGate;
			}
		}else{
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				Debug.Log("GATE UP");
				if(gateFilter.gateVelocity < maxGate){
					gateFilter.gateVelocity++;
				}
				Debug.Log("Gate level:" + gateFilter.gateVelocity);
				keyControlDisplay.UpdateDisplayValues();				
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				Debug.Log("GATE DOWN");
				if(gateFilter.gateVelocity > minGate){
					gateFilter.gateVelocity--;
				}
				Debug.Log("Gate level:" + gateFilter.gateVelocity);
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
				volumeFilter.volumeMultiplier = volume;
			}
		}else{
			
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				float currentVolume = volumeFilter.volumeMultiplier;
				Debug.Log("VOLUME DOWN");
				if(currentVolume > minVolume){
					currentVolume -= keyVolumeIncrement;
				}
				Debug.Log("Volume level:" + currentVolume);				
				volumeFilter.volumeMultiplier = currentVolume;
				keyControlDisplay.UpdateDisplayValues();
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				float currentVolume = volumeFilter.volumeMultiplier;
				Debug.Log("VOLUME UP");
				if(currentVolume < maxVolume){
					currentVolume += keyVolumeIncrement;
				}
				Debug.Log("Volume level:" + currentVolume);				
				volumeFilter.volumeMultiplier = currentVolume;
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

	void UpdateCurrentMidiFile() {
		// Change the midi file at will
		if(Input.GetButtonDown("Loop1")) {
			SetCurrentLoop(0);
		} else if(Input.GetButtonDown("Loop2")) {
			SetCurrentLoop(1);
		} else if(Input.GetButtonDown("Loop3")) {
			SetCurrentLoop(2);
		} else if(Input.GetButtonDown("Loop4")) {
			SetCurrentLoop(3);
		}
	}

	void SetCurrentLoop(int index) {
		currentLoopIndex = index;
		midiStreamer.SetCurrentMidiFile(currentLoopIndex);
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
			if(circleAnimator[i].isActiveAndEnabled)
				circleAnimator[i].StartSong(bpm);
		}
		metro.StartMetro(bpm);
		animationManager.SetBPM(bpm);
		animationManager.DidStartSong();
		animationManager.UpdateGroove(1f);

		midiPlayer.playbackRate = bpm / songBPM;
		midiPlayer.Play();
		songStructureManager.bpm = bpm;
		songStructureManager.StartSong();
	}
}
