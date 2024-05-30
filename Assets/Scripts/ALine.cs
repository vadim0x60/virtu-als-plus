using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ALine : MonoBehaviour {
	public bool debugging = true;

	public GameObject dotPrefab;
	private GameObject actualEcgDot;
    private GameObject aLineDot;

	public Control control;

	public Hub hub;

	public TextMesh bpText;

	private string bpSysText;
	private string bpDiasText;

	private float screenEnd = 0f;
	private float speed = 1f;
	public float timerAcrossScreen = 5f;
	private float timeStamp = 0f;
	public float scale = 1f;
	private float duration = 1f;
	private float screenWidth = 0f;
	public float stripLength = 1f;
	private float localY;
	private float screenLeftX = 0f;
	private float screenThreeQuartersY = 0f;
	public float MAP = 0f;
	private float bpSliderValue = 0f;
	private float heartRate = 0f;
	private float monitorCloneNumber = 0f;
	public float yPosition = 0f;
	private float screenToMonitorX = 0f;
	private float screenToMonitorY = 0f;

	public int bpSys = 120;
	public int bpDias = 80;

	private string wave = "x";
	public string rhythm = "sats";
    private string monitorClone = "Clone0";

    private int previousSats = 100;
	private int previousRR = 16;

	public Sats satsScript;
	public Resp respScript;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private Vector3 bending;
    private Vector3 lastPos;

    public bool server = true;
	private bool changing = false;
	public bool aLineOn = false;
	public bool bpCuffOn = false;
	public bool started = false;

	public Networker networker;

	// Use this for initialization
	void Start () {
		screenToMonitorX = control.screenToMonitorX;
		screenToMonitorY = control.screenToMonitorY;
		//string monitorClone = "Clone" + monitorCloneNumber.ToString ("0");
		if (GameObject.Find (monitorClone)) {
			actualEcgDot = GameObject.Find (monitorClone);
		} else {
			Debug.Log ("Couldn't find " + monitorClone);
		}

		bpText = GetComponent<TextMesh> ();

		startPosition = new Vector3 (actualEcgDot.transform.position.x - screenToMonitorX, 
			actualEcgDot.transform.position.y + yPosition + screenToMonitorY, transform.position.z);
		endPosition = startPosition;

		aLineDot = (GameObject)Instantiate (dotPrefab, startPosition, Quaternion.identity);
		aLineDot.name = "Current A line dot";
		localY = startPosition.y;

		transform.position = new Vector3 (transform.position.x,
			localY+1f, startPosition.z);

		bpSysText = bpSys.ToString();
		bpDiasText = bpDias.ToString ();
		bpText.text = "--\n--";

		timeStamp = Time.time;

		MAP = hub.MAP;

		//Necessary to ensure resps and sats set appropriately if MAP == 0f
		if (MAP == 0f) {
			ClientChangeBP (0f);
		} else {
			ClientChangeBP (MAP / 120f);
		}

        lastPos = aLineDot.transform.position;

        started = true;
	}

	public void CheckBP() {
		bpSysText = bpSys.ToString();
		bpDiasText = bpDias.ToString ();
		bpText.text = bpSysText + "\n" + bpDiasText;
	}

	// Update is called once per frame
	void Update () {
		if (aLineOn) {
			if ((Time.time - timeStamp) <= duration) {
				MoveDot ();
			} else if (changing) {
				ArtLineTracer ();
			} else {
				wave = "x";
				duration = 0.1f;
				timeStamp = Time.time;
				MoveDot ();
			}
		}
	}

	void MoveDot ()
	{
		//GameObject aLineDot = GameObject.Find ("Current A line dot");
		Vector3 newPosition = new Vector3 (0f, 0f, 0f);
		//string monitorClone = "Clone" + monitorCloneNumber.ToString ("0");
		if (GameObject.Find (monitorClone)) {
			actualEcgDot = GameObject.Find (monitorClone);
			float dotX = actualEcgDot.transform.position.x - screenToMonitorX;
			newPosition = new Vector3 (dotX, localY, aLineDot.transform.position.z);
		} else {
			newPosition = new Vector3 (aLineDot.transform.position.x + (Time.deltaTime * speed),
				localY,
				aLineDot.transform.position.z);
		}

		//Describe half a sin wave with a slant for sats waves
		if (wave == "t") {
			bending = new Vector3 (0f, bpSliderValue * scale, 0f);
			float x = ((Time.time - timeStamp) / duration);
			if (duration >= 0.75f) {
				newPosition.y += bending.y * Mathf.Sin (Mathf.Pow ((1f - x), 5f) * Mathf.PI);
			} else {
				newPosition.y += bending.y * Mathf.Sin (Mathf.Pow ((1f - x), 2f) * Mathf.PI);
			}
		}

        lastPos = aLineDot.transform.position;
		aLineDot.transform.position = newPosition;
        DrawLine(lastPos, newPosition, Color.red);
    }

	public void ClientChangeBP(float sliderY) {
		if (sliderY > 1f) {
			sliderY = 1f;
		}
		//bpSliderValue used to set height of arterial wave, so still needs to be set in client function
		bpSliderValue = sliderY;
		if (rhythm != "vf") {
			//SET THE ACTUAL BP BASED ON THE MAP - SAVES HAVING TO SET DIASTOLIC AND SYSTOLIC VALUES EACH TIME
			bool mapWasZero = false;
			if (MAP == 0f) {
				mapWasZero = true;
			}
			MAP = sliderY * 120f;
			//float bpDiasTemp = Random.Range (MAP * 0.7f, MAP * 0.9f);
			float bpDiasTemp = MAP*0.8f;
			if (bpDiasTemp < 10) {
				bpDiasTemp = 0f;
			}
			//float bpSysTemp = Random.Range (MAP * 1.1f, MAP * 1.3f);
			float bpSysTemp = MAP * 1.2f;
			if (bpSysTemp < 10) {
				bpSysTemp = 0f;
			}

			bpDias = (int)bpDiasTemp;
			bpSys = (int)bpSysTemp;
			bpSysText = bpSys.ToString();
			bpDiasText = bpDias.ToString ();
			if (aLineOn) {
				bpText.text = bpSysText + "\n" + bpDiasText;
			}

			if (MAP == 0f) {
				previousSats = satsScript.sats;
				previousRR = respScript.respRate;
				respScript.ClientChangeResps (0f);
				satsScript.wave = "x";
				satsScript.mapZero = true;
				satsScript.satsText.text = "-";
			} else if (mapWasZero) {
				satsScript.mapZero = false;
				satsScript.sats = previousSats;
				Debug.Log ("Changing to previous sats from A line script");
				float rrFraction = (float)previousRR / 40;
				respScript.ClientChangeResps (rrFraction);
				float z = (float)previousSats;
			}

			respScript.MAP = MAP;
			hub.MAP = MAP;
		}
	}

	public void ResetTracer(Vector3 newPosition, float monitorCloneNo) {
		//GameObject aLineDot = GameObject.Find ("Current A line dot");
		aLineDot.transform.position = new Vector3 (newPosition.x, aLineDot.transform.position.y, aLineDot.transform.position.z);
		/*if (GameObject.Find("Old A line dot")) {
			GameObject oldEcgDot = GameObject.Find ("Old A line dot");
			Destroy (oldEcgDot);
		}
		aLineDot.name = "Old A line dot";
		GameObject aLineDotTemp = (GameObject)Instantiate (dotPrefab, newPosition, Quaternion.identity);
		aLineDotTemp.name = "Current A line dot";
		aLineDotTemp.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		monitorCloneNumber = monitorCloneNo;*/
	}

	//For the sats function: the ECG trace calls this
	public void T(float currentHeartRate)
	{
		heartRate = currentHeartRate;
		changing = true;
	}

	private void ArtLineTracer () {
		changing = false;
		if (heartRate < 40f) {
			heartRate = 40f;
		}
		//GameObject aLineDot = GameObject.Find ("Current A line dot");
		if (aLineDot.transform.position.y - localY >= 0.15f) {
            lastPos = aLineDot.transform.position;
            aLineDot.transform.position = new Vector3 (aLineDot.transform.position.x,
				localY + 0.15f,
				aLineDot.transform.position.z);
            DrawLine(lastPos, aLineDot.transform.position, Color.red);
        } else if (aLineDot.transform.position.y - localY <= 0.15f) {
            lastPos = aLineDot.transform.position;
            aLineDot.transform.position = new Vector3 (aLineDot.transform.position.x,
				localY-0.15f,
				aLineDot.transform.position.z);
            DrawLine(lastPos, aLineDot.transform.position, Color.red);
        }
		wave = "t";
		duration = (60f / heartRate);
		timeStamp = Time.time;
		bpSysText = bpSys.ToString();
		bpDiasText = bpDias.ToString ();
		if (aLineOn) {
			bpText.text = bpSysText + "\n" + bpDiasText;
		}
		Update ();
	}

	public IEnumerator BPChecker () {
		if (bpCuffOn && !aLineOn) {
			CheckBP ();
			yield return new WaitForSeconds (60f);
			StartCoroutine (BPChecker ());;
		}
	}

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 3.9f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended"));
        lr.SetColors(color, color);
        lr.SetWidth(0.05f, 0.05f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}

