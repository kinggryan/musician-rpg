using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClock : MonoBehaviour {

	static GameClock instance = null;

	public float dayLength;
	public float nightLength;
	public float dawnLength;
	public float duskLength;
	public TimeOfDay time;
	public float timer = 0;
	public Image tint;
	public float dayAlpha;
	public float nightAlpha;
	public float alpha;

	public enum TimeOfDay{
		dawn,
		day,
		dusk,
		night
	}

	void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print ("Duplicate Spells Destroyed!");
        }
        else 
        {
            instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
        
    }

	void Start () {
		tint.color = new Color(tint.color.r,tint.color.g,tint.color.b,nightAlpha);
		alpha = nightAlpha;
		
	}
	
	void Update () {

		timer += Time.deltaTime;

		

		switch (time){
			case TimeOfDay.dawn:
				alpha -= (Time.deltaTime / dawnLength) * (nightAlpha - dayAlpha);
				break;
			case TimeOfDay.day:
				alpha = dayAlpha;
				break;
			case TimeOfDay.dusk:
				alpha += (Time.deltaTime / dawnLength) * (nightAlpha - dayAlpha);
				break;
			case TimeOfDay.night:
				alpha = nightAlpha;
				break;
		}
		tint.color = new Color(tint.color.r, tint.color.g, tint.color.b, alpha/255);

		if (timer < dawnLength){
			time = TimeOfDay.dawn;
		}else if (timer < dawnLength + dayLength){
			time = TimeOfDay.day;
		}else if (timer < dawnLength + dayLength + duskLength){
			time = TimeOfDay.dusk;
		}else if (timer < dawnLength + dayLength + duskLength + nightLength){
			time = TimeOfDay.night;
		}else{
			timer = 0;
		}

			
	}
}
