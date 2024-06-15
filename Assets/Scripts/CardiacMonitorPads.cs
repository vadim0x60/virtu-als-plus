using UnityEngine;
using System.Collections;

public class CardiacMonitorPads : MonoBehaviour {

	public Hub hub;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		hub.MonitorPadsOn ();
	}
}
