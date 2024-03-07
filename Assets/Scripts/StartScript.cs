﻿using UnityEngine;
using System.Collections;

using Unity.MLAgents;
public class StartScript : MonoBehaviour {
	public GameObject patientObject;
	public GameObject gameCanvas;
	public GameObject consciousButton;
	public GameObject unconsciousButton;
	public GameObject arrestButton;
	public GameObject randomButton;
	private Patient patientScript;
	public Camera mainCam;
	public AIMenu aiMenu;
	public Hub hub;
	Quaternion originalCamRot;
	int countdown = 0;

	bool autoplay = false;

	// Use this for initialization
	void Start () {
		originalCamRot = mainCam.transform.rotation;
		patientScript = patientObject.GetComponent<Patient> ();

		foreach (string arg in System.Environment.GetCommandLineArgs()) {
			if (arg == "--autoplay") {
				autoplay = true;
			}
		}

		Academy.Instance.OnEnvironmentReset += Reset;

		if (autoplay) {
			hub.masterTimeScale = 10.0f;
			hub.nonBlockingMode = true;
			RandomConsciousness();
		}
		else GoToMenu();
	}
	
	// Update is called once per frame
	void Update () {
		if (countdown > 0) {
			hub.gameObject.SetActive (true);
			if (autoplay) aiMenu.Play();
 			countdown--;
		}
	}

	void Reset() {
		if (countdown < 0) {
			hub.Reload();
		}
	}

	public void GoToMenu () {
		Vector3 mainCamMenuRot = new Vector3 (0f, 90f, 0f);
		//mainCam.transform.rotation = Quaternion.Euler (mainCamMenuRot);
	}

	public void StartGame(string scene) {
        /*if (scene == "Conscious" || scene == "Unconscious" || scene == "DemoCPR") {
			patientScript.scene = scene;
		}*/
        Debug.Log("Starting game " + scene);
		patientScript.scene = scene;

		patientObject.SetActive (true);
		mainCam.transform.rotation = originalCamRot;
		countdown = 1;

		/*consciousButton.SetActive (false);
		unconsciousButton.SetActive (false);
		randomButton.SetActive (false);
		arrestButton.SetActive (false);*/
	}

    public void StartGame(string scene, int scenario)
    {
        /*if (scene == "Conscious" || scene == "Unconscious" || scene == "DemoCPR") {
			patientScript.scene = scene;
		}*/
        Debug.Log("Starting game " + scene);
        patientScript.scene = scene;
        patientScript.training_scenario = scenario;

        patientObject.SetActive(true);
        mainCam.transform.rotation = originalCamRot;
        countdown = 1;

        /*consciousButton.SetActive (false);
		unconsciousButton.SetActive (false);
		randomButton.SetActive (false);
		arrestButton.SetActive (false);*/
    }

    public void RandomConsciousness () {
		int rand = Random.Range(0,2);
		if (rand == 0) {
			StartGame("Conscious");
		} else {
			StartGame("Unconscious");
		}
	}

    public void DHH_Demo ()
    {
        patientScript.dhh_demo = true;
        StartGame("Unconscious");
    }
}
