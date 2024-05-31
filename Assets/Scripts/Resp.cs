using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Resp : MonoBehaviour {
	public bool debugging = true;

	public GameObject dotPrefab;
	private GameObject actualEcgDot;
    private GameObject respDot;

	public Control control;

	public Hub hub;

	public TextMesh respText;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private Vector3 bending;
    private Vector3 lastPos;

    private float screenEnd = 0f;
	private float speed = 1f;
	public float timerAcrossScreen = 5f;
	private float timeStamp = 0f;
	public float scale = 1f;
	private float breathDuration = 1f;
	private float duration = 1f;
	private float screenWidth = 0f;
	public float stripLength = 1f;
	private float localY;
	private float screenLeftX = 0f;
	private float screenThreeQuartersY = 0f;
	private float halfWayX = 0f;
	public float MAP = 100f;
	private float lastY = 0.5f;
	private float monitorCloneNumber = 0f;
	public float yPosition = 0f;
	private float screenToMonitorX = 0f;
	private float screenToMonitorY = 0f;

	public int respRate = 16;

	public Insights rhythm = Insights.HeartRhythmNSR;
    private string monitorClone = "Clone0";

    private bool changing = false;
	public bool server = true;
	public bool threeD = false;
	public bool respOn = false;
	public bool started = false;

	public int doubleXspeed = 1;

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

		respText = GetComponent<TextMesh> ();

		startPosition = new Vector3 (actualEcgDot.transform.position.x - screenToMonitorX, 
			actualEcgDot.transform.position.y + yPosition + screenToMonitorY, transform.position.z);
		endPosition = startPosition;

		screenLeftX = startPosition.x;

		respDot = (GameObject)Instantiate (dotPrefab, startPosition, Quaternion.identity);
        respDot.name = "Current resp dot";
		localY = startPosition.y;

		transform.position = new Vector3 (transform.position.x,
			localY+0.5f, startPosition.z);

		respText.text = "--";

		timeStamp = Time.time;

		started = true;

        lastPos = respDot.transform.position;

        ClientChangeResps (hub.respRate / 40f);

		T ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (respOn) {
			if (duration > 60f && changing) {
				if (debugging) {
					Debug.Log ("Going to T() in resp script");
				}
				T ();
			} else if ((Time.time - timeStamp) <= duration) {
				MoveDot ();
			} else {
				T ();
			}
		}
	}

	void MoveDot() {
		//GameObject respDot = GameObject.Find ("Current resp dot");
		Vector3 newPosition = new Vector3 (0f, 0f, 0f);

		//Code to move dot to position.x of ECGdot
		//string monitorClone = "Clone" + monitorCloneNumber.ToString ("0");
		if (GameObject.Find (monitorClone)) {
			actualEcgDot = GameObject.Find (monitorClone);
			float dotX = actualEcgDot.transform.position.x - screenToMonitorX;
			dotX = ((dotX - screenLeftX) / 2f) + screenLeftX;
			if (doubleXspeed == 2) {
				dotX += halfWayX - screenLeftX;
			}
			newPosition = new Vector3 (dotX,
				localY,
				respDot.transform.position.z);
		} else {
			newPosition = new Vector3 (respDot.transform.position.x + (Time.deltaTime * speed)/2f,
				localY,
				respDot.transform.position.z);
		}

		if (rhythm != Insights.HeartRhythmVF && MAP != 0f) {
			float x = ((Time.time - timeStamp) / breathDuration);
			if (x <= 1f) {
				x = x;
				bending = new Vector3 (0f, 1f * scale, 0f);
				newPosition.y += bending.y * Mathf.Sin ((1f - x) * (1f - x) * Mathf.PI);
			} else if (changing) {
				T ();
			}
		}

        lastPos = respDot.transform.position;
        respDot.transform.position = newPosition;
        DrawLine(lastPos, newPosition, Color.yellow);
    }

	public void ClientChangeResps (float sliderY) {
		if (sliderY > 1f) {
			sliderY = 1f;
		}
		if (rhythm != Insights.HeartRhythmVF) {
			float newRR = sliderY * 40f;
			respRate = (int)newRR;
			hub.respRate = respRate;
			//Initial call from Aline script if MAP == 0 fails because respText not yet set.
			//Therefore needed this "if" clause:
			if (respText && respOn) {
				respText.text = respRate.ToString ();
			}
			changing = true;
		}
	}

	public void ResetTracer(Vector3 newPosition, float monitorCloneNo) {
		//GameObject respDot = GameObject.Find ("Current resp dot");
		if (doubleXspeed == 1) {
			halfWayX = respDot.transform.position.x;
			doubleXspeed++;
		} else {
            newPosition = new Vector3(newPosition.x, respDot.transform.position.y, respDot.transform.position.z);
            doubleXspeed = 1;
			/*if (GameObject.Find ("Old resp dot")) {
				GameObject oldEcgDot = GameObject.Find ("Old resp dot");
				Destroy (oldEcgDot);
			}
			respDot.name = "Old resp dot";
			GameObject respDotTemp = (GameObject)Instantiate (dotPrefab, newPosition, Quaternion.identity);
			respDotTemp.name = "Current resp dot";
			respDotTemp.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);*/
            respDot.transform.position = newPosition;
		}
		//monitorCloneNumber = monitorCloneNo;
	}

	//For the sats function: the ECG trace calls this
	public void T()
	{
		MAP = hub.MAP;
		GameObject respDot = GameObject.Find ("Current resp dot");
		if (respRate <= 20) {
			breathDuration = 3f;
		} else {
			breathDuration = (60f / respRate);
		}
		duration = (60f / respRate);
		if (debugging) {
			Debug.Log ("Resp duration = " + duration + "resp rate = " + respRate);
		}
		changing = false;
		timeStamp = Time.time;
		if (respOn) {
			respText.text = respRate.ToString ();
		}
		hub.respRate = respRate;
		rhythm = hub.rhythm;
		if (debugging) {
			Debug.Log ("Rhythm = " + rhythm + " MAP=" + MAP);
		}
		FixedUpdate ();
	}

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 7.8f)
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
