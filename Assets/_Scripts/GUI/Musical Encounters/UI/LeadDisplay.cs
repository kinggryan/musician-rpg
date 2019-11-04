using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadDisplay : MonoBehaviour, IAIListener {

	UnityEngine.UI.Text text;

	void Awake() {
		var npc = Object.FindObjectOfType<AIMIDIController>();
		npc.AddListener(this);
		text = GetComponent<UnityEngine.UI.Text>();
	}

	// Use this for initialization
	public void DidChangeAILoop(AIMIDIController npc, AudioLoop loop) {}

	public void DidChangeLead(bool aiIsLeading) {
		if(aiIsLeading) {
			text.text = "NPC is Leading";
		} else {
			text.text = "You are Leading";
		}
	}
}
