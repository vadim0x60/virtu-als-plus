using UnityEngine;
using System.Collections;

public class ALineCannula : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.ALineOn ();
	}
}
