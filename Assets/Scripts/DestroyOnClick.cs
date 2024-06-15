using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DestroyOnClick : MonoBehaviour {
    public GameObject mainHub;

	// Use this for initialization
	void Start () {
	
	}
	
    public void Destroyer()
    {
        this.gameObject.active = false;
    }
}
