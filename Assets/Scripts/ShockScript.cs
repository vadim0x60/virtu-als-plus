﻿using UnityEngine;
using System.Collections;

public class ShockScript : MonoBehaviour {

    public Hub mainHub;

	// Use this for initialization
	void Start () {
    }

    void OnClick()
    {
        mainHub.Shock();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
