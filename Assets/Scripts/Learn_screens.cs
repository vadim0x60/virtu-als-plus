using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The learning screen is determined by the text of the selected item in the dropdown menu, which is set to the "screen" variable in this script
// (Probably a better way of doing things, to allow for changes in the menu items?)

public class Learn_screens : MonoBehaviour {
    public string screen = "";
    public GameObject back_arrow;
    public GameObject forward_arrow;
    private int screen_number = 1;
    public int total_screens = 1;
    public Text screen_text;
    public Dropdown dropdown_menu;
    public StartScript startScript;
    public GameObject pauseButtons;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetLearnScreen ()
    {
        screen = dropdown_menu.captionText.text;
        screen_number = 1;
        back_arrow.SetActive(false);
        if (screen == "Circulation")
        {
            total_screens = 2;
        } else if (screen == "Introduction/CPR")
        {
            total_screens = 6;
        }

        if (total_screens > 1)
        {
            forward_arrow.SetActive(true);
        } else
        {
            forward_arrow.SetActive(false);
        }
    }

    public void Forward_Arrow()
    {
        screen_number++;
        if (back_arrow.active == false)
        {
            back_arrow.SetActive(true);
        }
        if (screen_number == total_screens)
        {
            forward_arrow.SetActive(false);
        }
        Screen_Changer();
    }

    public void Back_Arrow()
    {
        screen_number--;
        if (forward_arrow.active == false)
        {
            forward_arrow.SetActive(true);
        }
        if (screen_number == 1)
        {
            back_arrow.SetActive(false);
        }
        Screen_Changer();
    }

    private void Screen_Changer ()
    {
        if (screen == "Circulation")
        {
            if (screen_number == 1) {
                screen_text.text = "Bitesize tutorial 1 of 3: the \"circulation\" assessment";
            } else if (screen_number == 2)
            {
                screen_text.text = "Simulation 1 of 3: the \"circulation\" assessment";
            }
        }
        else if (screen == "Introduction/CPR")
        {
            if (screen_number == 1)
            {
                screen_text.text = "Bitesize tutorial 1 of 3: approaching the sick patient";
            }
            else if (screen_number == 2)
            {
                screen_text.text = "Simulation 1 of 3: approaching the sick patient";
            }
            else if (screen_number == 3)
            {
                screen_text.text = "Bitesize tutorial 2 of 3: getting CPR started";
            }
            else if (screen_number == 4)
            {
                screen_text.text = "Simulation 2 of 3: getting CPR started";
            }
            else if (screen_number == 5)
            {
                screen_text.text = "Bitesize tutorial 3 of 3: after the first rhythm check";
            }
            else if (screen_number == 6)
            {
                screen_text.text = "Simulation 3 of 3: after the first rhythm check";
            }
        }
    }

    public void Go_Button ()
    {
        if (screen == "Circulation")
        {
            if (screen_number == 1)
            {
                Debug.Log("Link to video tutorial on YouTube");
            }
            else if (screen_number == 2)
            {
                //Code to start simulation with stable airway & breathing, for circulation assessment only
                //Each scenario is assigned a number (in this case: 1), which is passed to SetStableTrainingPatient() in the Patient script
                startScript.StartGame("Training", 1);
                pauseButtons.SetActive(true);
                Debug.Log("Game starting?");
                gameObject.SetActive(false);
            }
        }

        if (screen == "Introduction/CPR")
        {
            if (screen_number == 1)
            {
                Debug.Log("Link to video tutorial on YouTube");
            }
            else if (screen_number == 2)
            {
                //Code to start simulation with stable airway & breathing, for circulation assessment only
                //Each scenario is assigned a number (in this case: 1), which is passed to SetStableTrainingPatient() in the Patient script
                startScript.StartGame("Training", 2);
                pauseButtons.SetActive(true);
                Debug.Log("Game starting?");
                gameObject.SetActive(false);
            }
            else if (screen_number == 3)
            {
                Debug.Log("Link to video tutorial on YouTube");
            }
            else if (screen_number == 4)
            {
                //Code to start simulation with stable airway & breathing, for circulation assessment only
                //Each scenario is assigned a number (in this case: 1), which is passed to SetStableTrainingPatient() in the Patient script
                startScript.StartGame("Training", 3);
                pauseButtons.SetActive(true);
                Debug.Log("Game starting?");
                gameObject.SetActive(false);
            }
            else if (screen_number == 5)
            {
                Debug.Log("Link to video tutorial on YouTube");
            }
            else if (screen_number == 6)
            {
                //Code to start simulation with stable airway & breathing, for circulation assessment only
                //Each scenario is assigned a number (in this case: 1), which is passed to SetStableTrainingPatient() in the Patient script
                startScript.StartGame("Training", 4);
                pauseButtons.SetActive(true);
                Debug.Log("Game starting?");
                gameObject.SetActive(false);
            }
        }
    }
}
