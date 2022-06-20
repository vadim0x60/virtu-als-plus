using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {
    /*
    THIS CODE MAKES THE DEFIB WORK.
    IT CONNECTS TO THE HUB LIKE ALL THE OTHER SCRIPTS, BUT IT ALSO
    CONNECTS TO OBJECTS ASSOCIATED WITH THE DEFIB, E.G. THE SHOCK BUTTON

	I've ended up having to scatter "if (defibReady) {...}" around because
	the monitor relies upon the defib being active. However, this can cause
	bugs because the shock, pacing and sync functions could still work when
	the defib is supposed to be inactive.
    */
	public bool debugging = true;

	private Vector3 bending = new Vector3(0f,0.35f,0f);
	private Vector3 startPosition;
	private Vector3 endPosition;
	private Vector3 screenLeft;
	private Vector3 screenRight;
    private Vector3 lastPosDefib;
    private Vector3 lastPosMonitor;
    
    public float heartRate = 80f;
    public float scale = 1f;
    public float MAP = 0f;
    public float stripLength = 1f;
    public float aftershockMAP = 70f;
    public float paceRate = 70f;
    private float paceCurrent = 0.1f;
    private float heartRateUpper;
    private float heartRateLower;
	private float actualHeartRateStamp = 0f;
	private float actualHeartRate = 0f;
	private float actualHeartRateAdder = -1f;
    private float chargeTime = 0f;
    private float pDur = 0.01f;
    private float localY = 0f;
    private float oldY = 0f;
    private float duration;
    private float pWidth = 0.08f;
    private float pqWidth = 0.1f;
    private float qrWidth = 0.02f;
    private float rsWidth = 0.02f;
    private float stwWidth = 0.02f;
    private float stsWidth;
    private float tWidth;
    private float scrnLft;
	private float scrnRt;
	private float lastScreenWidth;
	private float screenWidth;
	private float traceRate;
	private float startTime;
	private float timeStamp;
	private float cycles = 0f;
	private float QTc;
	private float lengthy;
	private float gap;
    private float sysBP = 120f;
    private float diasBP = 80f;
    private float cloneNo = 1f;
    private float lastSystole = 0f;
    private float lastPacingSpike = 0f;
	private float mobitzPQ = 1f;
	private float mobitzRatioToOne = 2f;
	private float mobitzQRS = 0f;
	private float cumulativeDeltaTime = -0.01f;
	private float aFlutterTimeStamp = 0f;
	private float hrDebugger = 0f;
	private float chbPwaveTimeStamp = 0f;
	private float nextHR = 60f;
	public float screenToMonitorX = 0f;
	public float screenToMonitorY = 0f;

    public GameObject mainHub;
	public GameObject spriteRen;
	public GameObject monitorSpriteRen;
    public GameObject defibScreen;
	public GameObject monitor;
    public GameObject spike;
    public GameObject syncMark;
    public GameObject defibCanvas;
    public GameObject testScripts;
	public GameObject defibOnButton;
    public GameObject shockButton;
    public GameObject chargeButton;
    public GameObject syncButton;
	public GameObject paceButton;
	public GameObject pacePauseButton;
    public GameObject heartRateLabel;
    public GameObject chargedLight;
    public GameObject chargingText;
    public GameObject pacingText;
    public GameObject energyText;
    public GameObject paceLight;
    public GameObject syncLight;
    private GameObject syncer;
    private GameObject spiker;
    private GameObject syncOnOff;
	public GameObject monitorSats;
	public GameObject monitorALine;
	public GameObject monitorResp;
	public GameObject monitorHeartRateText;

    private int energyListPosition=1;
    private int[] energyList;
   
    public Hub hub;
	public Sats satsScript;
	public ALine aLineScript;
	public Resp respScript;
	public Networker networker;

    private Patient patient;

    public string afterShockRhythm = "af";
    public string rhythm = "nsr";
    private string wave;
	private string nextRhyhtm = "nsr";
    
	public bool variableRhythm = true;
	public bool wideQRS = false;
    public bool noShock = false;
    private bool kill = false;
    private bool shock = false;
    private bool sync = false;
    public bool paused = false;
    private bool shockFail = false;
    public bool charging = false;
    public bool charged = false;
    public bool pacing = false;
    private bool pacingComplex = false;
    private bool pacingSpike = false;
    private bool previousWideQRS = false;
    public bool capture = false;
    public bool safeCapture = false;
	public bool mobitzI = false;
	public bool mobitzII = false;
	public bool torsades = false;
	private bool torsadesUp = true;
	public bool bigeminy = false;
	private bool changing = false;
	private bool chbPwave = false;
	private bool changingRateOnly = false;
	public bool defibReady = false;
	public bool remoteConnected = false;
	private bool bigeminyWideSwitch = false;
	public bool monitorPadsOn = false;
	public bool monitorPadsToComeOn = false;
	public bool satsScriptToComeOn = false;
	public bool aLineScriptToComeOn = false;
	public bool respScriptToComeOn = false;
	public bool adenosine = false;
	private bool remoteRhythmChange = false;

    public Material greenMat;

    List<LineItem> lineList;

    // Use this for initialization
    void Start () {
        if (debugging)
        {
            Debug.Log("Control starting");
        }

        patient = hub.patient;
        MAP = hub.MAP;

        scale = defibScreen.GetComponent<Renderer>().bounds.size.y / 15f;
        bending = new Vector3(bending.x, (bending.y * scale), bending.z);

        spriteRen=(GameObject)Instantiate (spriteRen, transform.position, 
			Quaternion.identity);
        spriteRen.name = "Clone0";
        spriteRen.transform.parent = defibScreen.transform;
        if (debugging)
        {
            Debug.Log("Clone0 pos: " + spriteRen.transform.position);
        }

		screenToMonitorX = defibScreen.transform.position.x - monitor.transform.position.x;
		//Other way around because monitor is above screen:
		screenToMonitorY =  monitor.transform.position.y - defibScreen.transform.position.y;
		//Allowing for fact that monitor screen is scaled up from defib:
		//(Original scale was 7, so need to add half of the difference between new scale and 7)
		screenToMonitorX += 1.5f;
		screenToMonitorY += 4f;

		spriteRen.transform.localPosition = new Vector3(-0.48f, 0f, -1);
		localY = spriteRen.transform.position.y;
		spriteRen.transform.position = new Vector3(spriteRen.transform.position.x,
			spriteRen.transform.position.y, defibScreen.transform.position.z - 0.01f);
		endPosition = spriteRen.transform.position;
		scrnLft = endPosition.x;
        lastPosDefib = spriteRen.transform.position;

		screenWidth = defibScreen.GetComponent<Renderer>().bounds.size.x;
		traceRate = (screenWidth*0.96f) / stripLength;
		lineList = new List<LineItem>();

		Vector3 monitorSpriteRenPos = new Vector3 (transform.position.x - screenToMonitorX,
			transform.position.y + screenToMonitorY, monitor.transform.position.z - 0.01f);

		monitorHeartRateText.transform.position = new Vector3 (monitorHeartRateText.transform.position.x,
			transform.position.y + screenToMonitorY + 0.5f, monitor.transform.position.z - 0.01f);

		monitorSpriteRen = (GameObject)Instantiate (spriteRen, monitorSpriteRenPos, 
			Quaternion.identity);
        lastPosMonitor = monitorSpriteRenPos;
		monitorSpriteRen.name = "MonitorClone0";
		monitorSpriteRen.transform.parent = defibScreen.transform;

		chargeButton.SetActive (true);
		syncButton.SetActive (true);
		monitorSats.SetActive (true);
		monitorALine.SetActive (true);
		monitorResp.SetActive (true);
		monitorHeartRateText.SetActive (true);
		energyList = new int[] { 120, 150, 200, 300, 360 };

		StartCoroutine (CHBsetter ());

        startTime = Time.time;

		SetBP();

		SetRhythm();

		SetOtherFactors();
	}

	public void RemoteConnected () {

	}

	public void RemoteChangeECG (string incomingRhythm) {
		if (debugging) {
			Debug.Log ("Control receiving rhythm: " + incomingRhythm);
		}
		MakeSenseOfRhythmVoid (incomingRhythm);
		remoteRhythmChange = true;
		changing = true;
	}

	public string MakeSenseOfRhythm(string incomingRhythm) {
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
			bigeminyWideSwitch = false;
			nextRhyhtm = "nsr";
			if (heartRate < 20f) {
				heartRate = 20f;
			}
			if (heartRate > 60f) {
				heartRate = 60f;
			}
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
		return nextRhyhtm;
	}

	void MakeSenseOfRhythmVoid (string incomingRhythm) {
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
			bigeminyWideSwitch = false;
			nextRhyhtm = "nsr";
			if (heartRate < 20f) {
				heartRate = 20f;
			}
			if (heartRate > 60f) {
				heartRate = 60f;
			}
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
	}

	public void RemoteChangeHeartRate (float newHeartRate) {
		nextHR = newHeartRate;
		changingRateOnly = true;
	}
    
    //SETS HEARTRATE TO PATIENT'S HEARTRATE AND ENSURES IS WITHIN ACCEPTED VALUES
    void SetRhythm()
    {

		if (!remoteRhythmChange) {
			if (debugging) {
				Debug.Log ("Locally changing rhythm to: " + rhythm);
			}
			heartRate = hub.NextHeartRate ();
			rhythm = MakeSenseOfRhythm (hub.rhythm);
		} else {
			if (debugging) {
				Debug.Log ("Remotely changing rhythm to: " + rhythm);
			}
			remoteRhythmChange = false;
		}

		actualHeartRate = 0f;
		actualHeartRateAdder = 0f;
		if (rhythm != "nsr" && rhythm != "af" && rhythm != "aflutter" &&
		    rhythm != "svt" && rhythm != "vt" && rhythm != "vf" && rhythm != "chb"
			&& debugging) {
			Debug.Log ("The defib does not recognise the rhythm: " + rhythm);
		} else if (rhythm == "nsr") {
			if (mobitzI || mobitzII) {
				if (heartRate < 20f) {
					heartRate = 20f;
				}
				if (heartRate > 80f) {
					heartRate = 80f;
				}
				wideQRS = false;
			} else {
				//COMMENTED OUT SECTIONS BELOW ARE FOR RANDOMLY CHANGING RHYTHM
				if (heartRate < 20f) {
					heartRate = 20f;
				}
				if (heartRate > 150f) {
					heartRate = 150f;
				}
			}
			variableRhythm = false;
		}
        else if (rhythm == "af")
        {
			if (heartRate < 20f) {
				heartRate = 20f;
			}
			if (heartRate > 100f) {
				heartRate = 100f;
			}
            //heartRate = Random.Range(20f, 150f);
            heartRateUpper = Random.Range(heartRate + (heartRate / 10), heartRate + (heartRate / 3));
            heartRateLower = Random.Range(heartRate - (heartRate / 10), heartRate - (heartRate / 3));
            wideQRS = false;
        }
        else if (rhythm == "aflutter")
        {
            heartRate = 75f;
            //RANDOMLY SETS RHYTM TO FIXED OR VARIABLE BLOCK
            bool[] trueFalse = new bool[] { true, false };
            int randomN = Random.Range(0, 2);
            variableRhythm = trueFalse[randomN];

            //MAKES COMPLEXES NARROW IF BLOCK IS VARIABLE
            //(OTHERWISE IT CAN LOOK LIKE VT)
            if (variableRhythm)
            {
                wideQRS = false;
            }
        }
        else if (rhythm == "svt")
        {
            if (heartRate < 120f || heartRate > 200f)
            {
                heartRate = Random.Range(120f, 200f);
            }
            //heartRate = Random.Range(120f, 200f);
            variableRhythm = false;
            wideQRS = false;
        }
        else if (rhythm == "vt" || rhythm == "vf")
        {
            wideQRS = true;
            variableRhythm = false;
			if (torsades && (heartRate < 161f || heartRate > 250f)) {
				heartRate = Random.Range (161f, 250f);
			} else if (heartRate < 150f || heartRate > 250f) {
				heartRate = Random.Range (150f, 250f);
			}
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

        if (rhythm=="vf")
        {
            heartRate = 200f;
        }

        //For reseting QRS width if pacer switched off
        previousWideQRS = wideQRS;
    }

    /*SETS CORRECTED QTc, SETS GAP BETWEEN BEATS ACCORDING TO HEART RATE, SETS INITIAL HEART RATE FOR AF
    (AF WILL VARY 20% ABOVE AND BELOW SET HEART RATE*/
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
			lengthy = qrWidth + rsWidth + stwWidth;
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

    //SETS THE BP TO 0 IF VF, OTHERWISE GETS patient.MAP
    void SetBP ()
    {
        string dias="0";
        string sys = "0";
        string map = "0";

        if (rhythm == "vf")
        {
            MAP = 0f;
        }
        else
        /*MAP will be set automatically by Hub with each rhythm change
        Code below is for setting a random MAP*/
        {
            /*float zeroChance = Random.Range(0f, 2f);
            if (zeroChance == 1)
            {
                MAP = 0f;
            }
            else
            {
                MAP = Random.Range(0f, 150f);
                while (MAP > 40 && MAP < 65)
                {
                    MAP = Random.Range(0f, 150f);
                }
                if (MAP < 10)
                {
                    MAP = 0f;
                }
            }*/
            MAP = hub.MAP;

			//This if clause safety-nets the start-up process. SetBP() is called in the first frame of Control.
			//These scripts are further down the start-up cascade than Control, so it cannot be assumed
			//they have started by then.
			if (aLineScript.started && respScript.started && satsScript.started) {
				aLineScript.ClientChangeBP (MAP / 120f);
				if (debugging) {
					Debug.Log ("A-line set to MAP: " + MAP);
				}
			}
        }

        //SET THE ACTUAL BP BASED ON THE MAP - SAVES HAVING TO SET DIASTOLIC AND SYSTOLIC VALUES EACH TIME
        diasBP = Random.Range(MAP * 0.5f, MAP * 0.9f);
        if (diasBP < 10)
        {
            diasBP = 0f;
        }
        sysBP = Random.Range(MAP * 1.1f, MAP * 1.5f);
        if (sysBP < 10)
        {
            sysBP = 0f;
        }

        //HAD TO FAFF WITH THIS BECAUSE "0f" WASN'T SHOWING UP IN TEXT
        if (diasBP != 0)
        {
            dias = string.Format("{0:#}", diasBP);
        }
        if (sysBP != 0)
        {
            sys = string.Format("{0:#}", sysBP);
        }
        if (MAP != 0)
        {
            map = string.Format("{0:#}", MAP);
        }
    }

    //PROVIDES A FUNCTION FOR THE SYNC BUTTON TO FEED INTO
    public void Sync()
    {
		if (defibReady && hub.clickable) {
			if (!pacing) {
				sync = !sync;
				syncLight.active = !syncLight.active;
			}
		}
    }

    //CREATES THE SYNC MARKER
    void SyncMarker()
    {
        if (spriteRen.transform.localPosition.y != 0)
        {
            Vector3 syncPos = new Vector3(spriteRen.transform.position.x,
                localY + (5f * scale), spriteRen.transform.position.z);
            syncer = (GameObject)Instantiate(syncMark, syncPos,
            Quaternion.identity);
            LineItem liner = new LineItem(syncer, Time.time);
            lineList.Add(liner);
        }
    }

    /*FOR TESTING PURPOSES
    PROVIDES A PUBLIC FUNCTION TO TOGGLE WIDE QRS COMPLEX*/
    public void Wide()
    {
        wideQRS = !wideQRS;
        if (!wideQRS)
        {
            stwWidth = 0.02f;
        }
        else
        {
            stwWidth = 0.12f;
        }
    }

    //SETS THE QRS COMPLEX WIDTH (TRUE = WIDE)
    void Wide(bool set)
    {
        wideQRS = set;
        if (!wideQRS)
        {
            stwWidth = 0.02f;
        }
        else
        {
            stwWidth = 0.12f;
        }
         }

    // Update is called once per frame
    void Update () {
        if (!paused)
        {
            
            //Networker is automatically activated and deactivated with connection
            if (networker.isActiveAndEnabled && !remoteConnected) {
				remoteConnected = true;
				hub.remoteConnected = true;
			} else if (!networker.isActiveAndEnabled && remoteConnected) {
				remoteConnected = false;
				hub.remoteConnected = false;
			}

            if (charging)
            {
                chargeTime += Time.deltaTime;
                float chargePercent = (chargeTime * 100f) / 3.8f;
                chargingText.GetComponent<TextMesh>().text = "Charging: " +
                    chargePercent.ToString("0") + "%\n" +
                    "Press \"CHARGE\" to dump";
                if (chargeTime>=3.8f)
                {
                    charging = false;
                    charged = true;
                    chargedLight.active = true;
                    shockButton.active = true;
                    chargingText.GetComponent<TextMesh>().text = "Charged!\n" +
                        "Press \"CHARGE\" to dump";
                }
            }
			if (rhythm != "vf"  && (!adenosine || (pacing && capture)))
            {
                if (paceRate > heartRate && paceCurrent > patient.pacingThreshold && pacing)
                {
					if (defibReady) {
						heartRateLabel.GetComponent<TextMesh> ().text = "Heart rate: " + string.Format ("{0:#}", paceRate);
					}
					if (monitorPadsOn || (satsScript.satsOn && monitorHeartRateText.GetComponent<TextMesh> ().text != "WAIT")) {
						monitorHeartRateText.GetComponent<TextMesh> ().text = string.Format ("{0:#}", paceRate);
					}
                }
                else
                {
                    if (actualHeartRate == 0f) {
                        if (defibReady) {
							heartRateLabel.GetComponent<TextMesh> ().text = "Heart rate: " + string.Format ("{0:#}", heartRate);
						}
						if (monitorPadsOn || (satsScript.satsOn && monitorHeartRateText.GetComponent<TextMesh> ().text != "WAIT")) {
							monitorHeartRateText.GetComponent<TextMesh> ().text = string.Format ("{0:#}", heartRate);
						}
					} else {
                        if (defibReady) {
							heartRateLabel.GetComponent<TextMesh> ().text = "Heart rate: " + string.Format ("{0:#}", actualHeartRate);
                        }
						if (monitorPadsOn || (satsScript.satsOn && monitorHeartRateText.GetComponent<TextMesh> ().text != "WAIT")) {
							monitorHeartRateText.GetComponent<TextMesh> ().text = string.Format ("{0:#}", actualHeartRate);
						}
					}
                }
            }
            else
            {
				if (defibReady) {
					heartRateLabel.GetComponent<TextMesh> ().text = "Heart rate: -";
				}
				if (monitorPadsOn || (satsScript.satsOn && monitorHeartRateText.GetComponent<TextMesh> ().text != "WAIT")) {
					monitorHeartRateText.GetComponent<TextMesh> ().text = "-";
				}
            }
            Vector3 tmpPos = spriteRen.transform.localPosition;
            //Debug.Log(tmpPos.y);
            //Debug.Log("Shock=" + shock + ", Sync=" + sync + "tmpPos.y=" + tmpPos.y);
            //Debug.Log(localY+" "+spriteRen.transform.position.y);
            if (shock && !sync && charged) //&& (spriteRen.transform.position.y== localY))
            {
                //Debug.Log("Shocking");
                Shock();
            }
            else {
                if (tmpPos.x < 0.48)
                {
                    if ((Time.time - timeStamp) <= duration)
                    {
                        Updater01();
                    }
                    else {
                        Updater02();
                    }
                }
                else {
                    Updater03();
                }
            }
        }
	}

    //CODE FOR MOVING THE TRACER
    void Updater01 ()
    {
        Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, ((Time.time - timeStamp) / duration));
        lastPosDefib = spriteRen.transform.position;
        lastPosMonitor = monitorSpriteRen.transform.position;

        if (pacing)
        {
            if (60f/paceRate <= Time.time - lastSystole)
            {
                if (60f / paceRate <= Time.time - lastPacingSpike)
                {
                    pacingSpike = false;
                }
                if ((wave == "p" || wave == "pq" || wave == "tp") && pacingSpike == false)
                {
                    DoPace();
                }
            }
        }
        if (wave == "p" || wave == "t")
        {
            currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp) / duration) * Mathf.PI);
        }
        else if (rhythm == "af")
        {
            if (wave == "tp")
            {
                float y = 0.05f * scale;
                pDur = 0.01f;
                float targetTime = (Time.time - timeStamp) / (pDur * traceRate);
                if (targetTime > 2)
                {
                    pDur = Random.Range(0.005f, 0.01f);
                    y = (Random.Range(0.01f, 0.05f)) * scale;
                    bending = new Vector3(0f, y, 0f);
                    //Debug.Log ("pDur: " + pDur + " y: " + y);
                }
                currentPos.y += bending.y * Mathf.Sin(((Time.time - timeStamp) / ((0.01f / scale) * traceRate)) * Mathf.PI);
            }
        }
        else if (rhythm == "aflutter")
        {
            if (wave == "tp")
            {
                bending = new Vector3(0f, 0.5f * scale, 0f);
				float x = ((Time.time - aFlutterTimeStamp) / 0.2f);
				if (x >= 1f) {
					aFlutterTimeStamp = Time.time;
				}
				currentPos.y += bending.y * Mathf.Sin(Mathf.Pow((1f-x),3f) * Mathf.PI);
            }
        }
        else if (rhythm == "vf")
        {
			if (wave == "tp") {
				currentPos.y += bending.y * Mathf.Sin (((Time.time - timeStamp) / duration) * 2f * Mathf.PI);
			}
		} else if (rhythm == "vt") {
			if (wave == "tp" && heartRate > 160f) {
				float x = (((Time.time - timeStamp) / duration) * 2f);
				currentPos.y += bending.y * Mathf.Sin (x * Mathf.PI);
			}
		}

		if (chbPwave) {
			if ((Time.time - chbPwaveTimeStamp) <= 0.08f) {
				//Not using bending because can conflict if coincides with QRS complex
				Vector3 pBending = new Vector3 (0f, 0.2f, 0f);
				currentPos.y += pBending.y * Mathf.Sin (Mathf.Clamp01 ((Time.time - chbPwaveTimeStamp) / 0.1f) * Mathf.PI);
			} else {
				chbPwave = false;
			}
		}

        bool drawing = false;

        spriteRen.transform.position = currentPos;
        //There was a bug where a line was being drawn across the screen.
        //Following if/then is to safeguard against that:
        if ((lastPosDefib.x < currentPos.x) && ((currentPos.x - lastPosDefib.x) < (screenWidth / 2f)))
        {
            DrawLine(lastPosDefib, currentPos, Color.green);
            drawing = true;
        }

        currentPos.x -= screenToMonitorX;
		currentPos.y += screenToMonitorY;
		currentPos.z = monitorSpriteRen.transform.position.z;

		if (monitorPadsOn) {
			monitorSpriteRen.transform.position = currentPos;
            if (drawing)
            {
                DrawLine(lastPosMonitor, currentPos, Color.green);
            }
		}

        if (lineList.Count > 0)
        {
            for (int i = lineList.Count - 1; i >= 0; --i)
            {
                if ((lineList[i].f + (stripLength - 0.1f)) <= Time.time)
                {
                    Destroy(lineList[i].g);
                    lineList.RemoveAt(i);
                }
            }
        }
        
    }

    //CODE FOR SETTING UP THE NEXT WAVE IN THE SEQUENCE
    void Updater02()
    {
        endPosition.x += (Time.time - timeStamp - duration) * traceRate;
        if (rhythm == "af" || rhythm == "aflutter")
        {
            if (wave == "qr")
            {
                RS();
            }
            else if (wave == "rs")
            {
                STwave();
            }
            else if (wave == "stw")
            {
                STsegment();
            }
            else if (wave == "sts")
            {
                T();
            }
            else if (wave == "t")
            {
                TP();
            }
            else if (wave == "tp" || wave == "pq")
            {
                QR();
            }
        }
        else if (rhythm == "vt")
        {
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
				if (heartRate >= 160f) {
					STsegment ();
				} else {
					QR ();
				}
			}
        }
        else if (rhythm == "vf")
        {
			if (changing) {
				STsegment ();
			} else {
				TP ();
			}
        }
		else if (rhythm == "nsr") {
			if (bigeminy && !bigeminyWideSwitch) {
				if (wave == "p") {
					PQ ();
				} else if (wave == "pq") {
					QR ();
				} else if (wave == "qr") {
					RS ();
				} else if (wave == "rs") {
					STwave ();
				} else if (wave == "stw") {
					STsegment ();
				} else if (wave == "sts") {
					bigeminyWideSwitch = true;
					wideQRS = true;
					QR ();
				} else if (wave == "t") {
					wideQRS = false;
					TP ();
				} else if (wave == "tp") {
					P ();
				} else {
					//Catch any transition errors
					PQ ();
				}
			} else if (bigeminy && bigeminyWideSwitch) {
				if (wave == "sts") {
					QR ();
				} else if (wave == "qr") {
					RS ();
				} else if (wave == "rs") {
					STwave ();
				} else if (wave == "stw") {
					T ();
				} else if (wave == "t") {
					bigeminyWideSwitch = false;
					wideQRS = false;
					TP ();
				} else {
					//Catch any transition errors
					bigeminyWideSwitch = false;
					wideQRS = false;
					PQ ();
				}
			} else if (wave == "p") {
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
				STsegment ();
			} else if (wave == "sts") {
				T ();
			} else if (wave == "t") {
				TP ();
			} else if (wave == "tp") {
				P ();
			}
        }
		else if (rhythm == "svt" || rhythm == "chb")
        {
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
        }
    }

    //CODE FOR RESETTING THE TRACER TO THE LEFT OF SCREEN
    void Updater03 ()
	{
		Vector3 currentPos = new Vector3 (scrnLft, spriteRen.transform.position.y, spriteRen.transform.position.z);
		endPosition.x -= screenWidth * 0.96f;
		startPosition.x -= screenWidth * 0.96f;
        spriteRen.transform.position = currentPos; /*spriteRen = (GameObject)Instantiate (spriteRen, currentPos,
			Quaternion.identity);
		spriteRen.name = "Clone" + string.Format ("{0:#}", cloneNo + 1f);
		spriteRen.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);*/

		currentPos.x -= screenToMonitorX;
		currentPos.y += screenToMonitorY;
		currentPos.z = monitorSpriteRen.transform.position.z;

        monitorSpriteRen.transform.position = currentPos;
		/*monitorSpriteRen = (GameObject)Instantiate (spriteRen, currentPos,
			Quaternion.identity);
		monitorSpriteRen.name = "MonitorClone" + string.Format ("{0:#}", cloneNo + 1f);
		monitorSpriteRen.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);*/
		if (monitorPadsToComeOn) {
			monitorPadsOn = true;
			monitorPadsToComeOn = false;
		}

        //Presumably obsolete? Haven't deleted just because not sure.
		GameObject isDes = GameObject.Find ("Clone" + string.Format ("{0:#}", cloneNo - 1));
		if (isDes) {
			Destroy (isDes);
			Destroy (GameObject.Find ("MonitorClone" + string.Format ("{0:#}", cloneNo - 1)));
		}

		cloneNo++;

		spriteRen.transform.parent = defibScreen.transform;
		if (satsScriptToComeOn) {
			satsScript.satsOn = true;
			satsScriptToComeOn = false;
		}
		if (aLineScriptToComeOn) {
			aLineScript.aLineOn = true;
			aLineScriptToComeOn = false;
		}

		if (respScriptToComeOn && respScript.doubleXspeed == 1) {
			respScript.doubleXspeed = 3;
			respScript.respOn = true;
			respScriptToComeOn = false;
		}
		if (satsScript.satsOn) {
			satsScript.ResetTracer (currentPos, cloneNo);
		}
		if (aLineScript.aLineOn) {
			aLineScript.ResetTracer (currentPos, cloneNo);
		}
		if (respScript.respOn) {
			respScript.ResetTracer (currentPos, cloneNo);
		}

		Update ();
	}

    //ALL THESE FUNCTIONS CREATE THE ECG CYCLE:

    void SetVariables(string wav, float dur) {
		if (cumulativeDeltaTime != -0.01f) {
			cumulativeDeltaTime += Time.time - (timeStamp + duration);
		}

        //IMPORTANT - KEEPS TRACE RATE CONSTANT:
        startPosition = endPosition;
        spriteRen.transform.position = startPosition;

        lastPosMonitor = monitorSpriteRen.transform.position;
        Vector3 currentPos = Vector3.zero;
        currentPos.x = startPosition.x - screenToMonitorX;
        currentPos.y = startPosition.y + screenToMonitorY;
        currentPos.z = monitorSpriteRen.transform.position.z;

        monitorSpriteRen.transform.position = currentPos;
        if ((lastPosDefib.x < startPosition.x) && ((startPosition.x - lastPosDefib.x) < (screenWidth / 2f)))
        {
            DrawLine(lastPosDefib, startPosition, Color.green);
        }
        if (monitorPadsOn)
        {
            DrawLine(lastPosMonitor, currentPos, Color.green);
        }

        wave = wav;
		duration = dur;
		timeStamp = Time.time;

		if (adenosine && wave != "p" && wave != "tp" && !(pacing && capture)) {
			endPosition = new Vector3((startPosition.x + (duration * traceRate)), localY, startPosition.z);
			bending = new Vector3 (0f, 0f, 0f);
		} 
		else if (wave == "qr")
        {
            if (wideQRS)
            {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), localY+(1f * scale), startPosition.z);
            }
            else {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), localY + (4f * scale), startPosition.z);
            }
        }
		else if (wave == "rs")
        {
            if (wideQRS)
            {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), localY + (-4f * scale), startPosition.z);
            }
            else {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), localY + (-1.5f * scale), startPosition.z);
            }
        }
        else {
            endPosition = new Vector3((startPosition.x + (duration * traceRate)), localY, startPosition.z);
        }
        Update ();
	}

	void P () {
		bending.y = 0.35f*scale;
		SetVariables ("p", pWidth);
	}

	void PQ () {
		float localPQwidth = pqWidth;
		if (mobitzI) {
			if (mobitzPQ > 4f) {
				mobitzPQ = 1f;
			}
			localPQwidth = mobitzPQ * pqWidth;
			mobitzPQ++;
		}
		SetVariables ("pq", localPQwidth);
	}

	void QR () {
		if (bigeminy) {
			if (!wideQRS) {
				actualHeartRate = 60f / (Time.time - hrDebugger);
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
			actualHeartRate = 60f / (Time.time - hrDebugger);
			hrDebugger = Time.time;
		}

		if (!(bigeminy&&wideQRS) || pacing) {
			satsScript.T (heartRate);
			if (pacing && capture) {
				aLineScript.T (paceRate);
			} else {
				aLineScript.T (heartRate);
			}
		}

        SetVariables ("qr", qrWidth);
		if (defibReady && (!adenosine || (pacing && capture))) {
			hub.PlaySound ("Beep");
		}
    }

	void RS () {
		SetVariables ("rs", rsWidth);
	}

	void STwave () {
        //RESET PACING SETTINGS
        wideQRS = previousWideQRS;
        pacingComplex = false;
		if (!adenosine) {
			lastSystole = Time.time;
		}

        if (shock && sync && charged)
        {
            Shock();
        }
        else if (!shock && sync)
        {
            SyncMarker();
            SetVariables("stw", stwWidth);
        }
        else
        {
            SetVariables("stw", stwWidth);
        }
        
	}

    void STsegment()
    {
		if (changing) {
			if (debugging) {
				Debug.Log ("Control changing");
			}
			string previousRhythm = rhythm;
			bool wasVF = false;
			if (rhythm == "vf" && nextRhyhtm != "vf") {
				wasVF = true;
			}
			if (nextRhyhtm == "vt") {
				bending.y = 1.5f;
			}
			rhythm = nextRhyhtm;
			respScript.rhythm = rhythm;
			satsScript.ecgRhythm = rhythm;
			changing = false;
			if (rhythm != "vt" && !bigeminy) {
				wideQRS = false;
			}
			SetRhythm ();
			SetOtherFactors ();
			if (hub.shockOrNotWaiting) {
				hub.ShockOrNotResume ();
			} else if (hub.rhythmPracticeWaiting) {
				hub.RhythmPracticeResume ();
			}
		} else if (changingRateOnly) {
			heartRate = nextHR;
			changingRateOnly = false;
			SetOtherFactors ();
		} else {
			if (wideQRS) {
				T ();
			} else if (rhythm == "vf") {
				//VF is just the TP segment on loop, but first call on instantiation is to STsegment(), so this is a redirect:
				TP ();
			} else if (rhythm == "svt") {
				T ();
			} else {
				SetVariables ("sts", 0.5F * QTc);
			}
		}
    }

    void T()
    {
		if (rhythm == "vt") {
			if (heartRate > 160f) {
				TP ();
			} else {
				bending = new Vector3 (0f, (1.5f * scale), 0f);
				SetVariables ("t", gap - cumulativeDeltaTime);
			}
		} else if (wideQRS) {
			bending = new Vector3 (0f, (1.5f * scale), 0f);
			SetVariables ("t", QTc);
		} else if (rhythm == "svt") {
			bending.y = 0.35f*scale;
			SetVariables("t", 2F * QTc);
		} else {
			bending.y = 0.35f*scale;
            SetVariables("t", 1.5F * QTc);
        }
    }

    void TP()
    {
		if (rhythm == "aflutter") {
			aFlutterTimeStamp = Time.time;
			if (variableRhythm) {
				int var1 = Random.Range (0, 5);
				float var2 = (float)var1;
				if (var2 == 0f) {
					QR ();
				} else {
					SetVariables ("tp", ((var2 / 2)) * gap - cumulativeDeltaTime);
				}
			} else {
				SetVariables ("tp", gap - cumulativeDeltaTime);
			}
		} else if (rhythm == "af") {
			heartRate = Random.Range (heartRateLower, heartRateUpper);
			pqWidth = (60f / heartRate) * 0.08f;
			QTc = ((((300 * Mathf.Sqrt (60f / heartRate)) - 40f) / 2f) / 1000);
			lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + (QTc * 2);
			gap = (60f / heartRate) - lengthy;
			bending = new Vector3 (0f, (0.03f * scale), 0f);
			SetVariables ("tp", gap - cumulativeDeltaTime);
		} else if (rhythm == "vf") {
			float x = Random.Range (0.1f, 0.3f);
			float y = (Random.Range (0.1f, 2.5f)) * scale;
			bending = new Vector3 (0f, y, 0f);
			SetVariables ("tp", x);
		} else if (rhythm == "svt") {
			if (gap <= 0) {
				QR ();
			} else {
				SetVariables ("tp", gap - cumulativeDeltaTime);
			}
		} else if (rhythm == "vt") {
			if (heartRate > 160f) {
				if (shock && sync && charged) {
					Shock ();
				} else {
					actualHeartRate = 60f / (Time.time - hrDebugger);
					hrDebugger = Time.time;
					if (torsades) {
						if (bending.y >= 1f) {
							torsadesUp = false;
						} else if (bending.y <= 0.5f) {
							torsadesUp = true;
						}
						if (torsadesUp) {
							bending.y += 0.25f;
						} else {
							bending.y -= 0.25f;
						}
					} else {
						bending.y = 1f;
					}
					//These are here because functions not triggered in QR() function as usual:
					satsScript.T (heartRate);
					aLineScript.T (heartRate);
					if (defibReady) {
						StartCoroutine(BeepDelay(((60f / heartRate) - cumulativeDeltaTime)/2f));
					}
					if (!shock && sync) {
						StartCoroutine(SyncDelay((60f / heartRate) - cumulativeDeltaTime));
					}
					SetVariables ("tp", (60f / heartRate) - cumulativeDeltaTime);
				}
			} else {
				bending.y = 0.5f;
				QR ();
			}
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

	IEnumerator SyncDelay (float delay) {
		yield return new WaitForSeconds (delay);
		SyncMarker();
	}

	IEnumerator BeepDelay (float delay) {
		yield return new WaitForSeconds (delay);
		hub.PlaySound ("Beep");
	}

	IEnumerator CHBsetter() {
		yield return new WaitForSeconds (0.75f);
		if (rhythm == "chb") {
			chbPwaveTimeStamp = Time.time;
			chbPwave = true;
		}
		StartCoroutine (CHBsetter ());
	}

	//THE PUBLIC FUNCTION FOR CALLING A SHOCK
	public void Shock()
	{
		if (defibReady) {
			ShockChecker ();
		}
	}

	//CHECKS IF SHOCK IS CORRECT, CALLS DeliverShock() IF SO, OR FAILSCREEN IF NOT
	void ShockChecker () {
		if (rhythm=="vf") { MAP = 0f; }
		DumpCharge();
		shock = false;
		if (
			//Shocking NSR
			(!noShock && (rhythm == "nsr" || rhythm == "chb") && MAP > 0f))
		{
			hub.ChangeFailText ("Oops! You cannot cardiovert sinus rhythm or AV blocks!");
			WrongShock();
		}
		else if (
			//Not shocking VF
			(rhythm=="vf" && noShock))
		{
			hub.ChangeFailText("Oops! That was VF. You should have shocked the patient.");
			WrongShock();
		}
		else if (
			//Not shocking pulseless VT
			(rhythm == "vt" && noShock && MAP==0f))
		{
			string failText = "Oops! That was pulseless";
			if (torsades) {
				failText += " polymorphic";
			}
			failText += " VT. You should have shocked the patient.";
			hub.ChangeFailText(failText);
			WrongShock();
		}
		else if (
			//Non-shockable rhythm in cardiac arrest
			(!noShock && MAP == 0 && (rhythm == "af" || rhythm == "aflutter" || rhythm == "svt" || rhythm == "nsr" || rhythm == "chb")))
		{
			string failText = "Oops! ";
			if (rhythm == "af") {
				failText += "That was atrial fibrillation";
				if (wideQRS) {
					failText += " with a broad QRS complex";
				}
				failText += ". That's a non-shockable rhythm.";
			} else if (rhythm == "aflutter") {
				failText += "That was atrial flutter";
				if (wideQRS) {
					failText += " with a broad QRS complex";
				}
				failText += ". That's a non-shockable rhythm.";
			} else if (rhythm == "svt") {
				failText += "That was SVT";
				if (wideQRS) {
					failText += " with a broad QRS complex";
				}
				failText += ". That's a non-shockable rhythm.";
			} else if (rhythm == "nsr") {
				if (mobitzI) {
					failText += "That was Mobitz I";
				} else if (mobitzII) {
					failText += "That was Mobitz II";
				} else {
					failText += "That was sinus rhythm";
				}
				if (wideQRS) {
					failText += " with a broad QRS complex";
				}
				failText += ". That's a non-shockable rhythm.";
			} else if (rhythm == "chb") {
				failText += "That was complete heart block. That's a non-shockable rhythm.";
			}
			hub.ChangeFailText(failText);
			WrongShock();
		}
		else if (
			//Shocking a good BP
			(!noShock && MAP > 60))
		{
			hub.ChangeFailText("Oops! With that blood pressure, the patient could have waited\nfor an elective shock.");
			WrongShock();
		}
		else if (
			//Non-sync'ed shock:
			((rhythm == "af" || rhythm == "aflutter" || rhythm == "svt" || (rhythm == "vt" && MAP > 0)) && !sync && !noShock))
		{
			hub.ChangeFailText("Oops! You should have synchronised that shock.");
			WrongShock();
		}
		else if (

			//Not shocking a low BP in non-sinus rhythm (if MAP < 60 it will be < 40)
			(noShock && MAP < 60 && MAP > 0 && rhythm != "nsr"  && heartRate >120f))
		{
			hub.ChangeFailText("Oops! The patient had an unstable tachyarrhythmia, that's an indication for emergency cardioversion.");
			WrongShock();
		}
		else if (

			//Not pacing an unstable bradycardia
			(noShock && MAP < 60 && heartRate < 45f && !pacing && hub.atropineDose==3000))
		{
			hub.ChangeFailText("Oops! The patient had unstable bradycardia and you already maxed out the atropine, you should have paced them.");
			WrongShock();
		}
		else if (

			//Not hitting the pacing threshhold 
			(noShock && MAP < 60 && heartRate < 45f && !capture && hub.atropineDose == 3000))
		{
			hub.ChangeFailText("Oops! Try increasing the pacing current until you get capture.");
			WrongShock();
		}
		else if (

			//Not getting safe capture
			(noShock && MAP < 60 && heartRate < 45f && !safeCapture && hub.atropineDose == 3000))
		{
			hub.ChangeFailText("Oops! You need to set the current 10mA above the pacing threshold for safe capture.");
			WrongShock();
		}
		else
		{
			DeliverShock();
		}
	}

	/*DELIVERS A SHOCK IF noShockToDeliver=false
    OTHERWISE, JUST RESETS RHYTHM*/
	void DeliverShock()
	{
		if (debugging) {
			Debug.Log ("Delivering shock from control");
		}
		if (!hub.patient.conscious) {
			//Function that deactivates failScreen if active and sets noShock as "true"
			noShock = hub.CheckFailScreen (noShock);

			hub.shockFail = false;

			if (!noShock) {
				Spike ();
				noShock = false;
				hub.CameraToDefaultView ();
				hub.PlaySequence ("Defib");
			}
			hub.AdrenalineChecker ();
			/*if ((rhythm == "af" || rhythm == "aflutter" || rhythm == "svt" || rhythm=="vt")&&MAP>0&&wave=="t"))
        {
            if (sync)
            {
                int pos = Random.Range(0, 6);
                string[] rhythms = new string[] { "af", "aflutter", "nsr", "svt", "vt", "vf" };
                string newRhyth = rhythms[pos];
                Debug.Log("rhythm = " + rhythms[pos]);
            }
            else
            {
                rhythm = "vf";
                //throw message about non-sync'ed DCC
            }
        }
        else
        {
            int pos = Random.Range(0, 6);
            string[] rhythms = new string[] { "af", "aflutter", "nsr", "svt", "vt", "vf" };
            string newRhyth = rhythms[pos];
            Debug.Log("rhythm = " + rhythms[pos]);
            //FOR SETTING RANDOM RHYTHM:
            //rhythm = newRhyth;

            //FOR PURPOSES OF TEST LEVEL, FIRST SHOCK WILL GIVE AF (PEA)
            rhythm = afterShockRhythm;
        }*/
			if (!remoteConnected) {
				rhythm = MakeSenseOfRhythm (hub.NextRhythm ());
			}
			wave = "sts";
			oldY = spriteRen.transform.localPosition.y;

			SetBP ();
			SetRhythm ();
			SetOtherFactors ();
			Update ();
		} else {
			string message = "Just as you are about to hit the shock button, your conscious patient asks you what you're " +
			                 "doing. You hurriedly dump the charge and wonder if you should use some sedation before proceeding...";
			hub.SendMessage (message, 0, 2, true);
			hub.StopAllSounds ();
		}
	}

	//CREATES A PACING OR DEFIB SPIKE
	void Spike()
	{
		Vector3 spikerPos = new Vector3(spriteRen.transform.position.x, localY, spriteRen.transform.position.z);
		spiker = (GameObject)Instantiate(spike, spikerPos,
			Quaternion.Euler(90, 0, 0));
		spiker.transform.parent = defibScreen.transform;
		spiker.transform.localScale += new Vector3(0f, 2f, 0f);
		LineItem liner = new LineItem(spiker, Time.time);
		lineList.Add(liner);
	}

	//CHARGEDS THE DEFIB
	public void Charge()
	{
		if (defibReady) {
			if (charging || charged) {
				hub.StopAllSounds ();
				DumpCharge ();
			} else if (!pacing) {
				chargeTime = 0f;
				charging = true;
				hub.PlaySound ("DefibCharging");
			}
		}
	}

	//DUMPS THE CHARGE AFTER A SHOCK OR IF SHOCK CANCELLED
	public void DumpCharge()
	{
		charging = false;
		charged = false;
		shockButton.active = false;
		chargedLight.active = false;
		chargingText.GetComponent<TextMesh>().text = "";
	}

	void WrongShock()
	{
		hub.DispatchInsight(Insights.Blunder);
		if (hub.patient.conscious) {
			string message = "Just as you are about to hit the shock button, your conscious patient asks you what you're " +
			                 "doing. You hurriedly dump the charge and wonder if you should use some sedation before proceeding...";
			hub.SendMessage (message, 0, 2, true);
			hub.StopAllSounds ();
		} else {
			hub.shockFail = true;
			if (hub.cardiacArrest) {
				hub.failedCycles++;
			} else {
				hub.failedShocks++;
			}
			shockFail = true;
			if (!noShock) {
				hub.CameraToDefaultView ();
				hub.PlaySequence ("Defib");
			} else {
				Fail ();
			}
		}
	}

	public void Fail()
	{
		paused = true;
		hub.StopAllSounds();
		hub.ActivateDeactivateFailScreen(true);
	}

	/*REACTIVATES THE DEFIB AND RESUMES CPR
    NOTE TIME.TIMESCALE IS RESET TO 1 BY THE hub.ActivateDeactivateFailScreen FUNCTION*/
	public void ReTry()
	{
		hub.ActivateDeactivateFailScreen(false);
		paused = false;
		noShock = true;
		shock = false;
		hub.PlaySequence("LastPlayed");
		//DeliverShock();
	}

	/*LETS THE DEFIB KNOW A SHOCK IS DUE (bool shock = true)
    IF SYNC IS ACTIVATED IN VF, NO SHOCK WILL BE DELIVERED
    SHOCK WILL BE DELIVERED IMMEDIATELY IF SYNC NOT ACTIVATED
    SHOCK WILL BE DELIVERED AT NEXT R WAVE IF SYNC IS ACTIVATED*/

	public void Change(bool noShockToDeliver)
	{
		//Debug.Log("Changing shock status");
		if (rhythm == "vf" && sync == true)
		{
		}
		else
		{
			shock = true;
		}
		noShock = noShockToDeliver;
		//Debug.Log(noShock);
	}

	//CODES THE PACER
	public void Pace()
	{
		if (defibReady && hub.clickable && !capture) {
			if (energyText.active) {
				energyText.active = false;
			} else {
				energyText.active = true;
			}
			if (charging || charged) {
				hub.StopAllSounds ();
				DumpCharge ();
			}
			if (sync) {
				Sync ();
			}
			paceLight.active = !paceLight.active;
			UpdatePacingText ();
			pacingText.active = !pacingText.active;
			pacing = !pacing;
		}
	}

	public void ChangePaceRate(string upDown)
	{
		if (hub.clickable) {
			if (defibReady) {
				if (upDown == "up" && paceRate < 100f) {
					paceRate += 5f;
				} else if (upDown == "down" && paceRate > 0f) {
					paceRate -= +5f;
				}
				UpdatePacingText ();
			}
		}
	}

	public void ChangePaceCurrent (string upDown)
	{
		if (hub.clickable) {
			if (defibReady) {
				if (upDown == "up" && paceCurrent < 125f) {
					if (hub.patient.conscious) {
						string message = "You might want to think about sedating your patient before you start externally " +
						                "pacing them...";
						hub.SendMessage (message, 0, 2, true);
					} else {
						paceCurrent += 5f;
					}
				} else if (upDown == "down" && paceCurrent >= 0f) {
					paceCurrent -= 5f;
				}
				UpdatePacingText ();
			}
		}
	}

	void UpdatePacingText()
	{
		pacingText.GetComponent<TextMesh>().text = "Pace rate: ";
		if (paceRate < 1)
		{
			pacingText.GetComponent<TextMesh>().text += "0";
		}
		else
		{
			pacingText.GetComponent<TextMesh>().text += string.Format("{0:#}", paceRate);
		}
		pacingText.GetComponent<TextMesh>().text += "\nCurrent: ";
		if (paceCurrent < 1)
		{
			pacingText.GetComponent<TextMesh>().text += "0";
		}
		else
		{
			pacingText.GetComponent<TextMesh>().text += string.Format("{0:#}", paceCurrent);
		}
		pacingText.GetComponent<TextMesh>().text += "mA";
	}

	void DoPace()
	{
		if (rhythm != "vf")
		{
			pacingSpike = true;
			lastPacingSpike = Time.time;
			Spike();
			if (paceCurrent > patient.pacingThreshold && MAP > 0f)
			{
				capture = true;
				if (paceCurrent >= (patient.pacingThreshold + 10))
				{
					safeCapture = true;
				}
				else
				{
					safeCapture = false;
				}
				previousWideQRS = wideQRS;
				Wide(true);
				pacingComplex = true;
				endPosition = spriteRen.transform.position;
				QR();
			}
			else
			{
				capture = false;
			}
		}
	}

	public void EnergyUp ()
	{
		if (defibReady && hub.clickable) {
			if (energyListPosition < energyList.Length - 1) {
				energyListPosition++;
				energyText.GetComponent<TextMesh> ().text = "Energy: " + energyList [energyListPosition] + "J";
			}
		}
	}

	public void EnergyDown()
	{
		if (defibReady && hub.clickable) {
			if (energyListPosition > 0) {
				energyListPosition--;
				energyText.GetComponent<TextMesh> ().text = "Energy: " + energyList [energyListPosition] + "J";
			}
		}
	}

	public void DefibReady () {
		defibReady = true;
        if (hub.resumeText.text == "Back")
        {
            hub.resumeText.text = "Resume CPR";
        }
	}

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 3.9f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
        lr.SetColors(Color.green, Color.green);
        lr.SetWidth(0.05f, 0.05f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}

//ALLOWS SPIKES AND SYNC MARKERS TO BE CREATED AND DELETED AFTER FIXED TIME
class LineItem {
    public GameObject g;
    public float f;
    public LineItem (GameObject gx, float fx)
    {
        g = gx;
        f = fx;
    }
}