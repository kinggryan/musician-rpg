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
        public AudioLoop loop;
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

    struct MidiFileNoteInfo {
        public int midiFileIndex;
        public int midiNote;
        public MidiFileNoteInfo(int fileIndex, int note) {
            midiFileIndex = fileIndex;
            midiNote = note;
        }
        public override int GetHashCode() {
            return midiFileIndex*1000 + midiNote;
        }
    }

    public override uint bpm {
        set {
            for(var i = 0 ; i < midiFiles.Count; i++)
                midiFiles[i].file.BeatsPerMinute = value;
        }
    }

    private List<DynamicMidiFile> midiFiles = new List<DynamicMidiFile>();
    private int currentMidiFileIndex = 0;
    private Dictionary<MidiFileNoteInfo,int> currentlyPlayingMidiNotes = new Dictionary<MidiFileNoteInfo,int>();

    public void LoadMidiFiles(List<AudioLoop> loops)
    {
        // TODO: Unload files properly
        midiFiles.Clear();

        foreach(var loop in loops)
        {
            var newDynamicFile = new DynamicMidiFile { };
            MidiFile file = null;
            try {
                file = new MidiFile(loop.filename);
            } catch(Exception e) {
                Debug.LogError("Couldn't load midi file with name " + loop.filename + ":" + e);
                continue;
            }

            newDynamicFile.SetMidiFile(file, sampleRate);
            newDynamicFile.loop = loop;

            midiFiles.Add(newDynamicFile);
        }
    }

    /// <summary>
    /// The provided loop MUST be in the array of loaded loops. otherwise this will be an error.
    /// </summary>
    public void SetCurrentMidiFileWith(AudioLoop loop) {
        for(var i = 0 ; i < midiFiles.Count; i++) {
            if(midiFiles[i].loop.name == loop.name) {
                currentMidiFileIndex = i;
                break;
            }
        }
    }

    public void SetCurrentMidiFile(int index) {
        Debug.Log("Changing midi file to " + index);
        currentMidiFileIndex = index;
    }

    public override List<MidiEvent> GetNextMidiEvents(int numFrames)
    {
        var events = new List<MidiEvent>();
        // Iterate through each file so the eventIndex is up to date for all of them always
        // HACK: We shouldn't constantly have to do this - we should be able to just calculate the current event index when we change files but this is a easier/less efficient way of doing it
        for(var i = 0 ; i < midiFiles.Count; i++) {
            var file = midiFiles[i];
            while (
                file.eventIndex < file.file.Tracks[0].EventCount
                && IsFrameInRangeWithLoopingFile(file.file.Tracks[0].MidiEvents[file.eventIndex].deltaTime, 
                sampleTime, 
                Mathf.FloorToInt(sampleTime + numFrames * playbackSpeedMultiplier), 
                (int)file.file.Tracks[0].TotalTime))
            {
                // Only actually output this midi event if this is the active file
                // We actually need to still send note_off events IF those notes are still on from a previos file
                // We need to track which notes are on for a given file and let note off events through because of this
                var newMidiEvent = file.file.Tracks[0].MidiEvents[file.eventIndex].Duplicate();
                if(i == currentMidiFileIndex || ShouldMidiEventFromInactiveFileBeLetThrough(newMidiEvent,i)) {
                
                    // Because of looping, we might end up with a deltatime that is less than the sample time. If this is the case, fix the deltatime
                    while(newMidiEvent.deltaTime < sampleTime) {
                        newMidiEvent.deltaTime += (uint)file.file.Tracks[0].TotalTime;
                    }

                    // Add this to the dictionary of currently sustained notes
                    if(newMidiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On) {
                        var noteInfo = new MidiFileNoteInfo(i, (int)newMidiEvent.parameter1);
                        if(currentlyPlayingMidiNotes.ContainsKey(noteInfo)) {
                            var numSustainedNotes = currentlyPlayingMidiNotes[noteInfo];
                            currentlyPlayingMidiNotes[noteInfo] = numSustainedNotes + 1;
                        } else {
                            currentlyPlayingMidiNotes[noteInfo] = 1;
                        }
                    } else if(newMidiEvent.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off) {
                        // If this is a note off event, decrement the number of sustained notes
                        var noteInfo = new MidiFileNoteInfo(i, (int)newMidiEvent.parameter1);
                        if(currentlyPlayingMidiNotes.ContainsKey(noteInfo)) {
                            var numSustainedNotes = currentlyPlayingMidiNotes[noteInfo];
                            currentlyPlayingMidiNotes[noteInfo] = numSustainedNotes - 1;
                        }
                    }

                    events.Add(newMidiEvent);
                }
                
                // Regardless of whether this is the active file, update the even tindex
                file.eventIndex++;
                if(file.eventIndex >= file.file.Tracks[0].EventCount) {
                    if(file.looping) {
                        file.eventIndex = 0;
                    }
                }
            }
        }

        // Feed the events through all of the filters
        events = FilterEvents(events);

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
        if(maxNumFrames == 0) {
            Debug.LogError("Tried to get frame in range when the max frames were 0");
            return false;
        }
        // If the range doesn't cross the edge of the file, determine if the frame is in the range
        if(endFrameRange / maxNumFrames == startFrameRange / maxNumFrames) {
            return frame % maxNumFrames >= startFrameRange % maxNumFrames && frame % maxNumFrames < endFrameRange % maxNumFrames;
        }
        else {
            return frame % maxNumFrames >= startFrameRange % maxNumFrames || frame % maxNumFrames < endFrameRange % maxNumFrames;
        }
    }

    private bool ShouldMidiEventFromInactiveFileBeLetThrough(MidiEvent ev, int fileIndex) {
        // Let the note through if it is a note off event which is currently being held bya file
        // The reason we do this is that we need to turn off sustained notes from non-playing files
        if(ev.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off) {
            var noteInfo = new MidiFileNoteInfo(fileIndex, (int)ev.parameter1);
            if(currentlyPlayingMidiNotes.ContainsKey(noteInfo) && currentlyPlayingMidiNotes[noteInfo] > 0) {
                return true;
            }
        }

        return false;
    }
}
