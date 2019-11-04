using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Midi;


public class MidiStreamerGroup : MidiStreamer {

	public override uint bpm {
		set {
			foreach(var streamer in midiStreamers) {
				streamer.bpm = value;
			}
			_bpm = value;
		}
	}
	public new int sampleRate {
		set {
			foreach(var streamer in midiStreamers) {
				streamer.sampleRate = value;
			}
			_sampleRate = value;
		}
		get {
			return _sampleRate;
		}
	}
	public new float playbackSpeedMultiplier {
		set {
			foreach(var streamer in midiStreamers) {
				streamer.playbackSpeedMultiplier = value;
			}
			_playbackSpeedMultiplier = value;
		}
		get {
			return _playbackSpeedMultiplier;
		}
	}

	private List<MidiStreamer> midiStreamers = new List<MidiStreamer>();
	private uint _bpm;
	private int _sampleRate;
	private float _playbackSpeedMultiplier;

	public override void PrepareToPlay() {
		foreach(var streamer in midiStreamers) {
			streamer.PrepareToPlay();
		}
	}

	public override void Dispose() {
		foreach(var streamer in midiStreamers) {
			streamer.Dispose();
		}
	}

	public void AddStreamerToGroup(MidiStreamer streamer) {
		streamer.sampleRate = _sampleRate;
		streamer.bpm = _bpm;
		streamer.playbackSpeedMultiplier = _playbackSpeedMultiplier;
		midiStreamers.Add(streamer);
	}

	public override  List<MidiEvent> GetNextMidiEvents(int numFrames)
    {
		// Add all of the streamer events together, but sort them baed on their sample time (which is now their deltaTime)
		var fullEventList = new List<MidiEvent>();
		foreach(var streamer in midiStreamers) {
			var events = streamer.GetNextMidiEvents(numFrames);
			foreach(var ev in events) {
				fullEventList.Add(ev);
			}
		}
		fullEventList.Sort(SortMidiEventsChronologically);
		fullEventList = FilterEvents(fullEventList);
		return fullEventList;
	}

	// SortMidiEventsChronologically functions only on events that have deltaTime set to their sample time
	private int SortMidiEventsChronologically(MidiEvent e1, MidiEvent e2) {
		return e1.deltaTime.CompareTo(e2.deltaTime);
	}
}
