using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperScissorsDisplay : MonoBehaviour {

	public UnityEngine.UI.Text npcStyleText;
	public UnityEngine.UI.Text playerStyleText;
	public UnityEngine.UI.Slider jammageMeter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateUI(float jammagePercent) {
		jammageMeter.value = jammagePercent;
	}

	public void UpdateUI(float jammagePercent, RockPaperScissorsManager.MusicalStyle npcStyle, RockPaperScissorsManager.MusicalStyle playerStyle) {
		jammageMeter.value = jammagePercent;
		SetTextWithStyle(npcStyleText, npcStyle);
		SetTextWithStyle(playerStyleText, playerStyle);
	}

	void SetTextWithStyle(UnityEngine.UI.Text text, RockPaperScissorsManager.MusicalStyle style) {
		switch(style) {
			case RockPaperScissorsManager.MusicalStyle.Halting:
				text.color = Color.red;
				text.text = "Halting";
				break;
			case RockPaperScissorsManager.MusicalStyle.Grooving:
				text.color = Color.blue;
				text.text = "Grooving";
				break;
			case RockPaperScissorsManager.MusicalStyle.Intense:
				text.color = Color.yellow;
				text.text = "Intense";
				break;
		}
	}
}
