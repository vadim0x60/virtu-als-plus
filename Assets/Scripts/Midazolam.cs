﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Midazolam : MonoBehaviour {
	public Hub hub;
	private bool ready = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnClick() {
		if (hub.MidazolamGiven ()) {
			gameObject.SetActive (false);
		}
	}
}
