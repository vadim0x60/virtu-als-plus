using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DropDownIntroScript02 : MonoBehaviour
{
    public GameObject continueButton;
    private Dropdown dropper;

    // Use this for initialization
    void Start()
    {
        dropper = this.GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Selector()
    {
        if (dropper.value > 0)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
}
