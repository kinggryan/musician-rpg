using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpSynth.Wave;
using CSharpSynth.Synthesis;
using CSharpSynth.Banks.Analog;
using CSharpSynth.Banks.Fm;
using CSharpSynth.Banks.Sfz;
using CSharpSynth.Banks;

public class ControllableSynthBank : InstrumentBank {

	const int playerChannel = 0;
	public List<Instrument> playerInstruments;
	public int currentPlayerInstrument = 1;

	public ControllableSynthBank(int sampleRate, string bankfile) : base(sampleRate, bankfile) {
		// Required for compilation, but we don't want any custom behavior here
		playerInstruments = getInstruments(false);
	}

	public ControllableSynthBank(int sampleRate, string bankfile, byte[] Programs, byte[] DrumPrograms) : base(sampleRate, bankfile, Programs, DrumPrograms) {
		playerInstruments = getInstruments(false);
	}

	// Use this for initialization
	public override Instrument getInstrument(int index, bool isDrum)
	{
		if (isDrum == false) {
			if(index == playerChannel) {
				return playerInstruments[currentPlayerInstrument];
			}
		}
		Debug.Log("Channel : " + index);
		
		return base.getInstrument(index, isDrum);
	}
}
