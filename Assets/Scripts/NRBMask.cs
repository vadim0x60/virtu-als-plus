﻿using UnityEngine;
using System.Collections;

public class NRBMask : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		hub.NRBMask ();
	}
}
