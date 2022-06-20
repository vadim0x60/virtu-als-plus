using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resusci_move : MonoBehaviour {

    void Start()
    {
    }

    void Update()
    {
        Debug.Log("Updating");
        Vector3 targetUp = transform.rotation * Vector3.up;
        Debug.Log("Rotation: " + targetUp);
    }
}
