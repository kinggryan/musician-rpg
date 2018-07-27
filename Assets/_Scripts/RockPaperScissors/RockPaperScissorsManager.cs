using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperScissorsManager : MonoBehaviour {

	const int minJammage = 0;
	const int maxJammage = 13;

	public enum MusicalStyle {
		Intense,
		Grooving,
		Halting
	}

	public RockPaperScissorsDisplay display;

	MusicalStyle[] npcMusicalStyles = new MusicalStyle[]{ 	MusicalStyle.Grooving, 
															MusicalStyle.Grooving, 
															MusicalStyle.Intense, 
															MusicalStyle.Intense, 
															MusicalStyle.Halting, 
															MusicalStyle.Halting, 
															MusicalStyle.Grooving, 
															MusicalStyle.Intense };
	int currentNPCMusicalStyleIndex;

	public MusicalStyle nextPlayerMusicalStyle;
	int currentJammage;
	int previousJammageModifier;

	// Use this for initialization
	void Start () {
		currentJammage = (maxJammage - minJammage) / 2 + 1;
		display.UpdateUI((currentJammage-minJammage)/(float)(maxJammage-minJammage));
	}

	public void PerformNextMove() {
		var npcMusicalStyle = npcMusicalStyles[currentNPCMusicalStyleIndex];
		var jammageMod = GetJammageModifierFromTwoMusicalStyles(nextPlayerMusicalStyle, npcMusicalStyle);
		previousJammageModifier = jammageMod;
		currentJammage += jammageMod;
		if(currentJammage <= minJammage || currentJammage >= maxJammage)
			LoseGame();

		currentNPCMusicalStyleIndex++;
		if(currentNPCMusicalStyleIndex >= npcMusicalStyles.Length)
			WinGame();

		Debug.Log("Jammage: " + currentJammage);
		display.UpdateUI((currentJammage-minJammage)/(float)(maxJammage-minJammage), npcMusicalStyle, nextPlayerMusicalStyle);
	}

	void LoseGame() {
		Debug.Log("You lost...");
	}

	void WinGame() {
		Debug.Log("Done!");
	}

	int GetJammageModifierFromTwoMusicalStyles(MusicalStyle playerStyle, MusicalStyle npcStyle) {
		// If the player played a trump style to the npc style, return +1
		if(DoesStyleTrumpOtherStyle(playerStyle, npcStyle)) {
			return 1;
		} else if (DoesStyleTrumpOtherStyle(npcStyle, playerStyle)) {
			return -1;
		}
		
		// If they are tied, continue the previous jam
		return previousJammageModifier;
	}

	bool DoesStyleTrumpOtherStyle(MusicalStyle style, MusicalStyle otherStyle) {
		if( (style == MusicalStyle.Intense && otherStyle == MusicalStyle.Grooving) ||
			(style == MusicalStyle.Grooving && otherStyle == MusicalStyle.Halting) ||
			(style == MusicalStyle.Halting && otherStyle == MusicalStyle.Intense) )
			return true;
		
		return false;
	}
}
