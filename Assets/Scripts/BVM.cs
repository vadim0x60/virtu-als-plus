using UnityEngine;
using System.Collections;

public class BVM : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.BVM ();
	}
}
