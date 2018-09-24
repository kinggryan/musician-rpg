using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterBar : MonoBehaviour {

	public GenericMeter meter;

	private RectTransform meterTransform;
	private RectTransform rt;


	// Use this for initialization
	void Awake(){
		meter = Object.FindObjectOfType<JammageMeter>();
		meterTransform = meter.GetComponent<RectTransform>();
		rt = GetComponent<RectTransform>();
		AddSelfAsListener();
	}

	protected virtual void AddSelfAsListener() {}
	protected virtual void RemoveSelfAsListener() {}

	void OnDestroy() {
		RemoveSelfAsListener();
	}
	
	protected void UpdateIndicator(int barValue){
 		float meterHeight = meterTransform.rect.height;
		float indicatorYPosition = (barValue/meter.maxValue) * meterHeight;
		rt.anchoredPosition = new Vector3(rt.anchoredPosition.x,indicatorYPosition);
	}
}
