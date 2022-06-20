using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRscript : MonoBehaviour {
	public AnimationTestingOriginal anim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			anim.PlaySequence ("SitIdle");
		} else if (Input.GetKeyDown("2")) {
			anim.PlaySequence ("CPR");
		}
	}
}
