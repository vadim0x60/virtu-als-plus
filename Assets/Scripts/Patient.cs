using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enum = System.Enum;
using System.Xml.Serialization;
using System.IO;

public class Patient : MonoBehaviour {

	public GameObject mainHub;

	public bool debugging = false;
	public bool randomPatient = true;
	public bool arrest = false;
	public bool conscious = true;
	public bool airwayObstructed = false;
	public bool apnoeic = false;
	public bool pulmonaryOedema = false;
	public bool randomRhythm = false;
	public bool bradyCardia = false;
    public bool stable_patient = false;
    public bool wheezy = false;
    public bool dhh_demo = false;

	/* Currently available diagnoses (case sensitive):
	 * Anaphylaxis
	 * Arrhythmia - will set a random unstable arrhythmia that can be shocked back into stable NSR
	 * Asthma
	 * Hyperkalaemia
	 * Hypothermia
	 * MI
	 * Opiates
	 * PE
	 * Pneumonia
	 * Pneumothorax
	 * UGIB
	 */
	public string diagnosis = "Asthma";
    public int training_scenario = 0;

	public string leadIn = "";
	public string chestFindings = "";
	public string pupilsFindings = "";
	public string AVPU = "A";

	public Insights exposureFindings;

    public Insights rhythm1=Insights.HeartRhythmNSR;
    public Insights rhythm2 = Insights.HeartRhythmNSR;
    public Insights rhythm3 = Insights.HeartRhythmNSR;
    public Insights rhythm4 = Insights.HeartRhythmNSR;
    public Insights rhythm5 = Insights.HeartRhythmNSR;

	public string scene = "Conscious";
	public string airwayObstruction = "";

    public float heartRate1 = 80f;
    public float heartRate2 = 20f;
    public float heartRate3 = 80f;
    public float heartRate4 = 80f;
    public float heartRate5 = 80f;

    public float MAP1 = 90f;
    public float MAP2 = 70f;
    public float MAP3 = 70f;
    public float MAP4 = 70f;
    public float MAP5 = 70f;

    public float MAP = 0f;

	public float pH = 7.4f;
	public float pO2 = 12f;
	public float pCO2 = 4.8f;
	public float HCO3 = 24f;
	public float lactate = 0.2f;
	public float K = 4.5f;
	public float hB = 13.5f;

	public float glucose = 4.8f;
	public float temperature = 37f;

    public float pacingThreshold = 70f;
	public float atropineResponsiveness = 15f;
	public float nebResponsiveness = 0.1f;
	public float fluidResponsiveness = 10; //The higher the number, the MORE fluid responsive

	public int fluidRefractoriness = 5; //The higher the number, the LESS fluid refractory
	public int sats = 100;
	public int oxygenResponse = 10;
	public int respRate = 12;
    public int pneumothoraxSide = 0; //0 = none, 1 = right, 2 = left
    public int pneumoniaSide = 0; //0 = none, 1 = right, 2 = left
    public int pleuralEffusionSide = 0; //0 = none, 1 = right, 2 = left
    public int crtPeripheral = 1;
    public int crtCentral = 1;
    public int pupils = 0; //0 = NAD, 1 = right blown, 2 = left blown

    public List<Insights> rhythmList;
	public List<string> diagnosisList;
    public List<float> heartRateList;
    public List<float> MAPList;

    // Use this for initialization
    void Start () {
        rhythmList = new List<Insights>();
        rhythmList.Add(rhythm1);
        rhythmList.Add(rhythm2);
        rhythmList.Add(rhythm3);
        rhythmList.Add(rhythm4);
        rhythmList.Add(rhythm5);

        heartRateList = new List<float>();
        heartRateList.Add(heartRate1);
        heartRateList.Add(heartRate2);
        heartRateList.Add(heartRate3);
        heartRateList.Add(heartRate4);
        heartRateList.Add(heartRate5);

		MAPList = new List<float> ();
        MAPList.Add(MAP1);
        MAPList.Add(MAP2);
        MAPList.Add(MAP3);
        MAPList.Add(MAP4);
        MAPList.Add(MAP5);

		diagnosisList = new List<string> ();
		diagnosisList.Add ("Arrhythmia");
		diagnosisList.Add ("Asthma");

        if (scene == "Training")
        {
            SetStableTrainingPatient();
        } else
        {
            SetRandomPatient();
        }

		mainHub.SetActive (true);
        //CODE FOR LOADING SAVED PATIENT
    }

    public void LoadingFunctions()
    {
        rhythmList = new List<Insights>();
        rhythmList.Add(rhythm1);
        rhythmList.Add(rhythm2);
        rhythmList.Add(rhythm3);
        rhythmList.Add(rhythm4);
        rhythmList.Add(rhythm5);

        heartRateList = new List<float>();
        heartRateList.Add(heartRate1);
        heartRateList.Add(heartRate2);
        heartRateList.Add(heartRate3);
        heartRateList.Add(heartRate4);
        heartRateList.Add(heartRate5);

        MAPList = new List<float>();
        MAPList.Add(MAP1);
        MAPList.Add(MAP2);
        MAPList.Add(MAP3);
        MAPList.Add(MAP4);
        MAPList.Add(MAP5);

        diagnosisList = new List<string>();
        diagnosisList.Add("Arrhythmia");
        diagnosisList.Add("Asthma");
      
        //CODE FOR LOADING SAVED PATIENT
    }

    // Update is called once per frame
    void Update () {
	
	}

	void SetRandomPatient() {
        //Ignore the fact this says "DemoCPR" - it's just the CPR mode, somehow I never got around to undoing the "demo" bit.
		if (scene == "DemoCPR" || scene == "AI") {
			arrest = true;
			if (debugging) {
				Debug.Log ("Scene = DemoCPR");
			}
		} else if (scene == "Random") {
            //Returns a 1:4 chance of arrest=true **Potential to tweak ratio if candidate struggling with arrest protocol?**
			arrest = RandomBool (4);
			if (arrest) {
				scene = "DemoCPR";
			}
		} else if (scene == "Conscious" || scene == "Unconscious") {
			arrest = false;
		}

		if (arrest) {
            //Sets variables in arrest mode
			for (int i = 0; i < rhythmList.Count; i++) {
				rhythmList [i] = RandomRhythm (true);
				while (rhythmList [i] == Insights.HeartRhythmMobitzI ||
				       rhythmList [i] == Insights.HeartRhythmMobitzII ||
				       rhythmList [i] == Insights.HeartRhythmBigeminy) {
					rhythmList [i] = RandomRhythm (true);
				}
			}
		} else {
			if (scene == "Conscious") {
				conscious = true;
			} else if (scene == "Unconscious") {
				conscious = false;
			} else if (scene == "Random") {
				conscious = RandomBool ();
				if (conscious) {
					scene = "Conscious";
				} else {
					scene = "Unconscious";
				}
			}

			if (!conscious) {
				//SET AIRWAY IF PATIENT UNCONSCIOUS:
                //One in three chance of airway being obstructed - another opportunity to individualise training
				airwayObstructed = RandomBool (3);

				if (airwayObstructed) {
					if (diagnosis == "UGIB") {
						airwayObstruction = "Blood";
					} else {
						airwayObstruction = RandomAirwayObstruction ();
					}
					if (debugging) {
						Debug.Log ("Airway obstructed with " + airwayObstruction);
					}
				}
				apnoeic = RandomBool(4);
			}
			if (debugging) {
				Debug.Log ("Scene set as " + scene);
			}

			//FROM HERE ON APPLIES TO BOTH CONSCIOUSNESS AND UNCONSCIOUSNESS

			//SET RESP RATE:
			if (!apnoeic) {
				if (diagnosis == "Opiates") {
					respRate = Random.Range (2, 7);
				} else {
					if (!conscious) {
                        respRate = Random.Range(2, 40);
                        if (dhh_demo)
                        {
                            respRate = 6;
                        }
                    } else {
						respRate = Random.Range (12, 40);
					}
				}
			} else {
				respRate = 0;
			}
            //**Need some more complex code here for asthma, type II respiratory failure, etc.**

			//SET SATS:
			if (!airwayObstructed) {
				sats = Random.Range (70, 101);
			} else {
				sats = Random.Range (85, 101);
			}
            if (dhh_demo)
            {
                sats = 84;
            }

            //To make it more realistic - hypoxic, conscious patient shouldn't have normal RR
            if (sats < 88f && conscious && diagnosis != "Opiates" && respRate < 20) {
				respRate = Random.Range (20, 40);
			}
			if (debugging) {
				Debug.Log ("Sats = " + sats);
			}

			//SET NEB RESPONSIVENESS:
			if (diagnosis == "Asthma") {
                //Neb responsiveness will boost oxygen, and oxygenResponsiveness will need to be able to boost sats
                //into target range after nebs
                wheezy = true;
				nebResponsiveness = Random.Range (0.1f, 1f);
			} else if (diagnosis == "Anaphylaxis") {
				nebResponsiveness = 0.1f;
                wheezy = true;
			} else if (diagnosis == "Arrhythmia" || diagnosis == "MI") {
				pulmonaryOedema = RandomBool ();
                if (dhh_demo)
                {
                    pulmonaryOedema = true;
                }
                if (pulmonaryOedema)
                {
                    wheezy = RandomBool(4);
                }
				//sats = Random.Range (70, 87);
				//Will set some variables regarding pulmonary oedema and furosemide
			}

			int x = Random.Range (88, 101);
			oxygenResponse = x - sats;

			//SET BP:
			MAP = Random.Range (20f, 120f);
			//Function to ensure MAP isn't borderline - makes autonomous decision-making about appropriateness
			//of DCCV very difficult
			while ((MAP > 55f && MAP < 75f)) {
				MAP = Random.Range (20f, 120f);
			}
			MAPList [0] = MAP;

			//SET RHYTHMS:
			if (scene == "DemoCPR" || scene == "AI") {
				randomRhythm = true;
			}
			if (diagnosis == "Arrhythmia") {
				rhythmList [0] = RandomRhythm(false);
				while (rhythmList[0] == Insights.HeartRhythmAtrialFlutter) {
					rhythmList [0] = RandomRhythm(false); //Ensure arrhythmia isn't flutter or VF
				}
                if (dhh_demo)
                {
                    rhythmList[0] = Insights.HeartRhythmCompleteHeartBlock;
                }

				rhythmList [1] = Insights.HeartRhythmNSR;
				rhythmList [2] = RandomRhythm(true);

				rhythmList [3] = RandomRhythm(true);
				rhythmList [4] = RandomRhythm(true);
				MAPList [0] = 40f;
				MAPList [1] = 75f; //To guarantee arrhythmia shocked into stable rhythm
				MAPList [2] = 0f;
				MAPList [3] = 0f;
				MAPList [4] = 0f;
				if (rhythmList [0] == Insights.HeartRhythmNSR || rhythmList [0] == Insights.HeartRhythmMobitzI ||
				    rhythmList [0] == Insights.HeartRhythmMobitzII || rhythmList [0] == Insights.HeartRhythmCompleteHeartBlock ||
				    rhythmList [0] == Insights.HeartRhythmBigeminy) {
					heartRateList [0] = Random.Range (20f, 35f);
					//NB: keep slow AF / flutter out of the bradycardia list, because the variable rates
					//will mess with the functions
					bradyCardia = true;
				} else {
					heartRateList [0] = Random.Range (120f, 200f);
				}

				while (heartRateList [0] > 45f && heartRateList [0] < 120f) {

				}
			} else {
                //This means any non-arrhythmia scenario will have NSR. Probably not ideal.
				rhythmList [0] = Insights.HeartRhythmNSR;
				rhythmList [2] = RandomRhythm(true);
				rhythmList [2] = RandomRhythm(true);
				rhythmList [3] = RandomRhythm(true);
				rhythmList [4] = RandomRhythm(true);
				MAPList [0] = Random.Range (30f, 120f);
				MAPList [1] = 0f;
				MAPList [2] = 0f;
				MAPList [3] = 0f;
				MAPList [4] = 0f;
			}
		}

        //Clinical lead-in:
        leadIn = "One of the ED nurses has called you to see a patient that she is worried about.\n\n" +
            "John is a fit and well 45-year-old man who has been feeling breathless since this morning. " +
            "Please perform an ABCDE assessment and stabilise him to complete the scenario.";

        //Don't need airway findings as set by obstruction bool

        //Chest findings:

		//Setting temperature
		if (diagnosis == "Pneumonia") temperature = Random.Range(38f, 40f);

        //Exposure findings:
        if (diagnosis == "Anaphylaxis") exposureFindings = Insights.ExposureRash;
        if (MAP < 65f) exposureFindings = Insights.ExposurePeripherallyShutdown;
        if (diagnosis == "UGIB") exposureFindings = Insights.ExposureStainedUnderwear;
    }

    void SetStableTrainingPatient()
    {
        stable_patient = true;
        if (training_scenario == 1)
        {
            leadIn = "\nUndertake the \"C\" stage of the ABCDE assessment to complete this simulation.\n\n" +
                "Hit the \"Done\" button when you think you're finished.";
        }
        else if (training_scenario == 2)
        {
            leadIn = "\nUndertake the \"C\" stage of the ABCDE assessment to complete this simulation.\n\n" +
                "Hit the \"Done\" button when you think you're finished.";
        }
        else
        {
            leadIn = "\nNo specific training scenario set";
        }
    }

	public Insights RandomRhythm (bool includeVF) {
        List<Insights> rhythms = new List<Insights>();
        if (scene == "AI")
        {
            rhythms.Add(Insights.HeartRhythmNSR);
            rhythms.Add(Insights.HeartRhythmSVT);
            rhythms.Add(Insights.HeartRhythmAF);
            rhythms.Add(Insights.HeartRhythmAtrialFlutter);
            rhythms.Add(Insights.HeartRhythmMobitzI);
            rhythms.Add(Insights.HeartRhythmMobitzII);
            rhythms.Add(Insights.HeartRhythmCompleteHeartBlock);
            rhythms.Add(Insights.HeartRhythmTorsades);
            rhythms.Add(Insights.HeartRhythmBigeminy);
        }
        else
        {
            rhythms.Add(Insights.HeartRhythmNSR);
            rhythms.Add(Insights.HeartRhythmSVT);
            rhythms.Add(Insights.HeartRhythmAF);
            rhythms.Add(Insights.HeartRhythmAtrialFlutter);
            rhythms.Add(Insights.HeartRhythmVT);
            rhythms.Add(Insights.HeartRhythmMobitzI);
            rhythms.Add(Insights.HeartRhythmMobitzII);
            rhythms.Add(Insights.HeartRhythmCompleteHeartBlock);
            rhythms.Add(Insights.HeartRhythmTorsades);
            rhythms.Add(Insights.HeartRhythmBigeminy);
        }
		if (includeVF) {
			rhythms.Add(Insights.HeartRhythmVF);
		}
		int thisRhythm = Random.Range (0, rhythms.Count);
		/*if (debugging) {
			Debug.Log ("Rhythm selected: " + rhythms [thisRhythm]);
		}*/
		return rhythms [thisRhythm];
	}

	bool RandomBool () {
		return (Random.value > 0.5f);
	}

	bool RandomBool (int oneInOdds) {
		int i = Random.Range (0, oneInOdds);
		if (i == 0) {
			return true;
		} else {
			return false;
		}
	}

	string RandomAirwayObstruction() {
		if (RandomBool()) {
			return "Vomit";
		} else {
			return "Tongue";
		}
	}

	public void Save () {
		gameObject.GetComponent<PatientSerializer> ().Save (this);
	}
}
