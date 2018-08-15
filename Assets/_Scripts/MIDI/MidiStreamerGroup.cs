using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
