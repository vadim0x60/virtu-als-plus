using UnityEngine;
using System.Collections;

public class TestGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void OnMouseDown () {
        Debug.Log("Pressed");
        Application.LoadLevel("DefibScene");
    }
}
