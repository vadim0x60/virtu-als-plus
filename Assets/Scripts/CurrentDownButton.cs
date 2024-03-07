using UnityEngine;
using System.Collections;

public class CurrentDownButton : MonoBehaviour {
    public GameObject defibController;

    // Use this for initialization
    void Start()
    {

    }

    void OnMouseDown()
    {
        defibController.GetComponent<Control>().ChangePaceCurrent("down");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
