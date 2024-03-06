using UnityEngine;
using System.Collections;

public class Amiodarone : MonoBehaviour {
    public Hub hub;
    public GameObject messageScreen;

	// Use this for initialization
	void Start () {
	
	}

    void OnClick()
    {
        //Stops the amiodarone script triggering when player clicks "OK" button on message screen
        //(overlaps with amiodarone box)
        if (!messageScreen.active)
        {
            hub.AmiodaroneGiven();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
