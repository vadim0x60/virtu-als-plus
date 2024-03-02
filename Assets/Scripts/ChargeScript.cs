using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChargeScript : MonoBehaviour {
    public GameObject shockButton;
    public GameObject messsageScreen;
    public GameObject messageButton;
    public Text messageText;
    public Hub mainHub;
    public Control control;


	// Use this for initialization
	void Start () {
    }

    void OnMouseDown()
    {
		if (control.defibReady && mainHub.Clickable) {
			mainHub.ToggleOffChest ();
			if (!control.charging && !control.charged && mainHub.MAP == 0f) {
				mainHub.SendMessage ("\"Charging defib, resume chest compressions!\"", 0, 2, false);
			} else if (!control.charging && !control.charged) {
				mainHub.SendMessage ("\"Charging defib!\"", 0, 2, false);
			} else if (mainHub.MAP == 0f) {
				mainHub.SendMessage ("\"Dumping charge! Stop chest compressions for rhythm check.\"", 0, 2, false);
				mainHub.PlaySound ("DumpingShock");
			} else {
				mainHub.SendMessage ("\"Dumping charge!\"", 0, 2, false);
			}
			control.Charge ();
		}
    }

    // Update is called once per frame
    void Update ()
    {

	}
}
