using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;
using CSharpSynth.Synthesis;

public abstract class MidiStreamer {

    public abstract uint bpm { set; }
    public virtual int sampleRate { get; set; }
    public virtual float playbackSpeedMultiplier { get; set; }
    /// <summary>
    /// If this is >= 0, it will change all midi events streamed by this streamer to that channel
    /// Otherwise, it will not affect the stream
    /// </summary>
    public int outputChannel = -1;
    private List<MIDITrackFilter> filters =  new List<MIDITrackFilter>();
    private MIDIFilterGroup filterGroup = new MIDIFilterGroup();

    protected int sampleTime;

    // Methods
    public MidiStreamer() {
        playbackSpeedMultiplier = 1;
    }

    public virtual List<MidiEvent> GetNextMidiEvents(int numFrames)
    {
        // Increment the sample time
        sampleTime += Mathf.FloorToInt(numFrames * playbackSpeedMultiplier);
        // Return an empty array by default
        return new List<MidiEvent>();
    }

    public virtual void PrepareToPlay() {
        sampleTime = 0;
    }
    
    public abstract void Dispose();

    public void AddFilter(MIDITrackFilter filter) {
        filters.Add(filter);
        filterGroup.AddFilter(filter);
    }

    protected List<MidiEvent> FilterEvents(List<MidiEvent> events) {
        if(outputChannel >= 0) {
            var newEvents = new List<MidiEvent>();
            foreach(var ev in events) {
                ev.channel = (byte)outputChannel;
                newEvents.Add(ev);
            }
            return new List<MidiEvent>(filterGroup.FilterMidiEvents(newEvents.ToArray()));
        }
        return new List<MidiEvent>(filterGroup.FilterMidiEvents(events.ToArray()));
    }
}
