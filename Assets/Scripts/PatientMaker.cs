using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientMaker : MonoBehaviour {
	public bool debugging = true;
	private bool starter = false;
	private int loaded = 0;

	private int diagnosisValue = 0;

	private string oldLeadIn = "";

	public Toggle saver;
	public InputField leadIn;
	public Dropdown diagnosisList;
	public Dropdown airwayDropdown;
	public Dropdown chestDropdown;
	public InputField respRate;
	public InputField sats;
	public Slider oxygenResponse;
	public Slider bronchodilatorResponse;
	public InputField pH;
	public InputField pO2;
	public InputField pCO2;
	public InputField HCO3;
	public InputField lactate;
	public InputField K;
	public InputField hB;
	public Dropdown initialRhythm;
	public Slider heartRate;
	public InputField MAP;
	public Slider fluidResponsiveness;
	public Slider fluidRefractoriness;
	public InputField glucose;
	public InputField pupils;
	public Dropdown AVPU;
	public InputField temperature;
	public InputField exposure;
	public Dropdown arrestRhythm1;
	public Dropdown arrestRhythm2;

	public PatientSerializer pSerializer;

	public void Load (Patient p) {
		leadIn.text = p.leadIn;
		foreach (Dropdown.OptionData op in diagnosisList.options) {
			if (op.text == p.diagnosis) {
				diagnosisList.value = diagnosisList.options.IndexOf (op);
			}
		}
		if (p.airwayObstruction == "Tongue") {
			airwayDropdown.value = 1;
		} else if (p.airwayObstruction == "Vomit") {
			airwayDropdown.value = 2;
		} else {
			airwayDropdown.value = 0;
		}
		foreach (Dropdown.OptionData op in chestDropdown.options) {
			if (op.text == p.chestFindings) {
				chestDropdown.value = chestDropdown.options.IndexOf (op);
			}
		}
		respRate.text = p.respRate.ToString ();
		sats.text = p.sats.ToString();
		oxygenResponse.value = p.oxygenResponse / 10f;
		bronchodilatorResponse.value = p.nebResponsiveness;
		pH.text = p.pH.ToString("0.00");
		pO2.text = p.pO2.ToString("0.0");
		pCO2.text = p.pCO2.ToString("0.0");
		Debug.Log ("Bicarb = " + p.HCO3);
		HCO3.text = p.HCO3.ToString("0.0");
		lactate.text = p.lactate.ToString("0.0");
		K.text = p.K.ToString("0.0");
		hB.text = p.hB.ToString("0.0");
		foreach (Dropdown.OptionData op in initialRhythm.options) {
			if (RhythmConverter(op.text) == p.rhythm1) {
				initialRhythm.value = initialRhythm.options.IndexOf (op);
			}
		}
		heartRate.value = p.heartRate1 / (heartRate.GetComponent<SliderValueDisplayHR> ().maxHR - heartRate.GetComponent<SliderValueDisplayHR> ().minHR);
		MAP.text = p.MAP.ToString ();
		fluidResponsiveness.value = p.fluidResponsiveness / 10f;
		fluidRefractoriness.value = p.fluidRefractoriness / 10f;

		StartCoroutine (LoadWithDelay ());
	}

	//I have absolutely no idea why this is necessary. Perhaps a delay in reading the .xml file from HDD?
	//Anyway, it IS necessary, don't mess with it!
	IEnumerator LoadWithDelay () {
		if (loaded == 0) {
			yield return new WaitForSeconds (0.5f);
			loaded++;
			pSerializer.Load ();
		} else if (loaded == 1) {
			yield return null;
			loaded = 0;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!starter) {
			Reset ();
			starter = true;
		}
		if (diagnosisList.value != diagnosisValue) {
			if (diagnosisValue == 0) {
				oldLeadIn = leadIn.text;
				Debug.Log (oldLeadIn);
			}
			diagnosisValue = diagnosisList.value;
			DiagnosisChanged ();
		}
	}

	public void DiagnosisChanged () {
		if (saver.isOn) {
			Reset ();
			if (diagnosisList.captionText.text == "Abdominal sepsis") {
				leadIn.text = "John is a 48-year-old man who presents to A&E with a two day history of constant abdominal pain, ";
				leadIn.text += "vomiting and rigors. He is usually fit and well. ";
				leadIn.text += "The triage nurse is concerned that he doesn't look too well and asks you to review him."; 
				respRate.text = Random.Range (22, 32).ToString ();
				oxygenResponse.value = Random.Range (0f, 1f);
				float severity = Random.Range (0f, 1f);
				pH.text = (7.4f - (0.3f * severity)).ToString ("0.00");
				lactate.text = (10f * severity).ToString ("0.0");
				HCO3.text = (24f - (14f * severity)).ToString ("0.0");
				hB.text = Random.Range (8.5f, 18f).ToString ("0.0");
				heartRate.value = Random.Range (0.7f, 1f);
				MAP.text = Random.Range (20, 90).ToString ();
				fluidResponsiveness.value = Random.Range (0f, 1f);
				fluidRefractoriness.value = Random.Range (0.7f, 1f);
				AVPU.value = Random.Range (0, 3);
				temperature.text = Random.Range (37.3f, 39.8f).ToString ("0.0");
				exposure.text = "John looks pale and clammy. His abdomen is rigid and the slightest pressure seems to cause him exquisite pain. ";
				exposure.text += "He does not tolerate a PR examination but there is no obvious blood loss here.";
			} else if (diagnosisList.captionText.text == "Anaphylaxis") {
				leadIn.text = "John is a 26-year-old man who is fit and well other than a sore throat, for which his GP ";
				leadIn.text += "prescribed him amoxicillin yesterday. He has come to A&E feeling flushed and breathless. ";
				leadIn.text += "At triage he is tachycardic and hypotensive, and you've been called to take a look at him.";
				bronchodilatorResponse.value = Random.Range (0f, 1f);
				heartRate.value = Random.Range (0.7f, 1f);
				fluidResponsiveness.value = Random.Range (0f, 1f);
				fluidRefractoriness.value = Random.Range (0.7f, 1f);
				exposure.text = "John is coming out in a red, blanching, macular-papular rash all over. His face is beginning to ";
				exposure.text += "look a little puffy too.";
			} else if (diagnosisList.captionText.text == "Arrhythmia") {
				leadIn.text = "John is a 56-year-old man who has been experiencing palpitations and breathlessness all morning. ";
				leadIn.text += "You are asked to see him regarding his heart rate.";
				initialRhythm.value = Random.Range (1, 11);
				MAP.text = Random.Range (20, 50).ToString();
			} else if (diagnosisList.captionText.text == "Asthma") {
				leadIn.text = "John is a 19-year-old man with a history of recurrent admissions for asthma. He has presented again ";
				leadIn.text += "with a wheeze, which hasn't settled despite multiple doses of his blue inhaler.";
				respRate.text = Random.Range (18, 30).ToString ();
				chestDropdown.value = 1;
				bronchodilatorResponse.value = Random.Range (0.8f, 1f);
			} else if (diagnosisList.captionText.text == "Hyperkalaemia") {
				leadIn.text = "John is a 32-year-old man who had a dodgy takeaway on Saturday night and has had profuse D&V for ";
				leadIn.text += "the last two days. He is not keeping down any fluids and is starting to get muscle cramps. ";
				leadIn.text += "You've been asked to see him as he looks a bit ropey.";
				respRate.text = Random.Range (18, 28).ToString ();
				MAP.text = Random.Range (40, 60).ToString ();
				K.text = Random.Range (5.6f, 7.2f).ToString ("0.0");
			} else if (diagnosisList.captionText.text == "Hypothermia") {
				leadIn.text = "John is a poorly-controlled insulin-dependent diabetic. The polic broke into his flat earlier after ";
				leadIn.text += "neighbours reported he hadn't been seen for several days. They found him collapsed and unresponsive. ";
				leadIn.text += "The paramedics have given IV dextrose and his glucose is now normal, but they couldn't measure his temperature and ";
				leadIn.text += "his heart rate is low.";
				heartRate.value = Random.Range (0f, 0.3f);
				MAP.text = Random.Range (30, 60).ToString ();
				temperature.text = Random.Range (20f, 32f).ToString ("0.0");
			} else if (diagnosisList.captionText.text == "MI") {
				leadIn.text = "John is a 56-year-old smoker with hypertension, hypercholesterolaemia and a strong family history ";
				leadIn.text += "of IHD. He presents with an hour of central, crushing chest pain and is pale and diaphoretic on arrival at A&E. ";
				leadIn.text += "Please assess him.";
				initialRhythm.value = Random.Range (1, 11);
				MAP.text = Random.Range (20, 50).ToString();
			} else if (diagnosisList.captionText.text == "Opiate overdose") {
				leadIn.text = "Following a public health warning about a particularly potent batch of heroin arriving in your area, ";
				leadIn.text += "a car pulls up outside A&E, bundles out a seemingly unconscious patient and drives off. The patient - John, ";
				leadIn.text += "according to the ID in his wallet - is taken straight to resus and you are asked to assess him. You notice track ";
				leadIn.text += "marks on both his arms.";
				airwayDropdown.value = 1;
				float severity = Random.Range (0f, 1f);
				float satFloat = (92f - (7f * severity));
				int satInt = (int)satFloat;
				sats.text = satInt.ToString ();
				pH.text = (7.4f - (0.2f * severity)).ToString ("0.00");
				pCO2.text = (5f + (2f * severity)).ToString ("0.0");
				pO2.text = (10f - (2.5f * severity)).ToString ("0.0");
				AVPU.value = 3;
				pupils.text = "Pinpoint and sluggishly reactive.";
			} else if (diagnosisList.captionText.text == "PE") {
				leadIn.text = "John presents with a three day history of left-sided chest pain which is worse on deep inspiration. ";
				leadIn.text += "He has become increasingly breathlessness and is now feeling very light-headed. He fractured his ankle ";
				leadIn.text += "ten days ago playing football. Please review him.";
				respRate.text = Random.Range (18, 30).ToString ();
				float severity = Random.Range (0f, 1f);
				float satFloat = (94f - (7f * severity));
				int satInt = (int)satFloat;
				sats.text = satInt.ToString ();
				pO2.text = (9f-(2f * severity)).ToString ("0.0");
				heartRate.value = Random.Range (0.8f, 1f);
				MAP.text = Random.Range (20, 50).ToString();
				exposure.text = "John's lower leg is in a plaster cast. His abdomen is soft and non-tender and there is no ";
				exposure.text += "obvious trauma, bleeding nor neurological deficit.";
			} else if (diagnosisList.captionText.text == "Pneumothorax") {
				leadIn.text = "John is a tall, slim man who was playing basketball for the university team earlier and noticed ";
				leadIn.text += "a sharp, right-sided chest pain when he was taking a shot. He thought it was a muscle strain but ";
				leadIn.text += "it has become worse over the course of the day and now it hurts too much to take a deep breath in ";
				leadIn.text += "and he is beginning to feel quite breathless, despite being very fit. Please assess him.";
				respRate.text = Random.Range (18, 30).ToString ();
				float severity = Random.Range (0f, 1f);
				float satFloat = (int)(94f - (7f * severity));
				int satInt = (int)satFloat;
				sats.text = satInt.ToString ();
				pO2.text = (9f - (2f * severity)).ToString ("0.0");
				chestDropdown.value = 8;
				heartRate.value = Random.Range (0.8f, 1f);
				MAP.text = Random.Range (20, 50).ToString();
			} else if (diagnosisList.captionText.text == "UGIB") {
				leadIn.text = "John is a 46-year-old alcoholic who usually refuses to see doctors, but he collapsed in the pub today ";
				leadIn.text += "and the paramedics have brought him to A&E. He is pale, clammy and hypotensive - please review him.";
				respRate.text = Random.Range (18, 30).ToString ();
				sats.text = Random.Range (87, 94).ToString ();
				heartRate.value = Random.Range (0.8f, 1f);
				MAP.text = Random.Range (20, 50).ToString();
				lactate.text = Random.Range (2f, 7f).ToString ("0.0");
				AVPU.value = Random.Range (1, 3);
				exposure.text = "John complains of some epigastric tenderness, though his abdomen is soft. ";
				exposure.text += "However, when you roll him you notice his underwear is full of dark, tarry stool.";
			}
		}
	}

	void Reset () {
		leadIn.text = "Enter the clinical lead-in you want users to see when they open your scenario.";
		airwayDropdown.value = 0;
		chestDropdown.value = 0;
		respRate.text = Random.Range (12, 16).ToString ();
		sats.text = Random.Range (94, 100).ToString ();
		oxygenResponse.value = Random.Range (0f, 0.5f);
		bronchodilatorResponse.value = 0f;
		pH.text = Random.Range (7.35f, 7.45f).ToString ("0.00");
		pO2.text = Random.Range (11.0f, 13.0f).ToString ("0.0");
		pCO2.text = Random.Range (4.3f, 5.7f).ToString ("0.0");
		HCO3.text = Random.Range (22.0f, 28.0f).ToString ("0.0");
		lactate.text = Random.Range (0.1f, 0.9f).ToString ("0.0");
		K.text = Random.Range (3.5f, 5.3f).ToString ("0.0");
		hB.text = Random.Range (13.0f, 18.0f).ToString ("0.0");
		initialRhythm.value = 0;
		heartRate.value = Random.Range (0.6f, 0.9f);
		MAP.text = Random.Range (80, 110).ToString ();
		fluidResponsiveness.value = Random.Range (0f, 0.5f);
		fluidRefractoriness.value = Random.Range (0f, 0.5f);
		glucose.text = Random.Range (4.0f, 7.3f).ToString ("0.0");
		pupils.text = "Equal and reactive to light.";
		AVPU.value = 0;
		temperature.text = Random.Range (36.0f, 37.3f).ToString ("0.0");
		exposure.text = "Abdomen soft, non-tender. No obvious injury nor bleeding source. No focal neurological signs.";
	}

	public void Save () {
		Patient p = new Patient ();
		p.arrest = false;

		//Patient strings:
		p.diagnosis = diagnosisList.captionText.text;
		p.leadIn = leadIn.text;
		if (airwayDropdown.value != 0) {
			if (airwayDropdown.value == 1) {
				p.airwayObstruction = "Tongue";
			} else {
				p.airwayObstruction = "Vomit";
			}
		}
		p.rhythm1 = RhythmConverter(initialRhythm.captionText.text);
		p.rhythm2 = Insights.HeartRhythmNSR;
		p.rhythm3 = RhythmConverter(arrestRhythm1.captionText.text);
		p.rhythm4 = RhythmConverter (arrestRhythm2.captionText.text);
		p.rhythm5 = Insights.HeartRhythmNSR;
		p.chestFindings = chestDropdown.captionText.text;
		p.pupilsFindings = pupils.text;

		// FIXME we are completely ignoring the exposure text
		// Patient maker needs some work... Or maybe it should be cut out of this branch
		//p.exposureFindings = exposure.text;

		//Patient bools:
		if (AVPU.value == 2 || AVPU.value == 3) {
			p.conscious = false;
		} else {
			p.conscious = true;
		}
		if (airwayDropdown.value == 0) {
			p.airwayObstructed = false;
		} else {
			p.airwayObstructed = true;
		}
		int resps = int.Parse (respRate.text);
		if (resps < 5) {
			p.apnoeic = true;
		} else {
			p.apnoeic = false;
		}
		if (chestDropdown.value == 3 || chestDropdown.value == 7) {
			p.pulmonaryOedema = true;
		} else {
			p.pulmonaryOedema = false;
		}
		if (initialRhythm.captionText.text == "NSR" || initialRhythm.captionText.text == "Mobitz I" ||
		    initialRhythm.captionText.text == "Mobitz II" || initialRhythm.captionText.text == "CHB") {
			p.bradyCardia = true;
		} else {
			p.bradyCardia = false;
		}

		//Patient floats:
		p.MAP = float.Parse (MAP.text);
		p.MAP1 = p.MAP;
		p.MAP2 = 75f;
		p.MAP3 = 0f;
		p.MAP4 = 0f;
		p.MAP5 = 0f;
		//Note the heartRateList is effectively redundant - heart rates are set during game play depending on the rhythm
		Text valueText = heartRate.gameObject.transform.Find ("Value").GetComponent<Text> ();
		p.heartRate1 = float.Parse(valueText.text);
		p.nebResponsiveness = bronchodilatorResponse.value;
		p.fluidResponsiveness = fluidResponsiveness.value * 10f;
		p.pH = float.Parse (pH.text);
		p.pO2 = float.Parse (pO2.text);
		p.pCO2 = float.Parse (pCO2.text);
		p.HCO3 = float.Parse (HCO3.text);
		p.K = float.Parse (K.text);
		p.lactate = float.Parse (lactate.text);
		p.hB = float.Parse (hB.text);

		//Patient ints:
		p.fluidRefractoriness = (int)(fluidRefractoriness.value * 10f);
		p.sats = int.Parse (sats.text);
		p.oxygenResponse = (int)(oxygenResponse.value * 10f);
		p.respRate = int.Parse (respRate.text);

		pSerializer.Save (p);
	}


	private Insights RhythmConverter (string rhythm) {
		if (rhythm == "NSR") {
			return Insights.HeartRhythmNSR;
		} else if (rhythm == "AF") {
			return Insights.HeartRhythmAF;
		} else if (rhythm == "Atrial flutter") {
			return Insights.HeartRhythmAtrialFlutter;
		} else if (rhythm == "SVT") {
			return Insights.HeartRhythmSVT;
		} else if (rhythm == "VT") {
			return Insights.HeartRhythmVT;
		} else if (rhythm == "Torsades") {
			return Insights.HeartRhythmTorsades;
		} else if (rhythm == "Mobitz I") {
			return Insights.HeartRhythmMobitzI;
		} else if (rhythm == "Mobitz II") {
			return Insights.HeartRhythmMobitzII;
		} else if (rhythm == "CHB") {
			return Insights.HeartRhythmCompleteHeartBlock;
		} else if (rhythm == "VF") {
			return Insights.HeartRhythmVF;
		} else {
			throw new System.Exception ("Unknown rhythm: " + rhythm);
		}
	}
}
