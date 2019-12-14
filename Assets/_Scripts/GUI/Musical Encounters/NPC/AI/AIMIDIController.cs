using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using MusicianRPG;

public class AIMIDIController : MonoBehaviour, ISongUpdateListener, IPlayerControllerListener {

	public static class Notifications {
		public static string changedLead = "npcDidChangeLead";
	}

	// The known loops of this character
	public string[] knownLoopNames;

	public float volumeFollow;
	public float volumeFollowRndm;

	public float gateFollowTime;
	public float gateFollowRndm;
	public float moveMinInterval;
	public float moveMaxInterval;
	// public float playerVolume {
	// 	get {
	// 		return player.volumeMultiplier;
	// 	}
	// }
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
	public PlayerMidiController player;
	public AIFeedback aiFeedback;
	
	public bool isLeading {
		get {
			return loopDecider is AILeadingLoopDecider;
		}
	}
	
	public bool mute;
	private float moveInterval;
	private float moveTimer;
	private bool moving;
	private float targetVolume;
	private AIJamController aiJamController;

	private AILoopDecider loopDecider;
	private List<AudioLoop> knownLoops;

	private MidiFileStreamer midiStreamer;
    private MIDITrackGate gate;
    private MIDIVolumeFilter volumeFilter;
	MIDIMonophonicFilter monophonicFilter;
	private bool dynMatchBuffer;
	
	private float volumeTimer;
	public float dynamicsMatchTimer;
	
    public bool isPlaying;

	private int currentPlayerGate;
	private float gateMatchTimer;
	private bool gateMatchBuffer;

	private const int channelNumber = 1;
	public const int instrumentIndex = 74;

	private List<IAIListener> listeners = new List<IAIListener>();
	[SerializeField]
	private bool mono;

	public void DidChangeLoop(AudioLoop playerLoop, int index) {
		loopDecider.DidStartPlayerLoop(playerLoop);
	}

	public void AddListener(IAIListener listener) {
		listeners.Add(listener);
	}

	public void RemoveListener(IAIListener listener) {
		listeners.Remove(listener);
	}

	void Awake() {
		// Do this during awake because we want the player to be fully initialized
		player = Object.FindObjectOfType<PlayerMidiController>();
		aiJamController = Object.FindObjectOfType<AIJamController>();
		player.AddListener(this);
	}

    void Start()
    {
		// Regiser with the song structure manager
		var songStructureManager = Object.FindObjectOfType<SongStructureManager>();
		songStructureManager.RegisterSongUpdateListener(this);

		// Load all the loops
		knownLoops = new List<AudioLoop>();
		var allLoopsToLoad = new List<AudioLoop>();
		foreach(var loopName in knownLoopNames) {
			var loop = AudioLoop.GetLoopForName(loopName);
			knownLoops.Add(loop);
			allLoopsToLoad.Add(loop);
		}

		// Add yourself to the midi player
		midiStreamer = midiPlayer.CreateNewMidiFileStreamer(allLoopsToLoad);
		midiStreamer.outputChannel = channelNumber;
		midiPlayer.midiSequencer.setProgram(channelNumber, instrumentIndex);

		if(mono){
			monophonicFilter = new MIDIMonophonicFilter();
			midiStreamer.AddFilter(monophonicFilter);
		}

		gate = new MIDITrackGate();
		// midiStreamer.AddFilter(gate);
		gate.activeChannel = channelNumber;

		volumeFilter = new MIDIVolumeFilter();
		midiStreamer.AddFilter(volumeFilter);
		volumeFilter.activeChannel = channelNumber;
		volumeFilter.volumeMultiplier = 0.8f;

		

		moveInterval = Random.Range(moveMinInterval, moveMaxInterval);

        trackGateVelocity = 79;
        volume = 0.8f;
	}

	public void LoadSong() {
		// Load all the loops
		knownLoops = new List<AudioLoop>();
		var allLoopsToLoad = new List<AudioLoop>();
		foreach(var loopName in knownLoopNames) {
			var loop = AudioLoop.GetLoopForName(loopName);
			knownLoops.Add(loop);
			allLoopsToLoad.Add(loop);
		}

		// Get the song specific loops
		var songStructureManager = Object.FindObjectOfType<SongStructureManager>();
		var songSpecificLoops = SongSection.GetSongSpecificNPCLoops(songStructureManager.songSections);
		allLoopsToLoad.AddRange(songSpecificLoops);
		Debug.Log("AI is loading " + allLoopsToLoad.Count + " loops.");
		midiStreamer.LoadMidiFiles(allLoopsToLoad);

		loopDecider = new AILeadingLoopDecider(knownLoops, songSpecificLoops, songStructureManager.songSections);
		var loopToPlay = loopDecider.ChooseLoopToPlay();
		SetCurrentLoop(loopToPlay);
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

	public void DidStartNextBeat(SongStructureManager.BeatUpdateInfo beatInfo) {
		loopDecider.DidStartNextBeat();
		var newLoopDecider = loopDecider.UpdateState();
		if(newLoopDecider != null) {
			loopDecider = newLoopDecider;
			DidSwitchLead(isLeading);
		}

		var newLoopToPlay = loopDecider.ChooseLoopToPlay();
		//Removed by Travis for PokePrototype
		// if(newLoopToPlay != null)
		// 	midiStreamer.SetCurrentMidiFileWith(newLoopToPlay);
		aiJamController.OnBeat();
	}

	

	public void DidStartSongWithBPM(float bpm) {
		NotificationBoard.SendNotification(Notifications.changedLead, this, true);
	}
	
	public void DidFinishSong() {}

	private IEnumerator WaitThenChangeGate(int newGateVelo, float timeToWait){
		yield return new WaitForSeconds(timeToWait);
		trackGateVelocity = newGateVelo;

	}

	void SetCurrentLoop(AudioLoop loop) {
		// midiStreamer.SetCurrentMidiFileWith(loop);
		// foreach(var listener in listeners) {
		// 	listener.DidChangeAILoop(this, loop);
		// }
	}

	public void SetCurrentLoopWithName(string loopName){
		midiStreamer.SetCurrentMidiFileWithName(loopName);
	}

	void MakeVolumeMove(){
		moveTimer = 0;
		moveInterval = Random.Range(moveMinInterval, moveMaxInterval);
		targetVolume = Random.Range(.5f,2);
		Debug.Log("Making vol move from " + volume + " to " + targetVolume);
		moving = true;
	}

	void MakeGateMove(){
		int newGateVelo = Random.Range(78, 81);
		trackGateVelocity = newGateVelo;
	}

	void MakeMove(){
		return;

		// aiFeedback.ChangeAvatarColorForDuration(moveColor, 1);
		// MakeVolumeMove();
		// MakeGateMove();
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

	// bool dynamicsMatched(){
	// 	if (Mathf.Abs(volume - playerVolume) <= 0.1f){
	// 		return true;
	// 	}else{
	// 		return false;
	// 	}
	// }

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
		// Switch the lead if it's been long enough
		// TODO: This should be based on song triggers or number of beats, something like that.
		if (mute){
            volumeFilter.volumeMultiplier = 0;
        }else{
			volumeFilter.volumeMultiplier = 1;
		}

		// Do the main actions based on whether leading or not
		if (isLeading){
			UpdateLeading();
		} else {
			UpdateFollowing();
		}
	}
	
	void DidSwitchLead(bool npcIsLeading) {
		// if(npcIsLeading)
		// 	aiFeedback.DisplayText("I'll lead now!", 3f, Color.white);
		// else
		// 	aiFeedback.DisplayText("Take the lead!", 3f, Color.white);

		foreach(var listener in listeners) {
			listener.DidChangeLead(npcIsLeading);
		}

		// Post a notification
		NotificationBoard.SendNotification(Notifications.changedLead, this, npcIsLeading);
	}

	private void UpdateLeading() {
		// if(dynamicsMatched() && !dynMatchBuffer){
		// 	dynamicsMatchTimer += Time.deltaTime;
		// 	if (dynamicsMatchTimer >= 2){
		// 		dynMatchBuffer = true;
		// 		Debug.Log("Dynamics Matched");
		// 		aiFeedback.DisplayText("Tasty!", 1, dynamicsTextColor);
		// 	}
		// }else if(!dynamicsMatched()){
		// 	dynMatchBuffer = false;
		// 	dynamicsMatchTimer = 0;
		// }else{
		// 	dynamicsMatchTimer = 0;
		// }

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
	}

	private void UpdateFollowing() {
		// if(dynamicsMatched() && !dynMatchBuffer){
		// 	dynamicsMatchTimer += Time.deltaTime;
		// 	if (dynamicsMatchTimer >= 2){
		// 		dynMatchBuffer = true;
		// 		Debug.Log("Dynamics Matched");
		// 		aiFeedback.DisplayText("Cool!", 1, dynamicsTextColor);
		// 	}
		// }else if(!dynamicsMatched()){
		// 	dynMatchBuffer = false;
		// 	dynamicsMatchTimer = 0;
		// }else{
		// 	dynamicsMatchTimer = 0;
		// }

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
		// if (volume != playerVolume){
		// 	float random = Random.Range(0, volumeFollowRndm);
		// 	volumeTimer += Time.deltaTime / (volumeFollow + random);
		// 	volume = Mathf.Lerp(volume, playerVolume, volumeTimer);
		// }else{
		// 	volumeTimer = 0;
		// }
	}
}
