using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenSwitchIPhone : MonoBehaviour {
	public bool largeScreen = false;
	public GameObject smallScreenMenu;
	public GameObject largeScreenMenu;
	public GameObject soundOnIcon;
	public GameObject soundOffIcon;
	public GameObject LS_soundOnIcon;
	public GameObject LS_soundOffIcon;
	public Hub hub;

	// Use this for initialization
	void Awake () {
		if (Screen.width > 2000f) {
			Debug.Log ("Switching screen!");
			largeScreen = true;
			smallScreenMenu.SetActive (false);
			largeScreenMenu.SetActive (true);
			if (PlayerPrefs.GetInt ("Sound") == 1) {
				LS_soundOnIcon.SetActive (true);
				LS_soundOffIcon.SetActive (false);
				hub.soundOn = false;
			}
		} else {
			if (PlayerPrefs.GetInt ("Sound") == 1) {
				soundOnIcon.SetActive (true);
				soundOffIcon.SetActive (false);
				hub.soundOn = false;
			}
		}

	}

	// Update is called once per frame
	void Update () {

	}
}
