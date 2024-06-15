using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Atropine : MonoBehaviour {
	public Hub hub;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		if (!EventSystem.current.IsPointerOverGameObject ()) { 
			hub.AtropineGiven ();
		}
	}
}
