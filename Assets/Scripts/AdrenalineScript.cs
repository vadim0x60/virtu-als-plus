using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdrenalineScript : MonoBehaviour {

    public Hub hub;

	void OnMouseDown ()
	{
		if (!EventSystem.current.IsPointerOverGameObject ()) { 
			hub.AdrenalineGiven ();
		}
    }
}
