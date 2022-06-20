using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenSwitchIPad : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (Screen.width < 2000f) {
			SceneManager.LoadScene ("CPR trainer iPhone");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
