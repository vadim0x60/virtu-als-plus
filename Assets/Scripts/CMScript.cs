using UnityEngine;
using System.Collections;

public class CMScript : MonoBehaviour {
	public GameObject bone;
	public float waitTime = 0.5f;

	// Use this for initialization
	void Start () {
		StartCoroutine (setParent ());
	}

	IEnumerator setParent () {
		yield return new WaitForSeconds (waitTime);
		gameObject.transform.parent = bone.transform;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
