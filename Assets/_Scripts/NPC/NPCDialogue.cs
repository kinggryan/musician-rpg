using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour {

	private Animator animator;
	private UnityEngine.UI.Text text;
	private string[] rhythmBonusPhrases = new string[]{ "Woa, sweet rhythm!", "Now we're groovin'!", "You've got the rhythm now!"};

	void Awake() {
		animator = GetComponent<Animator>();
		text = GetComponent<UnityEngine.UI.Text>();
		// NotificationBoard.AddListener(AIMIDIController.Notifications.changedLead, ChangedLead);
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.gotRhythmMatchBonus, GotRhythmBonus);
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

	void GotRhythmBonus(object sender, object arg) {
		var randomIndex = Random.Range(0, rhythmBonusPhrases.Length);
		SayPhrase(rhythmBonusPhrases[randomIndex]);
	}
}
