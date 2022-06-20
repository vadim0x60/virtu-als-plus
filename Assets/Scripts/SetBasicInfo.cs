using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBasicInfo : MonoBehaviour {
    public Dropdown professionDropDown;
    public Dropdown experienceDropDown;
    public Dropdown ageDropDown;
    public Dropdown genderDropDown;
    public InputField professionInputField;
    public bool debugging = true;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegisterInfo (string currentKey)
    {
        if (currentKey == "Profession")
        {
            PlayerPrefs.SetInt("Profession", professionDropDown.value);
            if (debugging)
            {
                Debug.Log("Profession: " + professionDropDown.value);
            }
            if (professionDropDown.value == 15)
            {
                PlayerPrefs.SetString("ProfessionString", professionInputField.text);
            }
            else
            {
                PlayerPrefs.SetString("ProfessionString", "None");
            }
        }
        else if (currentKey == "Experience")
        {
            PlayerPrefs.SetInt("Experience", experienceDropDown.value);
            if (debugging)
            {
                Debug.Log("Experience: " + experienceDropDown.value);
            }
        }
        else if (currentKey == "Demographics")
        {
            PlayerPrefs.SetInt("Age", ageDropDown.value);
            PlayerPrefs.SetInt("Gender", genderDropDown.value);
            if (debugging)
            {
                Debug.Log("Age: " + ageDropDown.value);
                Debug.Log("Gender: " + genderDropDown.value);
            }
            PlayerPrefs.SetInt("Registered", 1);
        }
    }
}
