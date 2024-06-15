using UnityEngine;
using System.Collections;

public class NRBMask : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.NRBMask ();
	}
}
