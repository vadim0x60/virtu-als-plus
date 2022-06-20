using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour
{

    public GameObject mainHub;
    private Hub hub;
    // Use this for initialization
    void Start()
    {
        hub = mainHub.GetComponent<Hub>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        hub.CameraToDefaultView();
    }
}
