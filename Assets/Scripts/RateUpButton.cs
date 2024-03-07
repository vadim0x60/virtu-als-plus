using UnityEngine;
using System.Collections;

public class RateUpButton : MonoBehaviour {
    public GameObject defibController;

    // Use this for initialization
    void Start()
    {

    }

    void OnMouseDown()
    {
        defibController.GetComponent<Control>().ChangePaceRate("up");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
