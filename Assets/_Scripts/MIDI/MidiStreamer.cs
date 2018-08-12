using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;
using CSharpSynth.Synthesis;

public class MidiStreamer {

    // This class needs to be able to output midi file output. Perhaps override existing MIDI file class?
    // Has an array of midi files
    // Has some sort of event index
    class DynamicMidiFile
    {
        public MidiFile file { get; private set; }
        public int eventIndex;
        public bool looping;

        public void SetMidiFile(MidiFile newFile, int sampleRate, int sampleTime = 0) {
            // What we need to do here is set the eventIndex based on the current sample
            file = newFile;
            //Combine all tracks into 1 track that is organized from lowest to highest abs time
            file.CombineTracks();
            //Convert delta time to sample time
            eventIndex = 0;
            uint lastSample = 0;
            for (int x = 0; x < file.Tracks[0].MidiEvents.Length; x++)
            {
                file.Tracks[0].MidiEvents[x].deltaTime = lastSample + (uint)DeltaTimetoSamples(file.Tracks[0].MidiEvents[x].deltaTime, sampleRate, file);
                lastSample = file.Tracks[0].MidiEvents[x].deltaTime;
                //Update tempo
                if (file.Tracks[0].MidiEvents[x].midiMetaEvent == MidiHelper.MidiMetaEvent.Tempo)
                {
                    file.BeatsPerMinute = MidiHelper.MicroSecondsPerMinute / System.Convert.ToUInt32(file.Tracks[0].MidiEvents[x].Parameters[0]);
                }
            }
            //Set total time to proper value
            file.Tracks[0].TotalTime = file.Tracks[0].MidiEvents[file.Tracks[0].MidiEvents.Length - 1].deltaTime;
            //reset tempo
            file.BeatsPerMinute = 120;
            //mark midi as ready for sequencing
            file.SequencerReady = true;

            // Increment the event index as much as possible
            while (eventIndex < file.Tracks[0].EventCount && file.Tracks[0].MidiEvents[eventIndex].deltaTime < sampleTime)
            {
                eventIndex++;
            }
        }

        private int DeltaTimetoSamples(uint DeltaTime, int sampleRate, MidiFile file)
        {
            return SynthHelper.getSampleFromTime(sampleRate, (DeltaTime * (60.0f / (((int)file.BeatsPerMinute) * file.MidiHeader.DeltaTiming))));
        }
    }

    public uint bpm {
        set {
            for(var i = 0 ; i < midiFiles.Count; i++)
                midiFiles[i].file.BeatsPerMinute = value;
        }
    }

    public int sampleRate;
    public float playbackSpeedMultiplier = 1f;

    //
    private List<DynamicMidiFile> midiFiles;
    private int sampleTime;

    public void LoadMidiFiles(MidiFile[] files)
    {
        // TODO: Unload files properly
        midiFiles = new List<DynamicMidiFile>();

        foreach(var file in files)
        {
            var newDynamicFile = new DynamicMidiFile { };
            newDynamicFile.SetMidiFile(file, sampleRate);

            midiFiles.Add(newDynamicFile);
        }
        
    }

    public void ChangeMidiFile(MidiFile file, int index) {
        midiFiles[index].SetMidiFile(file, sampleRate, sampleTime);
        Debug.Log("Changing midi file at time " + Time.time);
    }

    public List<MidiEvent> GetNextMidiEvents(int numFrames)
    {
        var events = new List<MidiEvent>();
        // For each file
        foreach(var file in midiFiles)
        {
            /* if (sampleTime >= (int)_MidiFile.Tracks[0].TotalTime)
            {
                sampleTime = 0;
                if (looping == true)
                {
                    //Clear the current programs for the channels.
                    Array.Clear(currentPrograms, 0, currentPrograms.Length);
                    //Clear vol, pan, and tune
                    ResetControllers();
                    //set bpm
                    _MidiFile.BeatsPerMinute = 120;
                    //Let the synth know that the sequencer is ready.
                    eventIndex = 0;
                }
                else
                {
                    playing = false;
                    synth.NoteOffAll(true);
                    return null;
                }
            } */
            while (file.eventIndex < file.file.Tracks[0].EventCount && file.file.Tracks[0].MidiEvents[file.eventIndex].deltaTime < (sampleTime + numFrames * playbackSpeedMultiplier))
            {
                events.Add(file.file.Tracks[0].MidiEvents[file.eventIndex]);
                file.eventIndex++;
            }
        }

        sampleTime += Mathf.FloorToInt(numFrames * playbackSpeedMultiplier);

        //  Go through the events of that file and return them
        return events;
    }

    public void PrepareToPlay() {
        // TODO: Get rid of this weird 120 bpm stuff. It feels like a hack in the original sequencer
        foreach(var midiFile in midiFiles)
            midiFile.file.BeatsPerMinute = 120;
    }

    public void Dispose() 
    {
        midiFiles.Clear();
    }

    public void ApplyMidiFilterToTracks(MIDITrackFilter filter)
    {
        foreach(var file in midiFiles)
            file.file.ApplyMidiFilterToTracks(filter);
    }

    
}
