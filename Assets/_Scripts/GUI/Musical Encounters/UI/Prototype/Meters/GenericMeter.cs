using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMeter : MonoBehaviour {

	public float value {
		set {
			_value = value;
			UpdateSlider();
		}
		get {
			return _value;
		}
	}
	public float maxValue {
		set {
			_maxValue = value;
			UpdateSlider();
		}
		get {
			return _maxValue;
		}
	}

	private float _value;
	private float _maxValue;

	UnityEngine.UI.Slider slider;

	virtual protected void Awake() {
		slider = GetComponent<UnityEngine.UI.Slider>();
	}

	void Update() {
		UpdateSlider();
	}

	void UpdateSlider() {
		if(maxValue == 0)
			return;
		slider.value = Mathf.Lerp(slider.value, Mathf.Clamp(value/maxValue, 0, 1), 20*Time.deltaTime);
	}
}
