using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Sats : MonoBehaviour {
	public bool debugging = true;

	public GameObject dotPrefab;
	private GameObject actualEcgDot;
    private GameObject satsDot;

	public TextMesh satsText;

	public Control control;

	public Hub hub;

	private float screenEnd = 0f;
	private float speed = 1f;
	public float timerAcrossScreen = 5f;
	private float timeStamp = 0f;
	public float scale = 0.1f;
	private float duration = 1f;
	private float screenWidth = 0f;
	public float stripLength = 1f;
	private float localY;
	private float screenLeftX = 0f;
	private float screenThreeQuartersY = 0f;
	private float sliderY = 1f;
	private float heartRate = 0f;
	public float yPosition = 0f;
	private float monitorCloneNumber = 0;
	private float screenToMonitorX = 0f;
	private float screenToMonitorY = 0f;

	public int sats = 100;

	public string wave = "x";
	public string rhythm = "sats";
	public string ecgRhythm = Insights.HeartRhythmNSR;
    private string monitorClone = "Clone0";

	public bool mapZero = true;
	private bool changing = false;
	public bool satsOn = false;
	public bool started = false;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private Vector3 bending;
    private Vector3 lastPos;

	// Use this for initialization
	void Start () {
		satsText = GetComponent<TextMesh> ();

		screenToMonitorX = control.screenToMonitorX;
		screenToMonitorY = control.screenToMonitorY;
		//string monitorClone = "Clone" + monitorCloneNumber.ToString ("0");
		if (GameObject.Find (monitorClone)) {
			actualEcgDot = GameObject.Find (monitorClone);
		} else {
			Debug.Log ("Couldn't find " + monitorClone);
		}

		startPosition = new Vector3 (actualEcgDot.transform.position.x - screenToMonitorX, 
			actualEcgDot.transform.position.y + yPosition + screenToMonitorY, transform.position.z);
		endPosition = startPosition;

		satsDot = (GameObject)Instantiate (dotPrefab, startPosition, Quaternion.identity);
		satsDot.name = "Current sats dot";
		localY = startPosition.y;

		transform.position = new Vector3 (transform.position.x,
			localY+0.5f, startPosition.z);

		satsText.text = "--";

		timeStamp = Time.time;

		started = true;

		ClientChangeSats ((hub.sats - 70f) / 30f);

        lastPos = satsDot.transform.position;

		Update ();

	}
	
	// Update is called once per frame
	void Update () {
		if (satsOn) {
			if ((Time.time - timeStamp) <= duration) {
				MoveDot ();
			} else if (changing) {
				SatsTracer ();
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
		//GameObject satsDot = GameObject.Find ("Current sats dot");
		Vector3 newPosition = new Vector3 (0f, localY, 0f);

		//string monitorClone = "Clone" + monitorCloneNumber.ToString ("0");
		if (GameObject.Find (monitorClone)) {
			actualEcgDot = GameObject.Find (monitorClone);

			float dotX = actualEcgDot.transform.position.x - screenToMonitorX;

			newPosition = new Vector3 (dotX, localY, satsDot.transform.position.z);
		} else {
			newPosition = new Vector3 (satsDot.transform.position.x + (Time.deltaTime * speed),
				localY,
				satsDot.transform.position.z);
		}

		//Describe half a sin wave with a slant for sats waves
		if (wave == "t") {
			bending = new Vector3 (0f, ((0.6f + (0.4f * sliderY))*scale), 0f);
			float x = ((Time.time - timeStamp) / duration);
			newPosition.y += bending.y * Mathf.Sin (Mathf.Pow((1f - x), 2f) * Mathf.PI);
		}

        lastPos = satsDot.transform.position;
        satsDot.transform.position = newPosition;
        DrawLine(lastPos, newPosition, Color.blue);
    }

	public void ResetTracer(Vector3 newPosition, float cloneNo) {
		//GameObject satsDot = GameObject.Find ("Current sats dot");
		newPosition = new Vector3 (newPosition.x, satsDot.transform.position.y, satsDot.transform.position.z);
        /*if (GameObject.Find("Old sats dot")) {
			GameObject oldEcgDot = GameObject.Find ("Old sats dot");
			Destroy (oldEcgDot);
		}
		satsDot.name = "Old sats dot";
		GameObject satsDotTemp = (GameObject)Instantiate (dotPrefab, newPosition, Quaternion.identity);*/
        satsDot.transform.position = newPosition;
		/*satsDotTemp.name = "Current sats dot";
		satsDotTemp.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		monitorCloneNumber = cloneNo;*/
	}

	public void ClientChangeSats (float newValue) {
		if (newValue > 1f) {
			newValue = 1f;
		}
		sliderY = newValue;
		if (sliderY == -1f) {
			if (satsOn) {
				satsText.text = "-";
			}
			hub.sats = 0;
		} else {
			float newSats = 70f + (sliderY * 30f);
			sats = (int)newSats;
			hub.sats = sats;
			if (satsOn) {
				satsText.text = newSats.ToString ("0");
			}
		}
	}

	//For the sats function: the ECG trace calls this
	public void T(float currentHeartRate)
	{
		heartRate = currentHeartRate;
		changing = true;
	}

	private void SatsTracer () {
		changing = false;
		if (!mapZero) {
			if (heartRate < 40f) {
				heartRate = 40f;
			}
			wave = "t";
			duration = (60f / heartRate);
			timeStamp = Time.time;
			satsText.text = sats.ToString ();
		}
	}

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 3.9f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended"));
        Color sky_blue = new Color(0, 191f, 255f);
        lr.SetColors(sky_blue, sky_blue);
        lr.SetWidth(0.05f, 0.05f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}

