using UnityEngine;
using System.Collections;

public class Guedel : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.Guedel ();
	}
}
