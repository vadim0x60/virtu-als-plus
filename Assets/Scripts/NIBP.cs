using UnityEngine;
using System.Collections;

public class NIBP : MonoBehaviour {
	public Hub hub;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.NIBP ();
	}
}
