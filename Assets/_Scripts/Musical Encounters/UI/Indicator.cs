using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {
	private JammageMeter meter;
	public Image background;

	// Use this for initialization
	void Awake(){
		meter = Object.FindObjectOfType<JammageMeter>();
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.setJammageThreshold, UpdateIndicator);
	}

	void OnDestroy() {
		NotificationBoard.RemoveListener(RPGGameplayManger.Notifications.setJammageThreshold, UpdateIndicator);	
	}
	
	void UpdateIndicator(object sender, object newJammageThreshold){
 		float scoreBarWidth = background.GetComponent<RectTransform>().rect.width;
		float jammageThreshold = (int)newJammageThreshold;
		float indicatorXPosition = (jammageThreshold/meter.maxValue) * scoreBarWidth;
		// Debug.Log("Indicator x pos: " + indicatorXPosition + " jammage threshold: " + jammageThreshold + " Scorebar width: " + scoreBarWidth + " Max score: " + meter.maxValue);
		RectTransform rt = GetComponent<RectTransform>();
		rt.anchoredPosition = new Vector3(indicatorXPosition,rt.anchoredPosition.y);

	}
}
