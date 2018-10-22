using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;

public class PlayerMidiController : MonoBehaviour, ISongUpdateListener {

	public static class Notifications {
		public static string changedSelectedDynamicControl = "playerChangedSelectedDynamicControl";
		public static string changedVolume = "playerChangedVolume";
		public static string changedGate = "playerChangedGate";
		public static string changedSelectedLoop = "changedSelectedLoop";
	}

	// The loopNames are now set by the RPGGameplayManager
	[HideInInspector]
	public string[] loopNames;

	public MidiSongStructureManager songStructureManager;
	public MIDISongPlayer midiPlayer;
	public KeyControlDisplay keyControlDisplay;
	public bool mouseControls;

	public Chord chord;

	// public float volumeMultiplier {
	// 	get {
	// 		return volumeFilter.volumeMultiplier;
	// 	}
	// }
	
	public float keyVolumeIncrement;
	public int keyControlIndex = 1;
	public int currentInstIndex;
	public int outputChannel = 0;
	
	float songBPM = 240f;

	int[] playerInstruments = new int[]{74,127,126,94,98,102,101,108,103,118,110,111,91,88};
	int playerInstrumentsIndex = 0;
	
	MidiFileStreamer midiStreamer;
	MIDIVolumeFilter volumeFilter;
	MIDIMonophonicFilter monophonicFilter;

	List<AudioLoop> playerLoops = new List<AudioLoop>();
	int currentLoopIndex = 0;
	private bool loopControlsEnabled = true;

	private List<IPlayerControllerListener> listeners = new List<IPlayerControllerListener>();

	public void AddListener(IPlayerControllerListener listener) {
		listeners.Add(listener);
	}

	public void RemoveListener(IPlayerControllerListener listener) {
		listeners.Remove(listener);
	}

	public void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {
		// When we start the next beat
		// Look at what loop is currently playing
		// Determine the rhythm string for that loop on this beat
		// Append it to the rhythm string
		if(beatInfo.currentBeat > 0) {
			var currentPhrase = SongSection.GetSongPhraseForBeat(songStructureManager.songSections, beatInfo.currentBeat+1);
			if(currentPhrase != null) {
				if(currentPhrase.playerLoop != null) {
					loopControlsEnabled = false;
					SetSongSpecificLoop(currentPhrase.playerLoop);
				} else if (!loopControlsEnabled) {
					// Resume the loop we were playing before the song specific loop
					loopControlsEnabled = true;
					SetCurrentLoop(currentLoopIndex);
				}
			}
			// var currentLoop = playerLoops[currentLoopIndex];
			// playedRhythmString = playedRhythmString.AppendRhythmString(currentLoop.GetRhythmStringForBeat(beatInfo.currentBeat-1));
		}
	}

	public void DidFinishSong() { }

	// Use this for initialization
	void Start () {
		
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		midiPlayer.midiSequencer.setProgram(outputChannel, playerInstruments[playerInstrumentsIndex]);

		var allLoopsToLoad = new List<AudioLoop>();
		foreach(var loopName in loopNames) {
			var loop = AudioLoop.GetLoopForName(loopName);
			playerLoops.Add(loop);
			allLoopsToLoad.Add(loop);
		}

		var songLoops = SongSection.GetSongSpecificPlayerLoops(songStructureManager.songSections);
		allLoopsToLoad.AddRange(songLoops);

		midiStreamer = midiPlayer.CreateNewMidiFileStreamer(allLoopsToLoad);
		midiStreamer.outputChannel = outputChannel;

		// Add the volume after the gate filter so it doesn't affect the gate
		volumeFilter = new MIDIVolumeFilter();
		volumeFilter.activeChannel = outputChannel;
		volumeFilter.volumeMultiplier = 1;
		midiStreamer.AddFilter(volumeFilter);

		// Add the monophonic filter here
		monophonicFilter = new MIDIMonophonicFilter();
		midiStreamer.AddFilter(monophonicFilter);
		
		currentInstIndex = 0;

		// NotificationBoard.SendNotification(Notifications.changedGate,this,1-(1f*gateFilter.gateVelocity-minGate)/(maxGate-minGate));	
		// NotificationBoard.SendNotification(Notifications.changedVolume,this,(volumeFilter.volumeMultiplier-minVolume)/(maxVolume-minVolume));	
	}
	
	// Update is called once per frame
	void Update () {
		
        UpdateCurrentMidiFile();

	}

	void UpdateCurrentMidiFile() {
		if(!loopControlsEnabled)
			return;

		// Change the midi file at will
		if(Input.GetButtonDown("Left")) {
			SetCurrentLoop(0);
		} else if(Input.GetButtonDown("Right")) {
			SetCurrentLoop(1);
		} else if(Input.GetButtonDown("Up")) {
			SetCurrentLoop(2);
		} else if(Input.GetButtonDown("Down")) {
			SetCurrentLoop(3);
		}
	}

	void SetCurrentLoop(int index) {
		currentLoopIndex = index;
		midiStreamer.SetCurrentMidiFile(currentLoopIndex);
		foreach(var listener in listeners) {
			listener.DidChangeLoop(playerLoops[currentLoopIndex], currentLoopIndex);
		}
		NotificationBoard.SendNotification(Notifications.changedSelectedLoop, this, currentLoopIndex);
	}

	void SetSongSpecificLoop(AudioLoop songSpecificLoop) {
		// TODO: Do we need to notify anyone when this happens?
		midiStreamer.SetCurrentMidiFileWith(songSpecificLoop);
	}

	void DidChangeBPM(double bpm) {
		if(!midiPlayer.IsPlaying()) {
			StartSongWithBPM((float)bpm);
		}
	}

	public void StartSongWithBPM(float bpm) {

		SetCurrentLoop(0);

		foreach(var listener in listeners) {
			listener.DidStartSongWithBPM(bpm);
		}
	}

	public void Stop() {
		// midiStreamer.
	}
}
