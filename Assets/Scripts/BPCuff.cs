using UnityEngine;
using System.Collections;

public class BPCuff : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown () {
		hub.BPCuffOn ();
	}
}
