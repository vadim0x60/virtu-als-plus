using UnityEngine;
using System.Collections;

public class EnergyUp : MonoBehaviour {
    public GameObject controller;
    private Control control;
    // Use this for initialization
    void Start()
    {
        control = controller.GetComponent<Control>();
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        control.EnergyUp();
    }
}