using UnityEngine;
using System.Collections;

public class PaceButton : MonoBehaviour {
    public GameObject defibController;

	// Use this for initialization
	void Start () {
	
	}

    void OnClick ()
    {
        defibController.GetComponent<Control>().Pace();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
