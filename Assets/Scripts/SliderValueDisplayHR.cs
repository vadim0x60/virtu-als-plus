using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplayHR : MonoBehaviour {
	public Text valueText;
	public Dropdown rhythmDropDown;
	public float minHR = 0f;
	public float maxHR = 0f;
	bool settableRate = true;

	// Use this for initialization
	void Start () {
		valueText = transform.Find ("Value").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Changer () {
		string rhythm = rhythmDropDown.captionText.text;
		if (rhythm == Insights.HeartRhythmNSR) {
			settableRate = true;
			minHR = 20f;
			maxHR = 150f;
		} else if (rhythm == "AF") {
			settableRate = true;
			minHR = 20f;
			maxHR = 100f;
		} else if (rhythm == "Atrial flutter") {
			settableRate = false;
			valueText.text = "N/A";
		} else if (rhythm == Insights.HeartRhythmSVT) {
			settableRate = true;
			minHR = 120f;
			maxHR = 200f;
		} else if (rhythm == Insights.HeartRhythmVT) {
			settableRate = true;
			minHR = 150f;
			maxHR = 250f;
		} else if (rhythm == Insights.HeartRhythmTorsades) {
			settableRate = true;
			minHR = 161f;
			maxHR = 250f;
		} else if (rhythm == "1* AV block") {
			settableRate = true;
			minHR = 20f;
			maxHR = 100f;
		} else if (rhythm == "Mobitz I") {
			settableRate = true;
			minHR = 20f;
			maxHR = 60f;
		} else if (rhythm == "Mobitz II") {
			settableRate = true;
			minHR = 20f;
			maxHR = 60f;
		} else if (rhythm == Insights.HeartRhythmCompleteHeartBlock) {
			settableRate = true;
			minHR = 20f;
			maxHR = 50f;
		} else if (rhythm == Insights.HeartRhythmVF) {
			settableRate = false;
			valueText.text = "N/A";
		}

		if (settableRate) {
			GetComponent<Slider> ().interactable = true;
			float rate = minHR + ((maxHR - minHR) * GetComponent<Slider> ().value);
			int percent = (int)rate;
			valueText.text = percent.ToString ();
		} else {
			GetComponent<Slider> ().interactable = false;
		}
	}
}
