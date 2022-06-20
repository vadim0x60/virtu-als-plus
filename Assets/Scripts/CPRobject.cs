using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRobject : MonoBehaviour {
	bool starting = false;
	float gravity = 0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (CheckGravity ());
	}

	// Update is called once per frame
	void Update () {
		if (starting) {
			transform.Translate (0f, Input.acceleration.z - gravity, 0f);
			if (transform.position.y > 2f) {
				Vector3 newPos = new Vector3 (transform.position.x, 2f, transform.position.z);
				transform.position = newPos;
			} else if (transform.position.y < -2f) {
				Vector3 newPos = new Vector3 (transform.position.x, -2f, transform.position.z);
				transform.position = newPos;
			}
		}
	}

	IEnumerator CheckGravity () {
		yield return new WaitForSeconds (1);
		gravity = Input.acceleration.z;
		starting = true;
	}
}
