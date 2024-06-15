using UnityEngine;
using System.Collections;

public class SatsProbe : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown () {
		hub.SatsProbeOn ();
	}
}
