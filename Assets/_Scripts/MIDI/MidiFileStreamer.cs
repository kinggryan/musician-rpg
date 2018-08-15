using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;
using CSharpSynth.Synthesis;

public class MidiFileStreamer: MidiStreamer {

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

            // DEBUG
            looping = true;
        }

        private int DeltaTimetoSamples(uint DeltaTime, int sampleRate, MidiFile file)
        {
            return SynthHelper.getSampleFromTime(sampleRate, (DeltaTime * (60.0f / (((int)file.BeatsPerMinute) * file.MidiHeader.DeltaTiming))));
        }
    }

    public override uint bpm {
        set {
            for(var i = 0 ; i < midiFiles.Count; i++)
                midiFiles[i].file.BeatsPerMinute = value;
        }
    }

    private List<DynamicMidiFile> midiFiles;
    private List<MIDITrackFilter> filters =  new List<MIDITrackFilter>();
    private MIDIFilterGroup filterGroup = new MIDIFilterGroup();

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

    public void LoadMidiFiles(List<string> fileNames)
    {
        var files = new List<MidiFile>();
        foreach(var filename in fileNames) {
            MidiFile file = null;
            try {
                file = new MidiFile(filename);
            } catch(Exception e) {
                Debug.LogError("Couldn't load midi file with name " + filename + ":" + e);
                continue;
            }
            files.Add(file);
        }
        LoadMidiFiles(files.ToArray());
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
            while (
                file.eventIndex < file.file.Tracks[0].EventCount
                && IsFrameInRangeWithLoopingFile(file.file.Tracks[0].MidiEvents[file.eventIndex].deltaTime, 
                sampleTime, 
                Mathf.FloorToInt(sampleTime + numFrames * playbackSpeedMultiplier), 
                (int)file.file.Tracks[0].TotalTime))
            {
                var newMidiEvent = file.file.Tracks[0].MidiEvents[file.eventIndex].Duplicate();
            
                // Because of looping, we might end up with a deltatime that is less than the sample time. If this is the case, fix the deltatime
                while(newMidiEvent.deltaTime < sampleTime) {
                    newMidiEvent.deltaTime += (uint)file.file.Tracks[0].TotalTime;
                }

                events.Add(newMidiEvent);
                file.eventIndex++;
                if(file.eventIndex >= file.file.Tracks[0].EventCount) {
                    if(file.looping) {
                        file.eventIndex = 0;
                    }
                }

            }
        }

        // Feed the events through all of the filters
        events = new List<MidiEvent>(filterGroup.FilterMidiEvents(events.ToArray()));

        sampleTime += Mathf.FloorToInt(numFrames * playbackSpeedMultiplier);

        //  Go through the events of that file and return them
        return events;
    }

    public override void PrepareToPlay() {
        // TODO: Get rid of this weird 120 bpm stuff. It feels like a hack in the original sequencer
        foreach(var midiFile in midiFiles)
            midiFile.file.BeatsPerMinute = 120;
    }

    public override void Dispose() 
    {
        midiFiles.Clear();
    }

    private bool IsFrameInRangeWithLoopingFile(uint frame, int startFrameRange, int endFrameRange, int maxNumFrames) {
        // If the range doesn't cross the edge of the file, determine if the frame is in the range
        if(endFrameRange / maxNumFrames == startFrameRange / maxNumFrames) {
            return frame % maxNumFrames >= startFrameRange % maxNumFrames && frame % maxNumFrames < endFrameRange % maxNumFrames;
        }
        else {
            return frame % maxNumFrames >= startFrameRange % maxNumFrames || frame % maxNumFrames < endFrameRange % maxNumFrames;
        }
    }
}
