using UnityEngine;
using System.Collections;

public class Fluids : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.Fluids ();
	}
}
