/*
 * using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ECG : MonoBehaviour {
	public GameObject dotPrefab;

	public GameObject monitorScreen;

	public GameObject satsGenerator;
	public GameObject respGenerator;
	public GameObject aLineGenerator;

	public Text heartRateText;
	public Text satsText;
	public Text respRateText;
	public Text bpText;

	private float screenEnd = 0f;
	private float speed = 1f;
	public float timerAcrossScreen = 5f;
	private float timeStamp = 0f;
	public float scale = 1f;
	private float pDur = 0.2f;
	private float duration = 1f;
	private float screenWidth = 0f;
	public float stripLength = 1f;
	private float lastSystole = 0f;
	private float lastPacingSpike = 0f;    
	public float heartRate = 60f;
	private float heartRateUpper = 40f;
	private float heartRateLower = 80f;	
	private float randomHeartRate = 0f;
	private float lengthy;
	private float gap;
	private float QTc;
	private float localY;
	private float screenLeftX = 0f;
	private float mobitzPQ = 1f;
	private float mobitzRatioToOne = 1f;
	private float mobitzQRS = 0f;
	public float nextHR = 80f;
	private float hrDebugger = 0f;
	private float cumulativeDeltaTime = -0.01f;
	private float chbPwaveTimeStamp = 0f;
	private float afTimeStamp = 0f;
	private float aFlutterTimeStamp = 0f;

	private float pWidth = 0.08f;
	private float pqWidth = 0.12f;
	private float qrWidth = 0.02f;
	private float rsWidth = 0.02f;
	private float stwWidth = 0.02f;
	private float stsWidth;
	private float tWidth;

	private string wave = "sts";
	public string rhythm = "nsr";
	public string nextRhyhtm = "nsr";

	private bool pacing = false;
	private bool pacingComplex = false;
	public bool wideQRS = false;
	private bool previousWideQRS = false;    
	private bool shock = false;
	private bool sync = false;
	public bool paused = false;
	private bool shockFail = false;
	public bool charging = false;
	public bool charged = false;
	public bool variableRhythm = false;
	public bool mobitzI = false;
	public bool mobitzII = false;
	private bool firstPass = true;
	private bool changing = false;
	private bool changingRateOnly = false;
	private bool afSliderChanged = false;
	private bool chbPwave = false;
	private bool torsades = false;
	private bool torsadesUp = false;
	private bool bigeminy = false;
	public bool server = true;
	public bool logHeartRate = false;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private Vector3 bending;
	private List <GameObject> sliderList;

	public Sats satsScript;
	public Resp respScript;
	public ALine aLineScript;

	//For disabling slider
	public GameObject heartRateSlider;
	public GameObject bpSlider;
	public GameObject satsSlider;
	public GameObject rrSlider;

	//For setting slider values
	public Slider hrSlider;
	public Text heartRateSliderLower;
	public Text heartRateSliderUpper;
	private float heartRateSliderLowerFloat = 20f;
	private float heartRateSliderUpperFloat = 130f;

	private float previousRR = 16f;
	private float previousSats = 100f;
	private float previousMAP = 100f;

	public Networker networker;

	// Use this for initialization
	void Start () {

		SetPositions ();

		Vector3 leftTop = new Vector3 (0f, Screen.height, 0f);
		Vector3 threeQuartersUp = new Vector3 (0f, 0.75f, 0f);
		float screenTop = Camera.main.ScreenToWorldPoint (leftTop).y;
		screenLeftX = Camera.main.ScreenToWorldPoint (leftTop).x;
		startPosition = new Vector3 (screenLeftX, transform.position.y, transform.position.z);
		endPosition = startPosition;
		transform.position = startPosition;

		GameObject ecgDot = (GameObject)Instantiate (dotPrefab, transform.position, Quaternion.identity);
		ecgDot.name = "Current dot";
		localY = ecgDot.transform.position.y;
		Vector3 right = new Vector3 (Screen.width, 0f, 0f);
		screenWidth = Camera.main.ScreenToWorldPoint (right).x;
		speed = (Camera.main.ScreenToWorldPoint(right).x / timerAcrossScreen)*2f;

		mobitzRatioToOne = Random.Range (2, 4);

		heartRateText.text = heartRate.ToString("0");

		bending = new Vector3(0f,0.35f,0f);

		satsScript.server = server;
		respScript.server = server;
		aLineScript.server = server;

		sliderList = new List<GameObject> ();
		sliderList.Add (heartRateSlider);
		sliderList.Add (bpSlider);
		sliderList.Add (satsSlider);
		sliderList.Add (rrSlider);
		if (!server) {
			for (int i = 0; i < sliderList.Count; i++) {
				sliderList [i].SetActive (false);
			} 
		} else {
			for (int i = 0; i < sliderList.Count; i++) {
				sliderList [i].SetActive (true);
			}
		}

		StartCoroutine (CHBsetter ());

		timeStamp = Time.time;
		SetRhythm ();
		SetOtherFactors ();
	}

	public void ChangeECG (string incomingRhythm) {
		if (rhythm != incomingRhythm) {
			bigeminy = false;
			torsades = false;
			mobitzI = false;
			mobitzII = false;
			if (incomingRhythm == "torsades") {
				torsades = true;
				nextRhyhtm = "vt";
			} else if (incomingRhythm == "bigeminy") {
				wideQRS = false;
				bigeminy = true;
				nextRhyhtm = "nsr";
			} else if (incomingRhythm == "mobitzI") {
				mobitzI = true;
				mobitzII = false;
				nextRhyhtm = "nsr";
			} else if (incomingRhythm == "mobitzII") {
				mobitzII = true;
				mobitzI = false;
				nextRhyhtm = "nsr";
			} else {
				nextRhyhtm = incomingRhythm;
			}

			if (rhythm == "af") {
				afSliderChanged = false;
			}
			changing = true;
		}
	}

	public void ClientChangeECG (float newHeartRate) {
		nextHR = newHeartRate;
		if (rhythm == "vf") {
			heartRateText.text = "-";
		} else {
			heartRateText.text = nextHR.ToString ("0");
		}
		changingRateOnly = true;
	}

	public void ChangeECG (float incomingRate) {
		if (nextHR != incomingRate) {
			nextHR = (incomingRate * heartRateSliderUpperFloat) + heartRateSliderLowerFloat;
			if (rhythm == "vf") {
				heartRateText.text = "-";
			} else {
				heartRateText.text = nextHR.ToString ("0");
			}
			changingRateOnly = true;
			networker.CmdChangeHR (nextHR);
		}
	}

	private void ChangeSlider() {
		if (server) {
			if (rhythm == "nsr" || rhythm == "af") {
				for (int i = 0; i < sliderList.Count; i++) {
					sliderList [i].SetActive (true);
				}
				if (mobitzI || mobitzII) {
					heartRateSliderLower.text = "20";
					heartRateSliderUpper.text = "80";
					heartRateSliderLowerFloat = 20f;
					heartRateSliderUpperFloat = 60f;
				} else if (bigeminy) {
					heartRateSliderLower.text = "20";
					heartRateSliderUpper.text = "80";
					heartRateSliderLowerFloat = 20f;
					heartRateSliderUpperFloat = 60f;
				} else {
					heartRateSliderLower.text = "20";
					heartRateSliderUpper.text = "150";
					heartRateSliderLowerFloat = 20f;
					heartRateSliderUpperFloat = 130f;
				}
				if (rhythm == "af") {
					afSliderChanged = true;
				}
			} else if (rhythm == "aflutter") {
				for (int i = 0; i < sliderList.Count; i++) {
					sliderList [i].SetActive (true);
					heartRateSlider.SetActive (false);
				}
			} else if (rhythm == "vf") {
				for (int i = 0; i < sliderList.Count; i++) {
					sliderList [i].SetActive (false);
				}
			} else if (rhythm == "svt") {
				for (int i = 0; i < sliderList.Count; i++) {
					sliderList [i].SetActive (true);
				}
				heartRateSliderLower.text = "120";
				heartRateSliderUpper.text = "200";
				heartRateSliderLowerFloat = 120f;
				heartRateSliderUpperFloat = 80;
			} else if (rhythm == "vt") {
				for (int i = 0; i < sliderList.Count; i++) {
					sliderList [i].SetActive (true);
				}
				if (torsades) {
					heartRateSliderLower.text = "200";
					heartRateSliderUpper.text = "250";
					heartRateSliderLowerFloat = 200;
					heartRateSliderUpperFloat = 50;
				} else {
					heartRateSliderLower.text = "150";
					heartRateSliderUpper.text = "250";
					heartRateSliderLowerFloat = 150;
					heartRateSliderUpperFloat = 100;
				}
			} else if (rhythm == "chb") {
				for (int i = 0; i < sliderList.Count; i++) {
					sliderList [i].SetActive (true);
				}
				heartRateSliderLower.text = "20";
				heartRateSliderUpper.text = "60";
				heartRateSliderLowerFloat = 20;
				heartRateSliderUpperFloat = 40;
			}
			float value = ((heartRate - heartRateSliderLowerFloat) / heartRateSliderUpperFloat);
			if (value > 1f) {
				value = 1f;
			} else if (value < 0f) {
				value = 0f;
			}

			//Necessary:
			if (heartRateSlider.active) {
				hrSlider.value = value;
			}
			ChangeECG (value);
		}
	}

	void SetPositions() {


		if (server) {
			Vector3 desiredPosition = new Vector3 (gameObject.transform.position.x, 
				7f, 
				gameObject.transform.position.z);
			gameObject.transform.position = desiredPosition;

			desiredPosition = new Vector3 (satsGenerator.transform.position.x, 
				-0.5f, 
				satsGenerator.transform.position.z);
			satsGenerator.transform.position = desiredPosition;

			desiredPosition = new Vector3 (respGenerator.transform.position.x, 
				-4f, 
				respGenerator.transform.position.z);
			respGenerator.transform.position = desiredPosition;

			desiredPosition = new Vector3 (aLineGenerator.transform.position.x, 
				2f, 
				aLineGenerator.transform.position.z);
			aLineGenerator.transform.position = desiredPosition;

			desiredPosition = Camera.main.ScreenToWorldPoint (heartRateText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				7f, 
				desiredPosition.z);
			heartRateText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);

			desiredPosition = Camera.main.ScreenToWorldPoint (satsText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				-1f, 
				desiredPosition.z);
			satsText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);

			desiredPosition = Camera.main.ScreenToWorldPoint (respRateText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				-4f, 
				desiredPosition.z);
			respRateText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);

			desiredPosition = Camera.main.ScreenToWorldPoint (bpText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				4f, 
				desiredPosition.z);
			bpText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);

		} else if (!server) {

			Vector3 desiredPosition = new Vector3 (gameObject.transform.position.x, 
				5.5f, 
				gameObject.transform.position.z);
			gameObject.transform.position = desiredPosition;

			desiredPosition = new Vector3 (satsGenerator.transform.position.x, 
				-2.5f, 
				satsGenerator.transform.position.z);
			satsGenerator.transform.position = desiredPosition;

			desiredPosition = new Vector3 (respGenerator.transform.position.x, 
				-7f, 
				respGenerator.transform.position.z);
			respGenerator.transform.position = desiredPosition;

			desiredPosition = new Vector3 (aLineGenerator.transform.position.x, 
				0.5f, 
				aLineGenerator.transform.position.z);
			aLineGenerator.transform.position = desiredPosition;

			desiredPosition = Camera.main.ScreenToWorldPoint (heartRateText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				5.5f, 
				desiredPosition.z);
			heartRateText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);

			desiredPosition = Camera.main.ScreenToWorldPoint (satsText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				-3f, 
				desiredPosition.z);
			satsText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);

			desiredPosition = Camera.main.ScreenToWorldPoint (respRateText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				-7f, 
				desiredPosition.z);
			respRateText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);

			desiredPosition = Camera.main.ScreenToWorldPoint (bpText.transform.position);
			desiredPosition = new Vector3 (desiredPosition.x, 
				1f, 
				desiredPosition.z);
			bpText.transform.position = Camera.main.WorldToScreenPoint (desiredPosition);
		} 

		satsGenerator.SetActive (true);
		respGenerator.SetActive (true);
		aLineGenerator.SetActive (true);
	}

	//SETS HEARTRATE TO PATIENT'S HEARTRATE AND ENSURES IS WITHIN ACCEPTED VALUES
	void SetRhythm()
	{
		if (rhythm != "nsr" && rhythm != "af" && rhythm != "aflutter" &&
			rhythm != "svt" && rhythm != "vt" && rhythm != "vf" && rhythm != "sats" && rhythm != "chb") {
			Debug.Log ("The defib does not recognise the rhythm: " + rhythm);
		} else if (rhythm == "nsr") {
			//COMMENTED OUT SECTIONS BELOW ARE FOR RANDOMLY CHANGING RHYTHM
			if (heartRate < 20f) {
				heartRate = 20f;
			}
			if (heartRate > 180f) {
				heartRate = 180f;
			}
			variableRhythm = false;
		} else if (rhythm == "af") {
			//heartRate = Random.Range(20f, 150f);
			heartRateUpper = Random.Range (heartRate + (heartRate / 10), heartRate + (heartRate / 3));
			heartRateLower = Random.Range (heartRate - (heartRate / 10), heartRate - (heartRate / 3));
			if (heartRate < 20f) {
				heartRate = 20f;
			}
			if (heartRate > 180f) {
				heartRate = 180f;
			}
			wideQRS = false;
		} else if (rhythm == "aflutter") {
			heartRate = 75f;
			//RANDOMLY SETS RHYTM TO FIXED OR VARIABLE BLOCK
			bool[] trueFalse = new bool[] { true, false };
			int randomN = Random.Range (0, 2);
			variableRhythm = trueFalse [randomN];
			//MAKES COMPLEXES NARROW IF BLOCK IS VARIABLE
			//(OTHERWISE IT CAN LOOK LIKE VT)
			if (variableRhythm) {
				wideQRS = false;
			}
		} else if (rhythm == "svt") {
			if (heartRate < 120f || heartRate > 200f) {
				heartRate = Random.Range (120f, 200f);
			}
			//heartRate = Random.Range(120f, 200f);
			variableRhythm = false;
			wideQRS = false;
		} else if (rhythm == "vt" || rhythm == "vf") {
			wideQRS = true;
			variableRhythm = false;
		} else if (rhythm == "chb") {
			wideQRS = true;
			variableRhythm = false;
			if (heartRate < 20f) {
				heartRate = 20f;
			}
			if (heartRate > 60) {
				heartRate = 60;
			}
		}

		//For reseting QRS width if pacer switched off
		//previousWideQRS = wideQRS;
	}

	//SETS CORRECTED QTc, SETS GAP BETWEEN BEATS ACCORDING TO HEART RATE, SETS INITIAL HEART RATE FOR AF
    //(AF WILL VARY 20% ABOVE AND BELOW SET HEART RATE
	void SetOtherFactors()
	{
		if (wideQRS)
		{
			stwWidth = 0.1f;
			qrWidth = 0.04f;
		} else if (rhythm=="svt")
		{
			stwWidth = 0.08f;
			qrWidth = 0.02f;
		} else
		{
			stwWidth = 0.02f;
			qrWidth = 0.02f;
		}

		//SET QTc (THIS WILL GIVE HALF QTc VALUE
		QTc = ((((300 * Mathf.Sqrt(60f / heartRate)) - 40f) / 2f) / 1000);

		if (rhythm == "af")
		{
			heartRateLower = heartRate - (heartRate / 5f);
			heartRateUpper = heartRate + (heartRate / 5f);
			heartRate = Random.Range(heartRateLower, heartRateUpper);
			pqWidth = (60f / heartRate) * 0.08f;
		}

		//NB: BELOW TECHNIQUE WON'T WORK FOR AF - RELIES ON FIXED LENGTH OF HEARTBEATS
		if (rhythm == "vt" || rhythm == "vf") {
			lengthy = qrWidth + rsWidth + stwWidth + 0.2f; //0.2 being length of T wave
		} else if (rhythm == "nsr") {
			if (bigeminy) {
				lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + 0.16f + (QTc * 3);
			} else {
				lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + (QTc * 2);
			}
		} else if (rhythm == "chb" || rhythm == "svt" || rhythm == "aflutter") {
			lengthy = qrWidth + rsWidth + stwWidth + (QTc*2f);
		}else if (rhythm == "svt") {
			lengthy = qrWidth + rsWidth + stwWidth + QTc;
		}
		else
		{
			lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + (QTc * 2);
		}
		gap = (60f / heartRate) - lengthy;

		//Not sure why, but bigeminy doesn't work unless it starts on the P wave
		if (bigeminy) {
			PQ ();
		} else {
			STsegment ();
		}
	}

	// Update is called once per frame
	void Update () {
		if ((Time.time - timeStamp) <= duration) {
			MoveDot ();
		} else {
			NextWave ();
		}
	}

	void MoveDot ()
	{
		GameObject ecgDot = GameObject.Find ("Current dot");

		Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, ((Time.time - timeStamp) / duration));

		//Describe half a sin wave for P and T waves
		if (wave == "p" || wave == "t") {
			if (rhythm == "sats") {
				bending = new Vector3 (0f, 3f, 0f);
				newPosition.y += bending.y * Mathf.Sin (Mathf.Pow (3, (Time.time - timeStamp)
					/ duration) * Mathf.PI) * -1f;
			} else {
				newPosition.y += bending.y * Mathf.Sin (Mathf.Clamp01 ((Time.time - timeStamp) / duration) * Mathf.PI);
			}
		}

		//Create a random interference pattern for AF
		else if (rhythm == "af") {
			if (wave == "tp") {
				float y = 0.05f;
				float targetTime = ((Time.time - afTimeStamp) / pDur);
				if (targetTime == 0f) {
					targetTime += 0.01f;
				}
				if (targetTime > 2f) {
					pDur = Random.Range (0.05f, 0.1f);
					targetTime = 0.01f;
					y = Random.Range (0.02f, 0.08f);
					bending = new Vector3 (0f, y, 0f);
					afTimeStamp = Time.time;
				}
				newPosition.y += bending.y * Mathf.Sin (targetTime * Mathf.PI);
			}
		} else if (rhythm == "aflutter") {
			//For creating flutter waves only during TP segment:
			if (wave == "tp") {
				int hR = (int)heartRate;
				int pS = 300 / hR;
				pS -= 1;
				float eachPdur = duration / (float)pS;
				bending = new Vector3 (0f, 0.3f * scale, 0f);
				float x = ((Time.time - aFlutterTimeStamp) / eachPdur);
				if (x >= 1f) {
					aFlutterTimeStamp = Time.time;
				}
				x = Mathf.Pow ((1f - x), 3f);
				newPosition.y += (bending.y * Mathf.Sin (x * Mathf.PI));
			}
		} else if (rhythm == "vf") {
			if (wave == "tp") {
				newPosition.y += bending.y * Mathf.Sin (((Time.time - timeStamp) / duration) * 2f * Mathf.PI);
			}
		} else if (rhythm == "vt") {
			if (wave == "tp" && heartRate > 160f) {
				float x = (((Time.time - timeStamp) / duration) * 2f);
				newPosition.y += bending.y * Mathf.Sin (x * Mathf.PI);
			}
		}

		if (chbPwave) {
			if ((Time.time - chbPwaveTimeStamp) <= 0.08f) {
				//Not using bending because can conflict if coincides with QRS complex
				Vector3 pBending = new Vector3 (0f, 0.5f, 0f);
				newPosition.y += pBending.y * Mathf.Sin (Mathf.Clamp01 ((Time.time - chbPwaveTimeStamp) / 0.08f) * Mathf.PI);
			} else {
				chbPwave = false;
			}
		}

		//Resets trace to left of screen
		if (Camera.main.WorldToScreenPoint (newPosition).x > (3f*Screen.width)/4f) {
			if (GameObject.Find("Old dot")) {
				GameObject oldEcgDot = GameObject.Find ("Old dot");
				Destroy (oldEcgDot);
			}
			ecgDot.name = "Old dot";
			newPosition = new Vector3 (screenLeftX, newPosition.y, newPosition.z);
			GameObject ecgDotTemp = (GameObject)Instantiate (dotPrefab, newPosition, Quaternion.identity);
			ecgDotTemp.name = "Current dot";
			startPosition.x -= screenWidth*1.5f;
			endPosition.x -= screenWidth*1.5f;
			satsScript.ResetTracer (newPosition);
			respScript.ResetTracer (newPosition);
			aLineScript.ResetTracer (newPosition);
		} 

		else {
			ecgDot.transform.position = newPosition;
		}
	}

	//CODE FOR SETTING UP THE NEXT WAVE IN THE SEQUENCE
	void NextWave()
	{
		endPosition.x += (Time.time - timeStamp - duration) * speed;
		if (rhythm == "af") {
			if (wave == "qr") {
				RS ();
			} else if (wave == "rs") {
				STwave ();
			} else if (wave == "stw") {
				STsegment ();
			} else if (wave == "sts") {
				T ();
			} else if (wave == "t") {
				TP ();
			} else if (wave == "tp" || wave == "pq") {
				QR ();
			}
		} else if (rhythm == "aflutter") {
			if (wave == "qr") {
				RS ();
			} else if (wave == "rs") {
				STwave ();
			} else if (wave == "stw") {
				STsegment ();
			} else if (wave == "sts") {
				T ();
			} else if (wave == "t") {
				TP ();
			} else if (wave == "tp" || wave == "pq") {
				QR ();
			}
		} else if (rhythm == "vt") {
			if (wave == "qr") {
				RS ();
			} else if (wave == "rs") {
				STwave ();
			} else if (wave == "stw") {
				STsegment ();
			} else if (wave == "sts") {
				T ();
			} else if (wave == "t") {
				TP (); 
			} else if (wave == "tp" || wave == "pq") {
				if (heartRate > 160f) {
					STsegment ();
				} else {
					QR ();
				}
			}
		} else if (rhythm == "vf") {
			if (changing) {
				STsegment ();
			} else {
				TP ();
			}
		} else if (rhythm == "nsr") {
			if (wave == "p") {
				PQ ();
			} else if (wave == "pq") {
				//Not done as "if (mobitzI && mobitzPQ==5) {" because fixes any problems if mobitzI and mobitzII true
				//(mobitzI gets preference)
				if (mobitzI) {
					if (mobitzPQ == 5) {
						mobitzPQ = 1;
						TP ();
					} else {
						QR ();
					}
				} else if (mobitzII && mobitzQRS == mobitzRatioToOne) {
					mobitzQRS = 0f;
					SetVariables ("tp", qrWidth + rsWidth + stwWidth + stsWidth + (QTc * 2) + gap);
				} else {
					QR ();
				}
			} else if (wave == "qr") {
				if (mobitzII) {
					mobitzQRS++;
				}
				RS ();
			} else if (wave == "rs") {
				STwave ();
			} else if (wave == "stw") {
				if (bigeminy && wideQRS) {
					T ();
				} else {
					STsegment ();
				}
			} else if (wave == "sts") {
				if (bigeminy && !wideQRS) {
					wideQRS = true;
					QR ();
				}else{
					T ();
				}
			} else if (wave == "t") {
				if (bigeminy) {
					wideQRS = false;
				}
				TP ();
			} else if (wave == "tp") {
				P ();
			}
		} else if (rhythm == "svt" || rhythm == "chb") {
			if (wave == "qr") {
				RS ();
			} else if (wave == "rs") {
				STwave ();
			} else if (wave == "stw") {
				STsegment ();
			} else if (wave == "sts") {
				T ();
			} else if (wave == "t") {
				TP ();
			} else if (wave == "tp" || wave == "pq") {
				QR ();
			}
		} else if (rhythm == "sats") {
			T ();
		}
	}

	//ALL THESE FUNCTIONS CREATE THE ECG CYCLE:

	void SetVariables(string wav, float dur) {
		//IMPORTANT - KEEPS TRACE RATE CONSTANT:
		startPosition = endPosition;
		GameObject ecgDot = GameObject.Find ("Current dot");
		ecgDot.transform.position = startPosition;

		if (cumulativeDeltaTime != -0.01f) {
			cumulativeDeltaTime += Time.time - (timeStamp + duration);
		}

		wave = wav;
		duration = dur;
		timeStamp = Time.time;
		if (wave == "qr")
		{
			if (wideQRS)
			{
				endPosition = new Vector3((startPosition.x + (duration * speed)), localY+(1f * scale), startPosition.z);
			}
			else {
				endPosition = new Vector3((startPosition.x + (duration * speed)), localY + (3f * scale), startPosition.z);
			}
		}
		else if (wave == "rs")
		{
			if (wideQRS)
			{
				endPosition = new Vector3((startPosition.x + (duration * speed)), localY + (-2f * scale), startPosition.z);
			}
			else {
				endPosition = new Vector3((startPosition.x + (duration * speed)), localY + (-1f * scale), startPosition.z);
			}
		}
		else {
			endPosition = new Vector3((startPosition.x + (duration * speed)), localY, startPosition.z);
		}
		Update ();
	}

	void P () {
		bending.y = 0.35f;
		SetVariables ("p", pWidth);
	}

	void PQ () {
		float localPQwidth = pqWidth;
		if (mobitzI) {
			localPQwidth = mobitzPQ * pqWidth;
			mobitzPQ++;
		}
		SetVariables ("pq", localPQwidth);

	}

	void QR () {
		if (bigeminy) {
			if (!wideQRS) {
				float actualHeartRate = 60f / (Time.time - hrDebugger);
				if (logHeartRate) {
					Debug.Log ("Actual heart rate: " + actualHeartRate);
				}
				hrDebugger = Time.time;
			}
			if (wideQRS)
			{
				stwWidth = 0.1f;
				qrWidth = 0.04f;
			} else
			{
				stwWidth = 0.02f;
				qrWidth = 0.02f;
			}
		} else {
			float actualHeartRate = 60f / (Time.time - hrDebugger);
			if (logHeartRate) {
				Debug.Log ("Actual heart rate: " + actualHeartRate);
			}
			hrDebugger = Time.time;
		}
		bending = new Vector3(0f, (0.35f * scale), 0f);
		SetVariables ("qr", qrWidth);

		//Am I repeating these equations for a particular reason?
		//(They are done in TP() function)
		//It seems I am, because fixing them fixed the heart rate in AF!
		if (rhythm == "af") {
			heartRate = Random.Range (heartRateLower, heartRateUpper);
			randomHeartRate = heartRate;
			pqWidth = (60f / heartRate) * 0.08f;
			QTc = ((((300 * Mathf.Sqrt (60f / heartRate)) - 40f) / 2f) / 1000);
			lengthy = (qrWidth + rsWidth + stwWidth) + (QTc * 2);
			gap = (60f / heartRate) - lengthy;
			satsScript.T (randomHeartRate);
			aLineScript.T (randomHeartRate);
		}else if (rhythm=="aflutter") {
			if (variableRhythm) {
				int var1 = Random.Range (2, 5);
				int hR = 300 / var1;
				heartRate = (float)hR;
				randomHeartRate = heartRate;
				pqWidth = (60f / heartRate) * 0.08f;
				QTc = ((((300 * Mathf.Sqrt (60f / heartRate)) - 40f) / 2f) / 1000);
				lengthy = (qrWidth + rsWidth + stwWidth) + (QTc * 2);
				gap = (60f / heartRate) - lengthy;
				satsScript.T (randomHeartRate);
				aLineScript.T (randomHeartRate);
			}
		} else if (!(bigeminy&&wideQRS)) {
			satsScript.T (heartRate);
			aLineScript.T (heartRate);
		}
	}

	void RS () {
		SetVariables ("rs", rsWidth);
	}

	void STwave () {
		SetVariables("stw", stwWidth);

	}

	void STsegment()
	{
		if (changing) {
			string previousRhythm = rhythm;
			bool wasVF = false;
			if (rhythm == "vf" && nextRhyhtm != "vf") {
				wasVF = true;
			}
			if (nextRhyhtm == "vt") {
				bending.y = 1.5f;
			}
			if (nextRhyhtm == "vf" && server) {
				previousRR = (float)respScript.respRate;
				previousSats = (float)satsScript.sats;
				previousMAP = aLineScript.MAP;
				respScript.ChangeResps (0f);
				aLineScript.ChangeBP (0f);
				satsScript.ChangeSats (-1f);
				heartRateText.text = "-";
			}
			rhythm = nextRhyhtm;
			respScript.rhythm = rhythm;
			satsScript.ecgRhythm = rhythm;
			if (wasVF && server) {
				aLineScript.ChangeBP (previousMAP / 120f);
				respScript.ChangeResps (previousRR / 40f);
				satsScript.ChangeSats ((previousSats - 70f) / 30f);
			}
			changing = false;
			if (rhythm != "vt" && !bigeminy) {
				wideQRS = false;
			}
			SetRhythm ();
			SetOtherFactors ();
			ChangeSlider ();
		} else if (changingRateOnly) {
			heartRate = nextHR;
			changingRateOnly = false;
			SetRhythm ();
			SetOtherFactors ();
		} else {
			if (rhythm == "vt") {
				if (firstPass) {
					GameObject ecgDot = GameObject.Find ("Current dot");
					endPosition = new Vector3 (ecgDot.transform.position.x - ((60f / heartRate) * speed * duration * 2f),
						localY, ecgDot.transform.position.z);
					firstPass = false;
				} else if (heartRate > 160f) {
					T ();
				} else {
					SetVariables ("sts", 0.01f);
				}
			} else if (rhythm == "vf") {
				//VF is just the TP segment on loop, but first call on instantiation is to STsegment(), so this is a redirect:
				TP ();
			} else if (rhythm == "sats") {
				if (firstPass) {
					GameObject ecgDot = GameObject.Find ("Current dot");
					endPosition = new Vector3 (ecgDot.transform.position.x - ((60f / heartRate) * speed * duration * 2f),
						localY, ecgDot.transform.position.z);
					firstPass = false;
				}
				T ();
			} else if (rhythm == "aflutter") {
				SetVariables ("sts", 0.5F * QTc);
			} else if (wideQRS) {
				T ();
			} else {
				if (firstPass) {
					GameObject ecgDot = GameObject.Find ("Current dot");
					if (heartRate > 60f) {
						endPosition = new Vector3 (ecgDot.transform.position.x - ((60f / heartRate) * speed * duration * 2f),
							localY, ecgDot.transform.position.z);
					} else {
						endPosition = new Vector3 (ecgDot.transform.position.x - (speed * duration * 2f),
							localY, ecgDot.transform.position.z);
					}
					firstPass = false;
				}
				SetVariables ("sts", 0.5F * QTc);
			}
		}
	}

	void T()
	{
		if (rhythm == "vt" || rhythm == "vf" || wideQRS) {
			if (heartRate > 160f) {
				TP ();
			} else {
				bending = new Vector3 (0f, (1.5f), 0f);
				if (wideQRS) {
					SetVariables ("t", QTc);
				} else {
					SetVariables ("t", 0.15f + gap);
				}
			}
		} else if (rhythm == "sats") {
			SetVariables("t", 1f);
		}
		else {
			SetVariables("t", 1.5F * QTc);
		}
	}

	void TP()
	{
		if (rhythm == "aflutter") {
			if (variableRhythm) {
				int var1 = Random.Range (2, 5);
				int hR = 300 / var1;
				heartRate = (float)hR;
			} else {
				heartRate = 75f;
			}
			heartRateText.text = heartRate.ToString ("0");
			randomHeartRate = heartRate;
			pqWidth = (60f / heartRate) * 0.08f;
			QTc = ((((300 * Mathf.Sqrt (60f / heartRate)) - 40f) / 2f) / 1000);
			lengthy = (qrWidth + rsWidth + stwWidth) + (QTc * 2);
			gap = (60f / heartRate) - lengthy;
			aFlutterTimeStamp = Time.time;
			SetVariables ("tp", gap - cumulativeDeltaTime);
		} else if (rhythm == "af") {
			heartRate = Random.Range (heartRateLower, heartRateUpper);
			randomHeartRate = heartRate;
			pqWidth = (60f / heartRate) * 0.08f;
			QTc = ((((300 * Mathf.Sqrt (60f / heartRate)) - 40f) / 2f) / 1000);
			lengthy = (qrWidth + rsWidth + stwWidth) + (QTc * 2);
			gap = (60f / heartRate) - lengthy;
			heartRateText.text = heartRate.ToString ("0");
			afTimeStamp = Time.time;
			bending = new Vector3 (0f, (0.03f * scale), 0f);
			SetVariables ("tp", gap - cumulativeDeltaTime);
		} else if (rhythm == "vf") {
			float y = (Random.Range (0.01f, 2f)) * scale;
			bending = new Vector3 (0f, y, 0f);
			SetVariables ("tp", 0.2f);
		} else if (rhythm == "svt") {
			lengthy = qrWidth + rsWidth + stwWidth + (QTc * 2);
			gap = (60f / heartRate) - lengthy;
			if (gap <= 0) {
				QR ();
			} else {
				SetVariables ("tp", gap - cumulativeDeltaTime);
			}
		} else if (rhythm == "vt") {
			if (heartRate > 160f) {
				float actualHeartRate = 60f / (Time.time - hrDebugger);
				if (logHeartRate) {
					Debug.Log ("Actual heart rate: " + actualHeartRate);
				}
				hrDebugger = Time.time;
				if (torsades) {
					if (bending.y >= 2f) {
						torsadesUp = false;
					} else if (bending.y <= 0.5f) {
						torsadesUp = true;
					}
					if (torsadesUp) {
						bending.y += 0.5f;
					} else {
						bending.y -= 0.5f;
					}
				} else {
					bending.y = 1.5f;
				}
				//These are here because functions not triggered in QR() function as usual:
				satsScript.T (heartRate);
				aLineScript.T (heartRate);

				SetVariables ("tp", (60f / heartRate) - cumulativeDeltaTime);
			} else {
				bending.y = 1.5f;
				QR ();
			}
			satsScript.T (heartRate);
			aLineScript.T (heartRate);

		}else if (rhythm=="nsr"){
			if (heartRate > 130f) {
				pqWidth = 0.06f * (80f / heartRate);
			} else if (heartRate > 80f) {
				pqWidth = 0.1f * (80f / heartRate);
			}
			SetVariables("tp", gap - cumulativeDeltaTime);
		} else {
			SetVariables("tp", gap - cumulativeDeltaTime);
		}
		cumulativeDeltaTime = 0f;
	}

	IEnumerator CHBsetter() {
		yield return new WaitForSeconds (0.75f);
		if (rhythm == "chb") {
			chbPwaveTimeStamp = Time.time;
			chbPwave = true;
		}
		StartCoroutine (CHBsetter ());
	}
}
*/