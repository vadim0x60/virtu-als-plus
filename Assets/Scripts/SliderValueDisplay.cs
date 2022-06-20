using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour {
	public Text valueText;
	// Use this for initialization
	void Start () {
		valueText = transform.Find ("Value").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Changer () {
		float percent = GetComponent<Slider> ().value * 100f;
		valueText.text = percent.ToString("0") + "%";
	}
}
