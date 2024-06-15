using UnityEngine;
using System.Collections;

public class Adenosine : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown () {
		hub.AdenosineGiven ();
	}
}
