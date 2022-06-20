using UnityEngine;
using System.Collections;

public class RateDownButton : MonoBehaviour {
    public GameObject defibController;

	// Use this for initialization
	void Start () {
	
	}
	
    void OnMouseDown()
    {
        defibController.GetComponent<Control>().ChangePaceRate("down");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
