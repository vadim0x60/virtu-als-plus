using UnityEngine;
using System.Collections;

public class MenuIcon : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnClick() {
		hub.GoToMenu ();
	}
}
