﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour {

	private Animator animator;
	private UnityEngine.UI.Text text;

	void Awake() {
		animator = GetComponent<Animator>();
		text = GetComponent<UnityEngine.UI.Text>();
		NotificationBoard.AddListener(AIMIDIController.Notifications.changedLead, ChangedLead);
	}

	void ChangedLead(object npc, object npcIsLeading) {
		var npcLeadingCast = (bool) npcIsLeading;
		if(npcLeadingCast) {
			SayPhrase("Follow along with me!");
		} else {
			SayPhrase("You take the lead!");
		}
	}

	void SayPhrase(string phrase) {
		animator.SetTrigger("Speak");
		text.text = phrase;
	}
}
