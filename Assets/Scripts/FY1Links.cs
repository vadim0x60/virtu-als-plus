using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FY1Links : MonoBehaviour {
	public string link;
	public Camera menuCam;
	public Camera mainCam;
	public StartScript startScript;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClick() {
		Debug.Log (link);
		if (link == "simulation") {
			startScript.StartGame ("abgPractice");
			menuCam.enabled = false;
			mainCam.enabled = true;
		}
	}
}
