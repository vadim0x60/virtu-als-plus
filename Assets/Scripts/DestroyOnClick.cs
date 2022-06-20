using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DestroyOnClick : MonoBehaviour {
    public GameObject mainHub;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Destroyer()
    {
        this.gameObject.active = false;
    }
}
