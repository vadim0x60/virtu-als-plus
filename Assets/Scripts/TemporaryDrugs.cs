using UnityEngine;
using System.Collections;

public class TemporaryDrugs : MonoBehaviour {
    public Hub hub;

	// Use this for initialization
	void Start () {
	
	}

    void OnMouseDown()
    {
        hub.TemporaryDrugMessage();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
