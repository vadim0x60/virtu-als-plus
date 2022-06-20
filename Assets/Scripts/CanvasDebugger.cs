using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDebugger : MonoBehaviour {
	Vector3 currentPos;

	// Use this for initialization
	void Start () {
		currentPos = transform.position;
		Debug.Log ("Starting position: " + currentPos);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentPos != transform.position) {
			Debug.Log ("Current position: " + currentPos);
			currentPos = transform.position;
		}
	}
}
