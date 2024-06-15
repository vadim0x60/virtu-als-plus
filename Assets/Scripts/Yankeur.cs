using UnityEngine;
using System.Collections;

public class Yankeur : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.Yankeur ();
	}
}
