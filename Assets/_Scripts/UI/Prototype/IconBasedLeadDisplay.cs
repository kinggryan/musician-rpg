using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBasedLeadDisplay : MonoBehaviour {

	public GameObject pcLeadingIcon;
	public GameObject npcLeadingIcon;

	// Use this for initialization
	void Awake () {
		NotificationBoard.AddListener(AIMIDIController.Notifications.changedLead,DidChangeLead);
	}

	void Start() {
		npcLeadingIcon.SetActive(false);
		pcLeadingIcon.SetActive(false);
	}
	
	// Update is called once per frame
	void DidChangeLead(object npc, object npcIsLeading) {
		var npcIsLeadingCast = (bool)npcIsLeading;
		if(npcIsLeadingCast) {
			npcLeadingIcon.SetActive(true);
			pcLeadingIcon.SetActive(false);
		} else {
			npcLeadingIcon.SetActive(false);
			pcLeadingIcon.SetActive(true);
		}
	}
}
