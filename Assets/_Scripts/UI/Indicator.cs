using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {
	private ScorekeeperScorebar scoreBar;
	public Image background;

	// Use this for initialization
	void Start () {
		scoreBar = Object.FindObjectOfType<ScorekeeperScorebar>();
		}

	void Awake(){
		var scorekeeper = Object.FindObjectOfType<Scorekeeper>();
		NotificationBoard.AddListener(RPGGameplayManger.Notifications.setJammageThreshold, UpdateIndicator);
	}
	
	void UpdateIndicator(object sender, object newJammageThreshold){
 		float scoreBarWidth = background.GetComponent<RectTransform>().rect.width;
		float jammageThreshold = System.Convert.ToSingle(newJammageThreshold);
		float indicatorXPosition = (jammageThreshold/scoreBar.maxScore) * scoreBarWidth;
		Debug.Log("Indicator x pos: " + indicatorXPosition + " jammage threshold: " + jammageThreshold + " Scorebar width: " + scoreBarWidth + " Max score: " + scoreBar.maxScore);
		RectTransform rt = GetComponent<RectTransform>();
		rt.anchoredPosition = new Vector3(indicatorXPosition,rt.anchoredPosition.y);

	}
}
