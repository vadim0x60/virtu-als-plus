using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;

public class AIMenu : MonoBehaviour {
    [Range(1, 500)]
    [Tooltip("The frequency with which the agent requests a decision. A DecisionPeriod " +
        "of 5 means that the Agent will request a decision every 5 Academy steps.")]
    public int DecisionPeriod = 200;

    public DirectPlayer DirectPlayer;
    public Hub Hub;

    public GameObject playButton;
    public GameObject pauseButton;

    private bool nonBlockingMode;

    public void Play() {
        Debug.Log ("AI mode on");
        DirectPlayer.AdviceMode = false;

        nonBlockingMode = Hub.nonBlockingMode;
        Hub.nonBlockingMode = true;

        DecisionRequester dr = DirectPlayer.gameObject.AddComponent<DecisionRequester>();
        dr.DecisionPeriod = DecisionPeriod;
        dr.TakeActionsBetweenDecisions = false;

        playButton.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void Pause() {
        Debug.Log ("AI mode off");
        
        foreach (DecisionRequester dr in DirectPlayer.gameObject.GetComponents<DecisionRequester>()) {
            Destroy(dr);
        }

        Hub.nonBlockingMode = nonBlockingMode;

        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }


    public void Decision() {
        DirectPlayer.AdviceMode = false;
        StartCoroutine(DirectPlayer.EnsureAction());
    }

    public void Advice() {
        DirectPlayer.AdviceMode = true;
        StartCoroutine(DirectPlayer.EnsureAction());
    }

    // Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
}