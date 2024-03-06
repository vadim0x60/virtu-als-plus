using UnityEngine;
using System.Collections;

public class RateUpButton : MonoBehaviour {
    public GameObject defibController;

    // Use this for initialization
    void Start()
    {

    }

    void OnClick()
    {
        defibController.GetComponent<Control>().ChangePaceRate("up");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
