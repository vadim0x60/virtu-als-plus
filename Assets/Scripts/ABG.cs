using UnityEngine;
using System.Collections;

public class ABG : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.ABG ();
	}
}
