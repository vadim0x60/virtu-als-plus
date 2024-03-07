using UnityEngine;
using System.Collections;

public class DefibPads : MonoBehaviour {
	public Hub hub;
	public DefibOn defibOn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		if (hub.Clickable) {
			defibOn.AttachPads ();
			hub.AttachPads ();
		}
	}
}
