using UnityEngine;
using System.Collections;

public class PacingPauseButton : MonoBehaviour {
    public GameObject defibController;

	// Use this for initialization
	void Start () {
	
	}
    void OnClick()
    {
        defibController.GetComponent<Control>().Pace();
    }


    // Update is called once per frame
    void Update () {
	
	}
}
