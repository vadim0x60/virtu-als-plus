using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DropDownIntroScript : MonoBehaviour {
    public GameObject continueButton;
    public GameObject inputField;
    private InputField inputFieldText;
    private Dropdown dropper;

	// Use this for initialization
	void Start () {
        dropper = this.GetComponent<Dropdown>();
        inputFieldText = inputField.GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Selector ()
    {
        if (inputField.active)
        {
            inputField.SetActive(false);
        }
        if (dropper.value > 0 && dropper.value < 15)
        {
            continueButton.SetActive(true);
        } else
        {
            continueButton.SetActive(false);
        }
        if (dropper.value == 15)
        {
            inputField.SetActive(true);
        }
    }

    public void Inputter()
    {
        if (inputFieldText.text.Length >= 3)
        {
            continueButton.SetActive(true);
        } else
        {
            continueButton.SetActive(false);
        }
    }
}
