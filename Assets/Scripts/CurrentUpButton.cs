using UnityEngine;
using System.Collections;

public class CurrentUpButton : MonoBehaviour {
    public GameObject defibController;

    // Use this for initialization
    void Start()
    {

    }

    void OnMouseDown()
    {
        defibController.GetComponent<Control>().ChangePaceCurrent("up");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
