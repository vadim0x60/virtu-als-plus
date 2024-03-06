using UnityEngine;
using System.Collections;

public class DefibOn : MonoBehaviour {
    public GameObject screenMask;
	public GameObject heartRateText;
	public GameObject energyText;

	public Control controller;

	bool padsAttached = false;
	bool pressed = false;
	// Use this for initialization
	void Start () {
	
	}

	public void AttachPads () {
		if (pressed) {
			screenMask.SetActive (false);
			controller.DefibReady ();
		} else {
			padsAttached = true;
		}
	}
	
	// Update is called once per frame
	public void OnClick () {
		if (padsAttached) {
            screenMask.SetActive (false);
			controller.DefibReady ();
		}
		heartRateText.SetActive (true);
		energyText.SetActive (true);
		pressed = true;

	}
}
