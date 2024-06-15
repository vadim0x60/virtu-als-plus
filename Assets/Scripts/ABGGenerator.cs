using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ABGGenerator : MonoBehaviour {
	public Text pHtext;
	public Text pO2text;
	public Text pCO2text;
	public Text HCO3text;

	private string type;

	public bool debugging = true;

	// Use this for initialization
	void Start () {
	}
	
	public string Generate () {
		//Generate type of ABG:
		List <string> types = new List <string> ();
		types.Add ("normal");
		types.Add ("CRAc");
		types.Add ("DRAc");
		types.Add ("CMAc");
		types.Add ("DMAc");
		types.Add ("RAl");
		types.Add ("MAl");
		types.Add ("mixed");
		int x = Random.Range (0, types.Count);
		string type = types [x];

		if (debugging) {
			Debug.Log (type);
		}

		float pH = 0f;
		float pO2 = 0f;
		float pCO2 = 0f;
		float HCO3 = 0f;

		//Generate individual numbers:
		if (type == "normal") {
			pH = Random.Range (7.35f, 7.45f);
			pO2 = Random.Range(11f,13.1f);
			pCO2 = Random.Range(4.9f,6.1f);
			HCO3 = Random.Range(22f,28f);
		}

		if (type == "CRAc") {
			pH = Random.Range (7.35f, 7.45f);
			pO2 = Random.Range(7.3f,9f);
			pCO2 = Random.Range(6.4f,9f);
			HCO3 = 29f + (((pCO2 - 6f) / 3f) * 11f);
		}

		if (type == "DRAc") {
			pO2 = Random.Range(7.3f,9f);
			pCO2 = Random.Range(6.4f,9f);
			HCO3 = Random.Range(22f,28f);
			pH = 7.35f - (((pCO2 - 6f) / 3f) * 0.2f);
		}

		if (type == "CMAc") {
			pH = Random.Range (7.35f, 7.4f);
			pO2 = Random.Range(10f,15f);
			HCO3 = Random.Range(15f,20f);
			pCO2 = 4.9f - (((22f-HCO3) / 7f) * 3f);
		}

		if (type == "DMAc") {
			pO2 = Random.Range(8f,10f);
			HCO3 = Random.Range(15f,20f);
			pCO2 = Random.Range(4.9f,6.1f);
			pH = 7.35f - (((22f-HCO3) / 7f) * 0.2f);
		}

		if (type == "RAl") {
			pO2 = Random.Range(12f,15f);
			pCO2 = Random.Range(1.9f,4f);
			HCO3 = Random.Range(22f,28f);
			pH = 7.45f + (((4.9f-pCO2) / 3f) * 0.2f);
		}

		if (type == "MAl") {
			pO2 = Random.Range(11f,13f);
			HCO3 = Random.Range(29f,40f);
			pCO2 = Random.Range(4.9f,6.1f);
			pH = 7.45f + (((HCO3-28f) / 12f) * 0.2f);
		}

		if (type == "mixed") {
			pO2 = Random.Range(8f,10f);
			HCO3 = Random.Range(15f,20f);
			pCO2 = Random.Range(6.4f,9f);
			pH = 7.35f - (((22f-HCO3) / 7f) * 0.2f);
			pH -= ((pCO2 - 6f) / 3f) * 0.2f;
		}

		pHtext.text = pH.ToString ("0.00");
		pO2text.text = pO2.ToString ("0.0");
		pCO2text.text = pCO2.ToString ("0.0");
		HCO3text.text = HCO3.ToString ("0.0");

		return type;
	}

}
