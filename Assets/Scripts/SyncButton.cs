using UnityEngine;
using System.Collections;

public class SyncButton : MonoBehaviour {

    public GameObject controller;

    // Use this for initialization
    void Start()
    {

    }

    void OnMouseDown()
    {
        Control control = controller.GetComponent<Control>();
        control.Sync();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
