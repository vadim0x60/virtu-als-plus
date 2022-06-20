using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PulseText : MonoBehaviour {
    private float secs=7f;
    private float secsPlusTime=5f;

    public GameObject lifeCheckButton;
    public GameObject resumeButton;

	// Use this for initialization
	void Start () {
        this.GetComponent<Text>().text = "Palpating carotids...\n";
	}
	
	// Update is called once per frame
	void Update () {
        secsPlusTime += Time.deltaTime;
        if (secsPlusTime >= secs)
        {
            if (secs < 11)
            {
                this.GetComponent<Text>().text = "Palpating carotids...\n" + string.Format("{0:0}", secs);
                secs++;
            }
            else if (secs <15)
            {
                this.GetComponent<Text>().text = "No pulse\nNo respiratory effort";
                lifeCheckButton.active = false;
                resumeButton.active = true;
                secs++;
            }
            else
            {
                this.gameObject.active = false;
            }
        }
	}
}
