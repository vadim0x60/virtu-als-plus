﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVMcanvas : MonoBehaviour {
	public AnimationTesting anim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick () {
		Debug.Log ("Canvas BVM clicked");
		anim.CanvasBVM ();
	}
}
