using UnityEngine;
using System.Collections;

public class Bloods : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.Bloods ();
	}
}
