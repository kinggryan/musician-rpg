using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour

{
    public float playerMelody;
	public float playerHarmony;
	public float playerRhythm;
	public float aiMelody;
	public float aiHarmony;
	public float aiRhythm;
    public Slider melodyMeter;
	public Slider harmonyMeter;
	public Slider rhythmMeter;
	public Image[] fill;
	public int successThreshold = 2;
	private float melody;
	private float harmony;
	private float rhythm;
	public float gracePeriod = 3;
	private bool graceTimerOn;
	public float graceTimer = 0;
	public bool fail = false;
	[SerializeField]
	private Color badColor;
	[SerializeField]
	private Color goodColor;
	public bool inEncounter;

    
    // Start is called before the first frame update

	void Start(){
		SetMeterColors();
	}

	void SetMeterColors(){
		foreach(Image image in fill){
			if(graceTimerOn){
				image.color = badColor;
			}else{
				image.color = goodColor;
			}
		}
	}

    public void UpdateScores(){
		melody = playerMelody + aiMelody;
		harmony = playerHarmony + aiHarmony;
		rhythm = playerRhythm + aiRhythm;
		UpdateMeters();
	}

	void UpdateMeters(){
		melodyMeter.value = melody;
		harmonyMeter.value = harmony;
		rhythmMeter.value = rhythm;
	}

	void Update(){
		if(inEncounter){
			if(!graceTimerOn){
				if(melody < successThreshold || harmony < successThreshold || rhythm < successThreshold){
					StartGraceTimer();
				}
			}else if(graceTimerOn){
				graceTimer += Time.deltaTime;
				if(graceTimer >= gracePeriod){
					fail = true;
					StopGraceTimer();
				}
				if(melody >= successThreshold && harmony >= successThreshold && rhythm >= successThreshold){
					StopGraceTimer();
				}
			}
		}
	}

	void StartGraceTimer(){
		graceTimerOn = true;
		SetMeterColors();
	}

	public void StopGraceTimer(){
		graceTimerOn = false;
		graceTimer = 0;
		SetMeterColors();
	}
}
