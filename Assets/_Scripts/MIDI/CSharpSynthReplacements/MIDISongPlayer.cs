using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;
using MusicianRPG;

[RequireComponent(typeof(AudioSource))]
public class MIDISongPlayer : MonoBehaviour
{
    //Try also: "FM Bank/fm" or "Analog Bank/analog" for some different sounds
    public string bankFilePath = "GM Bank/gm";
    public int bufferSize = 1024;
    public int midiNote = 60;
    public int midiNoteVolume = 100;
    [Range(0, 127)] //From Piano to Gunshot
    public int midiInstrument = 0;
    
    public float playbackRate {
        set { midiSequencer.playbackSpeedMultiplier = value; }
        get { return midiSequencer.playbackSpeedMultiplier; }
    }
    public ControllableSynthBank synthBank {
        get { return midiStreamSynthesizer.controllableSynthBank; }
    }

    // public int chordChange {
    //     set {transposeFilter.transposeRules = transposeRules[value];
    //         midiStreamSynthesizer.NoteOffAll(true);
    // }

    // public TransposeRules[] transposeRules;
    
    public MusicianRPG.MidiSequencer midiSequencer;
    public StreamSynthesizer midiStreamSynthesizer;

    public bool mute;
    //Private 
    private float[] sampleBuffer;
    private float gain = 1f;

    private float sliderValue = 1.0f;
    private float maxSliderValue = 127.0f;
    private bool isPlaying;

    // Awake is called when the script instance
    // is being loaded.
    void Awake()
    {
        midiStreamSynthesizer = new StreamSynthesizer(44100, 2, bufferSize, 40);
        sampleBuffer = new float[midiStreamSynthesizer.BufferSize];
        
        midiStreamSynthesizer.LoadBank(bankFilePath);

        midiSequencer = new MusicianRPG.MidiSequencer(midiStreamSynthesizer);
        // transposeFilter.transposeRules = transposeRules[0];
    }

    public void Play() {
        isPlaying = true;
        midiSequencer.Play();
    }

    public bool IsPlaying() {
        return isPlaying;
    }

    // Start is called just before any of the
    // Update methods is called the first time.
    void Start()
    {
    }

    // Update is called every frame, if the
    // MonoBehaviour is enabled.
    void Update()
    {
        if (mute){
            //muter.volumeMultiplier = 0;
            //midiSequencer.ApplyMidiFilterToTracks(filterGroup);
        }
        // if (!midiSequencer.isPlaying)
        // {
        //     //if (!GetComponent<AudioSource>().isPlaying)
        //     if (ShouldPlayFile)
        //     {
        //         LoadSong(midiFilePath);
        //     }
        // }
        // else if (!ShouldPlayFile)
        // {
        //     midiSequencer.Stop(true);
        // }
    }

        // See http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html for reference code
        //	If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.
        //
        //	The filter is inserted in the same order as the MonoBehaviour script is shown in the inspector. 	
        //	OnAudioFilterRead is called everytime a chunk of audio is routed thru the filter (this happens frequently, every ~20ms depending on the samplerate and platform). 
        //	The audio data is an array of floats ranging from [-1.0f;1.0f] and contains audio from the previous filter in the chain or the AudioClip on the AudioSource. 
        //	If this is the first filter in the chain and a clip isn't attached to the audio source this filter will be 'played'. 
        //	That way you can use the filter as the audio clip, procedurally generating audio.
        //
        //	If OnAudioFilterRead is implemented a VU meter will show up in the inspector showing the outgoing samples level. 
        //	The process time of the filter is also measured and the spent milliseconds will show up next to the VU Meter 
        //	(it turns red if the filter is taking up too much time, so the mixer will starv audio data). 
        //	Also note, that OnAudioFilterRead is called on a different thread from the main thread (namely the audio thread) 
        //	so calling into many Unity functions from this function is not allowed ( a warning will show up ). 	
        private void OnAudioFilterRead(float[] data, int channels)
    {
        //This uses the Unity specific float method we added to get the buffer
        midiStreamSynthesizer.GetNext(sampleBuffer);

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = sampleBuffer[i] * gain;
        }
    }

    public void MidiNoteOnHandler(int channel, int note, int velocity)
    {
        Debug.Log("NoteOn: " + note.ToString() + " Velocity: " + velocity.ToString());
    }

    public void MidiNoteOffHandler(int channel, int note)
    {
        Debug.Log("NoteOff: " + note.ToString());
    }

    // This function creates a new midi streamer with the given file names and returns it.
    // That streamer can then be controlled by outside classes
    public MidiFileStreamer CreateNewMidiFileStreamer(List<AudioLoop> loops) {
        var streamer = new MidiFileStreamer();
        // We must add the streamer to the sequencer BEFORE loading the files, as it sets some required information for the loading process
        midiSequencer.AddMidiStreamer(streamer);
        streamer.LoadMidiFiles(loops);
        return streamer;
    }

    public void AddFilterToMainMidiStreamerGroup(MIDITrackFilter filter) {
        midiSequencer.AddMidiFilterToMainMidiStreamerGroup(filter);
    }
}
