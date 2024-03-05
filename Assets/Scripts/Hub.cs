using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

/*
Notes:

The Hub is supposed to minimise complex interconnections between objects.
Ideally, most objects should connect via the Hub (i.e. "public Hub hub") rather than direct to each other.

TO DO 17/9/16:

Should have a delay while the arterial line is getting inserted. Possibly same with cardiac monitor leads? Possibly
same with other equipment, in fact?

*/


public class Hub : MonoBehaviour {
	public bool debugging = true;
    public bool stable_patient = false;

	public bool nonBlockingMode = false;

    public GameObject failScreen;
    public GameObject controller;
    public GameObject defibScreen;
    public GameObject mainCamera;
    public GameObject defibCamera;
    public GameObject resumeButton;
    public GameObject defibHint;
    public GameObject nonResumeButtons;
    public GameObject CPRButtons;
	public GameObject nonCPRButtons;
    public GameObject soundPlayer;
    public GameObject patientObject;
    public GameObject attachPadsButton;
    public GameObject signsOfLifeButton;
    public GameObject checkRhythmButton;
    public GameObject defibOnCanvasButton;
	public GameObject defibOnDefibButton;
    public GameObject iVaccessButton;
    public GameObject fluidsButton;
	public GameObject monitorButton;
	public GameObject chestCompressionButton;
	public GameObject baggingDuringCPRButton;
	public GameObject airwayButton;
	public GameObject airwayDrawer;
	public GameObject airwayText;
	public GameObject breathingButton;
	public GameObject breathingDrawer;
	public GameObject breathingText;
	public GameObject circulationButton;
	public GameObject circulationDrawer;
	public GameObject circulationText;
	public GameObject drugsButton;
	public GameObject drugsButtonNCPR;
	public GameObject drugDrawer;
	public GameObject drugText;
	public GameObject drugTextNCPR;
	public GameObject monitorButtonNCPR;
	public GameObject useDefibButton;
	public GameObject doneButton;
	public GameObject messageScreen;
	public GameObject messageBackground;
	public GameObject messageButton;
	public GameObject demoEndScreen;
	public GameObject menu;
	public GameObject menuPlayButtons;
	public GameObject menuLearningZone;
	public GameObject menuPauseButtons;
	public GameObject startResumeButton;
	public GameObject gameCanvas;
	public GameObject drawerCmPads;
	public GameObject drawerSatsProbe;
	public GameObject drawerAline;
	public GameObject drawerBPCuff;
	public GameObject drawerFluids;
	public GameObject drawerDefibPads;
	public GameObject drawerVenflon;
	public GameObject drawerBVM;
	public GameObject drawerNRBMask;
	public GameObject drawerYankeur;
	public GameObject drawerGuedel;
	public GameObject drawerABG;
	public GameObject drawerVacutainers;
	public GameObject animationObject;
	public GameObject satsGenerator;
	public GameObject respGenerator;
	public GameObject aLineGenerator;
	public GameObject examinationButtons;
    public GameObject examineResponseButton;
    public GameObject examineAirwayButton;
	public GameObject airwayManoeuvresButton;
	public GameObject headTiltChinLiftButton;
	public GameObject jawThrustButton;
	public GameObject examineBreathingButton;
	public GameObject examineCirculationButton;
	public GameObject examineDisabilityButton;
	public GameObject examineExposureButton;
	public GameObject subtitlesBar;
	public GameObject nibpButton;
	public GameObject startResumeText;
	public GameObject hsTsTextObject;
	public GameObject screenMask;
	public GameObject rhythmPracticeCanvas;
	public GameObject rhythmPracticeMessageScreen;
	public GameObject rhythmPracticeOKButton;
	public GameObject rhythmPracticeButtonPanel;
	public GameObject rhythmPracticeMessageHolder;
	public GameObject abgCanvas;
	public GameObject abgMessageScreen;
	public GameObject abgButtonPanel;
	public GameObject shockOrNotCanvas;
	public GameObject shockOrNotMessageScreen;
	public GameObject shockOrNotOKButton;
	public GameObject shockOrNotButtonPanel;
	public GameObject shockOrNotMessageHolder;
	public GameObject cprTimer;
	public GameObject menuIcon;
	public GameObject aiMenu;
	public GameObject topBar;
	public GameObject bottomBar;
	public GameObject nsrButton;
	public GameObject mobitzIButton;
	public GameObject mobitzIIButton;
	public GameObject afButton;
	public GameObject aFlutterButton;
	public GameObject svtButton;
	public GameObject bigeminyButton;
	public GameObject vtButton;
	public GameObject vfButton;
	public GameObject chbButton;
	public GameObject torsadesButton;
	public GameObject resumeGameButton;
	public GameObject shockIcon;
	public GameObject notIcon;
	public GameObject pauseQuit;
	public GameObject pauseReload;
	public GameObject pauseResume;
	public GameObject submitFBbutton;
	public GameObject shockOrNotSplash;
	public GameObject defibPaceButtons;
	public GameObject cprObject;
	public GameObject canvasBVM;
	public GameObject arrestTrolley;
	public GameObject defibObject;
	public GameObject messageScreenOkButton;
    public GameObject buttonPushCPR;

	//public FBholder fbHolder;

	private Control control;

	public AnimationTesting animationTesting;

	public Patient patient;

    public PatientSerializer serializer;

	public Text failText;
    public Text timeOffChestText;
	public Text resumeText;
	public Text checkRhythmText;
	public Text hsTsHintText;
	public Text messageText;
	public Text subtitles;
	public Text demoEndText;
	public Text rhythmPracticeMessageText;
	public Text abgMessageText;
	public Text shockOrNotMessageText;
	public Text cprTimerText;
	public Text shockOrNotTimer;
	public Text shockOrNotRoundsText;
	public Text actionMenuText;
	public Text examinationMenuText;
	public Text cprMenuText;
    public Text signsOfLifeText;

	private Text [] monitorTexts;

    public float secondsOffChest = 0f;
    public float minsOffChest = 0f;
    public float averageSecondsOffChest = 0f;
	public float rhythmChecks = 0;
	private float drawerStart = 0f;
	public float MAP;
	public float heartRate;
	private float topButtonY;
	private float canvasLerpTimeStamp = 0f;
	private float airwayButtonTimeStamp = 0f;
	private float cprTimeSecs = 0f;
	private float shockOrNotTotal = 0f;
	private float shockOrNotRounds = 0f;
	private float posX = 0f;
	private float cprQualityTimer = 1f;
	private float deltaTime = 0f;

    public bool offChest = false;
	private bool airwayDrawerOpen = false;
	private bool airwayDrawerOpening = false;
	private bool airwayDrawerClosing = false;
	private bool breathingDrawerOpen = false;
	private bool breathingDrawerOpening = false;
	private bool breathingDrawerClosing = false;
	private bool circulationDrawerOpen = false;
	private bool circulationDrawerOpening = false;
	private bool circulationDrawerClosing = false;
	private bool drugDrawerOpen = false;
	private bool drugDrawerOpening = false;
	private bool drugDrawerClosing = false;
	private bool drawersAllClosed = true;
	private bool hsTsHintTextShown = false;
	public bool defibFocus = false;
	public bool randomRhythm = false;
	public bool cardiacArrest = false;
	private bool canvasSlideRight = false;
	private bool canvasSlideLeft = false;
	private bool canvasSlideRightExaminationOnly = false;
	private bool canvasSlideLeftExaminationOnly = false;
	private bool viewingMonitor = false;
	public bool fluidsGiven = false;
	private bool adrenalineThisCycle=false;
	private bool amiodaroneThisCycle = false;
	private bool doubleClickBlocker=false;
	public bool remoteConnected = false;
	public bool shockFail = false;
	private bool airwayCleared = false;
	private bool baggingStarted = false;
	private bool playStarted = false;
	private bool fluidsRunning = false;
	private bool dontSuspendCanvas = false;
	public bool mapChanging = false;
	private bool atropineRunning = false;
	private bool drawersOpeningArrestDebug = false;
	private bool activateRhythmPractice = false;
	private bool rhythmPracticeCorrect = false;
	private bool activateShockOrNot = false;
	private bool activateABGpractice = false;
	public bool abgPracticeCorrect = true;
	public bool canvasResizeDone = false;
	private bool timerWasActive = false;
	private bool shockOrNotRunning = false;
	public bool shockOrNotWaiting = false;
	public bool rhythmPracticeWaiting = false;
	private bool bvmDeactivatedTemp = false;
	public bool retrying = false;
	private bool firstRun = true;
	public bool defibView = false;
	public bool actualCPR = false;
	private bool cprMidwayPassed = false;
	public bool cprThirdwayPassed = false;
	public bool largeScreen = false;
	public bool soundOn = true;
    private bool signs_of_life_check = false;

	private string averageTimeOffChestString = "00.00";
	public string rhythm;
	public string scene;
	private string arrestCause = "";
	public string abgType = "";
	private string lastCanvas = "game";
	public string platform = "Desktop";
    
    private Vector3 drugDrawerOpenPos;
    private Vector3 drawerClosedPos;
	private Vector3 originalCamPos;
	private Vector3 examinationButtonsOnScreen;
	private Vector3 examinationButtonsOffScreen;
	private Vector3 menuIconPos;
	public Vector3 trolleyPos;
	public Vector3 defibPos;

    public Camera mainCam;
    public Camera defibCam;
    public Camera drawerCam;
	public Camera monitorCam;
	public Camera rhythmCam;
	public Camera cprCam;
    public Camera signsOfLifeCam;

    public List<GameObject> buttonList;
	private List<GameObject> examinationButtonList;
    private List<string> rhythmList;
    private List<float> heartRateList;
    private List<float> MAPList;
	private List<string> messageList;
	private List<GameObject> defibButtons;

    public Dictionary<float, string> clinical_information_obtained = new Dictionary<float, string>();
    public Dictionary<float, string> patient_state = new Dictionary<float, string>();

    public int atropineDose = 0;
    private int rhythmNumber = 4;
	public int numberOfCorrectCycles = 0;
	public int numberOfShockableCycle = 0;
	private int numberOfAdrenalines = 0;
	private int adrenalineDueCycle = 0;
	private int amiodaroneDueCycle = 0;
	public int failedCycles = 0;
	public int failedShocks = 0; //For non-arrest shocks
	private int cameraMask = 1;
	private int drugFails = 0;
	private int drugFailsNCPR = 0;
	public int gcs = 15;
	public int respRate = 30;
	public int sats = 100;
	public int messageNumber = 0;
	private int cprTimeMins = 0;
	private int rhythmPracticeCorrectInt = 0;
	public int prevCPRheight = 1;
	private int compressionsCompleted = 0;
    
    private Quaternion originalCamRot;

	public ABGGenerator abgGenerator;

	public ScreenSwitchIPhone screenSwitcher;

	public float masterTimeScale = 1f;

	private bool clickable;
	public bool Clickable {
		get {
			return clickable;
		}
		set {
			clickable = value;
			ClickabilityChanged?.Invoke(this, clickable);
		}
	}

	public event EventHandler<Feedback> FeedbackDispatched;
	public event EventHandler<Insights> InsightDispatched;
	public event EventHandler<Measurement> MeasurementDispatched;
	public event EventHandler<string> MemoDispatched;
	public event EventHandler<bool> ClickabilityChanged;

	public void DispatchFeedback(Feedback feedback) {
		FeedbackDispatched?.Invoke(this, feedback);
	}

	public void DispatchInsight(Insights insight) {
		InsightDispatched?.Invoke(this, insight);
	}

	public void DispatchMeasurement(Insights measurable, float value) {
		DispatchInsight(measurable);

		MeasurementDispatched?.Invoke(this, new Measurement(measurable, value));
	}

	public void DispatchMemo(string memo) {
		MemoDispatched?.Invoke(this, memo);
	}

	private void DispatchECG() {
		DispatchMeasurement(Insights.MeasuredHeartRate, heartRate);
		switch (rhythmNumber) {
			case 0:
				DispatchInsight(Insights.HeartRhythm0);
				break;
			case 1:
				DispatchInsight(Insights.HeartRhythm1);
				break;
			case 2:
				DispatchInsight(Insights.HeartRhythm2);
				break;
			case 3:
				DispatchInsight(Insights.HeartRhythm3);
				break;
			case 4:
				DispatchInsight(Insights.HeartRhythm4);
				break;
		}
	}

	private void DisplayAIMenu() {
		if (scene == "Conscious" || scene == "Unconscious") {
			aiMenu.SetActive(true);
		}
	}

    // Use this for initialization
    void Start() {
		largeScreen = screenSwitcher.largeScreen;

		if (debugging) {
			Debug.Log ("Hub starting");
		}

		messageList = new List<string> ();

		Pause();

		Clickable = false;

		SetPatientFactors ();

        originalCamPos = mainCamera.transform.position;
		originalCamRot = mainCam.transform.rotation;

        drugDrawerOpenPos = new Vector3(-0.448f, 0f, 0f);
        drawerClosedPos = new Vector3(0f, 0f, 0f);

        patient = patientObject.GetComponent<Patient>();
        scene = patient.scene;

        stable_patient = patient.stable_patient;

        if (debugging)
        {
            Debug.Log("Hub scene: " + scene);
        }

        rhythmList = patient.rhythmList;
        heartRateList = patient.heartRateList;

        CardiacArrestZeroMAP();

        control = controller.GetComponent<Control>();

        buttonList = new List<GameObject>();
        buttonList.Add(attachPadsButton);
        buttonList.Add(checkRhythmButton);
        //buttonList.Add(defibOnCanvasButton);
        buttonList.Add(iVaccessButton);
        buttonList.Add(fluidsButton);
		buttonList.Add (airwayButton);
		buttonList.Add (breathingButton);
		buttonList.Add (circulationButton);
        buttonList.Add(drugsButton);
		buttonList.Add (drugsButtonNCPR);
		buttonList.Add (monitorButton);
		buttonList.Add (monitorButtonNCPR);
		buttonList.Add (useDefibButton);
		buttonList.Add (chestCompressionButton);
		buttonList.Add (baggingDuringCPRButton);
		buttonList.Add (doneButton);
		buttonList.Add (resumeButton);
		buttonList.Add(signsOfLifeButton);

		examinationButtonList = new List<GameObject> ();
        examinationButtonList.Add(examineResponseButton);
        examinationButtonList.Add (examineAirwayButton);
		//examinationButtonList.Add (airwayManoeuvresButton);
		examinationButtonList.Add (examineBreathingButton);
		examinationButtonList.Add (examineCirculationButton);
		examinationButtonList.Add (examineDisabilityButton);
		examinationButtonList.Add (examineExposureButton);

		defibButtons = new List<GameObject> ();
		defibButtons.Add (defibOnDefibButton);
		defibButtons.Add (control.chargeButton);
		defibButtons.Add (control.paceButton);
		defibButtons.Add (control.syncButton);
		defibButtons.Add (control.pacePauseButton);


		examinationButtonsOnScreen = examinationButtons.transform.position;
		examinationButtonsOffScreen = new Vector3 (Screen.width + Screen.width/7f, examinationButtonsOnScreen.y, examinationButtonsOnScreen.z);

        topButtonY = attachPadsButton.transform.position.y;

		subtitlesBar.SetActive (true);

        if (scene != "AI")
        {
            gameCanvas.SetActive(true);
        }

		posX = Screen.width / 7f;

		/*Vector3 newby = new Vector3 (1230f,
			750f, menuIcon.transform.position.z);*/
		Resizer ();

		CPRButtons.transform.position = new Vector3 (posX, CPRButtons.transform.position.y, CPRButtons.transform.position.z);
		nonCPRButtons.transform.position = new Vector3 (posX, nonCPRButtons.transform.position.y,
			nonCPRButtons.transform.position.z);
		examinationButtons.transform.position = examinationButtonsOnScreen;

		if (scene == "CPR") {
			CPRButtons.SetActive (true);
			nonCPRButtons.SetActive (false);
			examinationButtons.SetActive (false);
			cardiacArrest = true;
			string message = "You are the first ALS provider on scene at a cardiac arrest call on a medical ward. " +
			                 "You have confirmed cardiac arrest, CPR is ongoing. The rest of the team are en route.\n\n" +
			                 "Manage the arrest as best you can until they get here. ";
			DispatchMemo(message);
			messageText.text = message;
			CPRButtons.transform.position = new Vector3 (posX, CPRButtons.transform.position.y, CPRButtons.transform.position.z);
		} else if (scene == "DemoCPR") {
			DeactivateCameras ();
			cprCam.enabled = true;
			animationTesting.debugText.gameObject.SetActive (true);
            patient.arrest = true;
            patient.conscious = false;
            actualCPR = true;
            if (SystemInfo.supportsAccelerometer) {
				Debug.Log ("Supports Accelerometer");
            } else {
				cprObject.SetActive (false);
			}
			drawerBVM.SetActive (false);
			defibPaceButtons.SetActive (false);
			//CPRButtons.SetActive (true);
			nonCPRButtons.SetActive (false);
			examinationButtons.SetActive (false);
			cardiacArrest = true;
			MAP = 0f;
			string message = "You attend a cardiac arrest call on a medical ward. " +
			                 "You have confirmed cardiac arrest, and commenced CPR. Someone is getting the arrest trolley and you " +
			                 "need to perform effective CPR until the trolley arrives.\n\nTo deliver a chest compression ";
			if (SystemInfo.supportsAccelerometer) {
                message += "push your device downwards 5-6cm and pull it upwards again.\n\n";
                message += "If this doesn't work well on your device, switch to button-push CPR using the icon in the bottom right corner of the screen.\n\n";
			} else {
				message += "push the spacebar on your keyboard. ";
			}
			message += "Select the bag/valve mask icon to deliver a breath. Good luck! ";
			DispatchMemo(message);
			messageText.text = message;
			CPRButtons.transform.position = new Vector3 (posX, CPRButtons.transform.position.y, CPRButtons.transform.position.z);
			//If randomRhythm is true, MAP will automatically be zero for first three cycles
			//randomRhythm = true;
		} else if (scene == "Conscious" || scene == "Unconscious") {
            if (debugging)
            {
                Debug.Log("Starting scene...");
            }
			/*Vector3 newby = new Vector3 (640f,
				750f, menuIcon.transform.position.z);
			menuIcon.transform.position = newby;*/
			checkRhythmText.text = "Pulse check";
			CPRButtons.SetActive (false);
			//nonCPRButtons.SetActive (true);
			//examinationButtons.SetActive (true);
			cardiacArrest = false;
			string message = "Welcome to the app! ";
			messageText.text = message;
            message = patient.leadIn;
			messageList.Add (message);
			message = "For the purposes of this exercise, being \"stabilised\" means that John will have a clear airway," +
			" oxygen saturation of at least 88%, a respiratory rate of at least 8 breaths per minute and " +
			"a mean arterial pressure of at least 60mmHg. ";
			messageList.Add (message);
			message = "What's more, if John's sats fall below 65% or if his mean arterial pressure falls below 20mmHg, " +
			"he will go into cardiac arrest.\n\nGood luck! ";
			messageList.Add (message);
			nonCPRButtons.transform.position = new Vector3 (posX, nonCPRButtons.transform.position.y,
				nonCPRButtons.transform.position.z);
			examinationButtons.transform.position = examinationButtonsOnScreen;
		} else if (scene == "RhythmPractice") {
			DeactivateCameras ();
			Debug.Log("RhythmPracticeActivated");
			defibCam.enabled = true;
			activateRhythmPractice = true;
		} else if (scene == "ShockOrNot") {
			DeactivateCameras ();
			defibCam.enabled = true;
			activateShockOrNot = true;
		} else if (scene == "abgPractice") {
			activateABGpractice = true;
		} else if (scene == "Training")
        {
            if (debugging)
            {
                Debug.Log("Starting scene...");
            }
            /*Vector3 newby = new Vector3 (640f,
				750f, menuIcon.transform.position.z);
			menuIcon.transform.position = newby;*/
            checkRhythmText.text = "Pulse check";
			DispatchMemo(checkRhythmText.text);
            CPRButtons.SetActive(false);
            //nonCPRButtons.SetActive (true);
            //examinationButtons.SetActive (true);
            cardiacArrest = false;
            messageList.Clear();
            string message = patient.leadIn;
            messageText.text = message;
            nonCPRButtons.transform.position = new Vector3(posX, nonCPRButtons.transform.position.y,
                nonCPRButtons.transform.position.z);
            examinationButtons.transform.position = examinationButtonsOnScreen;
            doneButton.GetComponentInChildren<Text>().text = "Done";
        }
		playStarted = true;

		ArrangeButtons ();

		NextRhythm ();

        if (debugging)
        {
            Debug.Log("Variables set, leaving menu");
        }

		ReturnFromMenu ();

		if (debugging) {
			Debug.Log ("Start function finished");
		}
        //The intro with the cardiac arrest call. Disabled for testing.
        /*cameraMask = mainCam.cullingMask;
        messageScreen.SetActive(false);
        mainCam.cullingMask = 0;
        PlaySound("CardiacArrestCall");*/
        if (debugging)
        {
            Debug.Log("Heart Rate: " + heartRate + " Rhythm: " + rhythm);
        }

		if (nonBlockingMode) {
			messageScreen.active = false;
			OnMessageRead();
			ArrangeButtons ();
		}
    }

	public void ABG() {
		if (Clickable) {
			string message = "You take blood for an ABG. ";
			drawerABG.SetActive (false);
			SendMessage (message, 0, 2, true);
		}
	}

	public void ABGpractice (string thisType) {
		if (debugging) {
			Debug.Log ("Abg: " + thisType + " type: " + abgType);
		}
		abgButtonPanel.SetActive (false);
		menuIcon.SetActive (false);
		aiMenu.SetActive (false);
		if (abgType == thisType) {
			abgPracticeCorrect = true;
			abgMessageText.text = "Excellent! ";
		} else {
			abgPracticeCorrect = false;
			abgMessageText.text = "Not quite! Try again... ";
		}
		abgMessageScreen.SetActive (true);
	}

	public void ABGpracticeOK () {
		abgButtonPanel.SetActive (true);
		abgMessageScreen.SetActive (false);
		menuIcon.SetActive (true);
		DisplayAIMenu();
		if (abgPracticeCorrect) {
			abgType = abgGenerator.Generate ();
		}
	}

    void ActivateABGPractice () {
		abgType = abgGenerator.Generate ();

		menuIcon.transform.position = new Vector3 (menuIcon.transform.position.x, Screen.height / 3f, menuIcon.transform.position.z);;
		menuIcon.SetActive (true);

		DisplayAIMenu();

		gameCanvas.SetActive (false);
		abgCanvas.SetActive (true);
		activateABGpractice = false;
	}

	public void ActivateDeactivateFailScreen(bool trueFalse)
	{
		failScreen.active = trueFalse;
		if (cardiacArrest) {
			CPRButtons.active = !trueFalse;
		} else {
			nonCPRButtons.SetActive (!trueFalse);
		}
		if (trueFalse)
		{
			DispatchFeedback(Feedback.Failure);
			Pause();
		}
		else
		{
			if (timerWasActive && !cprTimer.activeSelf) {
				cprTimer.SetActive(true);
			}
			soundPlayer.GetComponent<SoundPlayer>().stopSounds = false;
			Unpause();
			if ((nonCPRButtons.transform.position.x == 0f && nonCPRButtons.activeSelf)
			    || (CPRButtons.transform.position.x == 0f && CPRButtons.activeSelf)) {
				CanvasSlider ();
			}
		}
	}

	void ActivateRhythmPractice () {
		cardiacArrest = true;
		AttachPads ();
		control.DefibReady ();
		gameCanvas.SetActive (false);
		rhythmPracticeCanvas.SetActive (true);
		Unpause();
		StartCoroutine (RhythmPracticeStarter ());
		activateRhythmPractice = false;
		DeactivateDefibButtons ();
	}

	void ActivateShockOrNot () {

		cardiacArrest = true;
		AttachPads ();
		control.DefibReady ();
		gameCanvas.SetActive (false);
		shockOrNotSplash.SetActive (true);
		Unpause();
		StartCoroutine (ShockOrNotStarter ());
		activateShockOrNot = false;
		DeactivateDefibButtons ();
	}

    public void AddClinicalInformation(string button_pressed)
    {
        if (button_pressed == "Airway")
        {
            if (patient.airwayObstructed)
            {
                clinical_information_obtained.Add(Time.time, "Airway_obstructed");
            }
            else
            {
                clinical_information_obtained.Add(Time.time, "Airway_clear");
            }
        } else if (button_pressed == "Breathing")
        {
            clinical_information_obtained.Add(Time.time, "Breathing_examination");
        } else if (button_pressed == "Circulation")
        {
            clinical_information_obtained.Add(Time.time, "Circulation_examination");
        } else if (button_pressed == "Disability")
        {
            clinical_information_obtained.Add(Time.time, "Disability_examination");
        } else if (button_pressed == "Exposure")
        {
            clinical_information_obtained.Add(Time.time, "Exposure_examination");
        } else if (button_pressed == "Monitor")
        {
            if (control.aLineScript.aLineOn || control.aLineScript.bpCuffOn)
            {
                clinical_information_obtained.Add(Time.time, "BP");
            }
            if (control.monitorPadsOn)
            {
                clinical_information_obtained.Add(Time.time, "ECG");
            }
            if (control.satsScript.satsOn)
            {
                clinical_information_obtained.Add(Time.time, "Sats");
            }
            if (control.respScript.respOn)
            {
                clinical_information_obtained.Add(Time.time, "Resps");
            }
        } else if (button_pressed == "Defib")
        {
            if (animationTesting.defibPads.activeSelf && control.defibReady)
            {
                clinical_information_obtained.Add(Time.time, "ECG");
            }
        } else if (button_pressed == "Bloods")
        {
            clinical_information_obtained.Add(Time.time, "Bloods");
        }
    }

    public string AddPatientState ()
    {
        /*The string in this dictionary is a CSV format entry corresponding to the following column headers:
         * Cardiac arrest (bool)
         * Cardiac rhythm (string)
         * Heart rate (int)
         * Conscious (bool)
         * Airway obstructed (bool)
         * Sats (int)
         * Oxygen responsiveness (int)
         * Resp rate (int)
         * Wheeze (bool)
         * Pulmonary oedema (bool)
         * Pneumonia (0 = no, 1 = right, 2 = left)
         * Pneumonothorax (0 = no, 1 = right, 2 = left)
         * Pleural effusion (0 = no, 1 = right, 2 = left)
         * Bronchodilator responsiveness (int)
         * pH (float)
         * pO2 (float)
         * pCO2 (float)
         * Bicarb (float)
         * Lactate (float)
         * K (float)
         * Hb (float)
         * Heart rate (int)
         * MAP (int)
         * Sys BP (int)
         * Dias BP (int)
         * Fluid responsiveness (int)
         * Fluid refractoriness (int)
         * Cap refil peripheral (int)
         * Cap refil central (int)
         * Pacing threshold mV (int)
         * Atropine responsiveness (int)
         * AVPU (string)
         * Pupils (0 = NAD, 1 = right blown, 2 = left blown)
         * Glucose (float)
         * Temperature (float)
         * Exposure (string)
         */
        string patient_state_string = "";
        patient_state_string += AddPatientStateBoolHelper(cardiacArrest) + ",";
        patient_state_string += rhythm + ",";
        patient_state_string += heartRate + ",";
        patient_state_string += AddPatientStateBoolHelper(patient.conscious) + ",";
        patient_state_string += AddPatientStateBoolHelper(patient.airwayObstructed) + ",";
        patient_state_string += sats + ",";
        patient_state_string += patient.oxygenResponse + ",";
        patient_state_string += respRate + ",";
        patient_state_string += AddPatientStateBoolHelper(patient.wheezy) + ",";
        patient_state_string += AddPatientStateBoolHelper(patient.pulmonaryOedema) + ",";
        patient_state_string += patient.pneumoniaSide + ",";
        patient_state_string += patient.pneumothoraxSide + ",";
        patient_state_string += patient.pleuralEffusionSide + ",";
        patient_state_string += patient.nebResponsiveness + ",";
        patient_state_string += patient.pH + ",";
        patient_state_string += patient.pO2 + ",";
        patient_state_string += patient.pCO2 + ",";
        patient_state_string += patient.HCO3 + ",";
        patient_state_string += patient.lactate + ",";
        patient_state_string += patient.K + ",";
        patient_state_string += patient.hB + ",";
        patient_state_string += MAP + ",";
        patient_state_string += control.aLineScript.bpSys + ",";
        patient_state_string += control.aLineScript.bpDias + ",";
        patient_state_string += patient.fluidResponsiveness + ",";
        patient_state_string += patient.fluidRefractoriness + ",";
        patient_state_string += patient.crtPeripheral + ",";
        patient_state_string += patient.crtCentral + ",";
        patient_state_string += patient.pacingThreshold + ",";
        patient_state_string += patient.atropineResponsiveness + ",";
        patient_state_string += patient.AVPU + ",";
        patient_state_string += patient.pupils + ",";
        patient_state_string += patient.glucose + ",";
        patient_state_string += patient.temperature + ",";
        patient_state_string += patient.exposureFindings + ",";
        return patient_state_string;
    }

    int AddPatientStateBoolHelper (bool b)
    {
        if (b)
        {
            return 1;
        } else
        {
            return 0;
        }
    }

    public void AddRhythmCheck()
	{
		//This condition stops the message "There is a pulse!" displaying on first rhythm check if in VF
		//Hereafter, the "Control" script will ensure MAP==0 if pt in cardiac arrest
		if (rhythm == "vf") {
			MAP = 0f;
		}
		rhythmChecks++;

		if (debugging) {
			Debug.Log ("Time off chest: " + secondsOffChest + ", rhythm checks: " + rhythmChecks);
		}

		if (MAP <= 0f) {
			SendMessage ("\"There's no pulse!\"", 2, 2, false);
			if (numberOfCorrectCycles == 0 && failedCycles == 0) {
				StartCoroutine (SoundWithDelay ("NoPulse01", 2));
			} else {
				StartCoroutine (SoundWithDelay ("NoPulse02", 2));
			}
		} else if (scene == "DemoCPR" || scene == "CPR") {
			SendMessage ("\"There is a pulse!\"", 2, 2, false);
			//FOR PURPOSES OF DEMO VERSION
			DemoEnd ();
		} else {
			CardiacArrestEnder ();
		}
	}

	public void AdenosineGiven() {
		if (!doubleClickBlocker) {
			string message = "";
			if (cardiacArrest) {
				message = "Adenosine isn't a drug you need during cardiac arrest. ";
				SendMessage (message, 0, 2, true);
				drugFails++;
				DispatchFeedback(Feedback.Blunder);
			} else if (patient.diagnosis == "Asthma") {
				message = "One of your colleagues points out that you should never give adenosine to asthmatics. ";
				SendMessage (message, 0, 2, true);
				drugFailsNCPR++;
				DispatchFeedback(Feedback.Blunder);
			} else if (drawerDefibPads.activeSelf || !control.defibReady) {
				message = "If you're going to give adenosine, better attach some pads and switch the defib on "
				+ "so you can see what happens! ";
				SendMessage (message, 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
			} else if (drawerVenflon.activeSelf) {
				message = "The patient has no line for giving IV drugs! ";
				SendMessage (message, 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
			} else {
				useDefibButton.GetComponent<Button> ().onClick.Invoke ();
				StartCoroutine (AdenosineTimer ());
				SendMessage ("\"Giving adenosine.\"", 0, 3, false);
			}
		}
	}

	//This function checks correct administration for first 3 shocks.
	//Thereafter, adrenaline is simply due every other cycle.
	public void AdrenalineChecker()
	{
		if (debugging) {
			Debug.Log ("Starting adrenaline checker");
		}
		if (MAP == 0f)
		{
			if (debugging) {
				Debug.Log ("MAP = 0 from adrenaline checker");
			}
			bool shockable = false;

			numberOfCorrectCycles++;

			cprTimeMins = 2*(numberOfCorrectCycles-1);
			cprTimeSecs = 0f;

			if (numberOfCorrectCycles == 1) {
				cprTimer.SetActive (true);
			}

			adrenalineThisCycle = false;
			amiodaroneThisCycle = false;

			if (debugging) {
				Debug.Log ("Adrenaline checker. Rhythm: " + rhythm);
			}
			if (control.rhythm == "vt" || control.rhythm == "vf") //To avoid confusion with, e.g. torsades
			{
				shockable = true;
				numberOfShockableCycle++;
				if (numberOfShockableCycle==3||numberOfShockableCycle==6||numberOfShockableCycle==9)
				{
					amiodaroneDueCycle = numberOfCorrectCycles;
				}
			}

			if (numberOfCorrectCycles == 1 && !shockable)
			{
				adrenalineDueCycle = 1;
			}
			else if (numberOfCorrectCycles == 2 && !shockable && numberOfAdrenalines == 0 && adrenalineDueCycle == 0)
			{
				adrenalineDueCycle = 2;
			}
			else if (numberOfCorrectCycles == 3 && numberOfAdrenalines == 0 && adrenalineDueCycle == 0)
			{
				adrenalineDueCycle = 3;
			}
		}
	}

	public void AdrenalineGiven()
	{
		if (!doubleClickBlocker) {
			string messageText = "";
			if (!cardiacArrest) {
				messageText = "Giving boluses of IV adrenaline to a patient who's not in cardiac arrest might be okay for anaesthetists, " +
				"but mere mortals like us should avoid it as a rule. ";
				drugFailsNCPR++;
				DispatchFeedback(Feedback.Blunder);
			} else {
				if (adrenalineThisCycle) {
					messageText = "You've already given adrenaline this cycle! ";
					DispatchFeedback(Feedback.Blunder);
				} else if (numberOfCorrectCycles == 0) {
					messageText = "Oops! Let's wait until after the first successful rhythm check before giving adrenaline! ";
					drugFails++;
					DispatchFeedback(Feedback.Blunder);
				} else if (adrenalineDueCycle == 0) {
					messageText = "Oops! Adrenaline should be given after the third cycle " +
					"while you're on the shockable side of the algorithm. " +
					"We'll give it now, but remember to give it every other cycle from now on, even if you switch between " +
					"the shockable and non-shockable sides of the algorithm. ";
					numberOfAdrenalines++;
					adrenalineDueCycle = numberOfCorrectCycles + 2;
					drugFails++;
					adrenalineThisCycle = true;
					DispatchFeedback(Feedback.Blunder);
				} else if (numberOfCorrectCycles == adrenalineDueCycle) {
					messageText = "Excellent! Adrenaline administered appropriately. ";
					adrenalineDueCycle = numberOfCorrectCycles + 2;
					numberOfAdrenalines++;
					adrenalineThisCycle = true;
				} else if (numberOfCorrectCycles == (adrenalineDueCycle + 1)) {
					messageText = "Oops! You were supposed to give adrenaline at the last cycle. " +
					"We'll give it now, but remember to give it every other cycle from now on, even if you switch between " +
					"the shockable and non-shockable sides of the algorithm. ";
					numberOfAdrenalines++;
					adrenalineDueCycle = numberOfCorrectCycles + 2;
					drugFails++;
					adrenalineThisCycle = true;
					DispatchFeedback(Feedback.Blunder);
				} else if (numberOfCorrectCycles == (adrenalineDueCycle - 1)) {
					messageText = "Oops! You were supposed to give adrenaline at the next cycle. " +
					"We'll give it now, but remember to give it every other cycle from now on, even if you switch between " +
					"the shockable and non-shockable sides of the algorithm. ";
					numberOfAdrenalines++;
					adrenalineDueCycle = numberOfCorrectCycles + 2;
					drugFails++;
					adrenalineThisCycle = true;
					DispatchFeedback(Feedback.Blunder);
				} else {
					messageText = "Oops! You were supposed to give adrenaline at cycle " + adrenalineDueCycle +
					". This is cycle " + numberOfCorrectCycles + ". " +
					"We'll give it now, but remember to give it every other cycle from now on, even if you switch between " +
					"the shockable and non-shockable sides of the algorithm. ";
					adrenalineDueCycle = numberOfCorrectCycles + 2;
					drugFails++;
					adrenalineThisCycle = true;
					DispatchFeedback(Feedback.Blunder);
				}
			}

			SendMessage (messageText, 0, 8, true);
		}
	}

	public void AirwayManoeuvresButton () {
		if (patient.conscious) {
			string message = "You think about trying some airway manoeuvres, but decide your conscious patient " +
			                 "probably wouldn't appreciate it. ";
			SendMessage (message, 0, 2, true);
			DispatchFeedback(Feedback.Blunder);
			airwayManoeuvresButton.SetActive (false);
			ArrangeButtons ();
		} else {
			headTiltChinLiftButton.SetActive (true);
			jawThrustButton.SetActive (true);
			airwayButtonTimeStamp = Time.time;
		}
	}

	void AirwayManoeuvresButtonsMove () {
		if (headTiltChinLiftButton.activeSelf) {
			float fraction = (Time.time - airwayButtonTimeStamp) / 0.2f;
			Vector3 startPos = new Vector3 (0f, 0f, 0f);
			Vector3 endPos = new Vector3 (-180f, 30f, 0f);
			if (fraction < 1f) {
				startPos = new Vector3 (0f, 0f, 0f);
				endPos = new Vector3 (-180f, 80f, 0f);
				headTiltChinLiftButton.transform.localPosition = Vector3.Lerp (startPos, endPos, fraction);
				endPos = new Vector3 (-180f, -80f, 0f);
				jawThrustButton.transform.localPosition = Vector3.Lerp (startPos, endPos, fraction);
			} else if (headTiltChinLiftButton.transform.localPosition != endPos) {
				headTiltChinLiftButton.transform.localPosition = new Vector3 (-180f, 80f, 0f);
				jawThrustButton.transform.localPosition = new Vector3 (-180f, -80f, 0f);
			}
		}
	}

	public void ALineOn () {
		control.aLineScriptToComeOn = true;
		control.aLineScript.bpText.text = "WAIT";
		drawerAline.SetActive (false);
		if (nibpButton.activeSelf) {
			nibpButton.SetActive (false);
		}
	}

	public void AmiodaroneGiven()
	{
		if (!doubleClickBlocker) {
			string messageText = "";
			if (!cardiacArrest) {
				if (drawerVenflon.activeSelf) {
					messageText = "The patient has no line for giving IV drugs! ";
					DispatchFeedback(Feedback.Blunder);
				} else if (patient.diagnosis == "Arrhythmia" && !patient.bradyCardia && rhythmNumber == 1) {
					messageText = "You give 300mg IV amiodarone (in 5% dextrose over 30 minutes)." +
					"\n\n(Check the monitor to see if it worked.)";
					int chance = UnityEngine.Random.Range (0, 5);
					if (chance == 0) {
						NextRhythm ();
					}
				} else if (heartRate < 60f) {
					messageText = "Oops! At the last minute, one of your colleagues points out that you probably " +
					"shouldn't give amiodarone to your bradycardic patient... ";
					drugFailsNCPR++;
					DispatchFeedback(Feedback.Blunder);
				} else if (rhythm == "nsr") {
					messageText = "You give amiodarone to John, who is normal sinus rhythm."
					+ " Surprisingly enough, nothing happens. ";
					drugFailsNCPR++;
					DispatchFeedback(Feedback.Blunder);
				}
			} else if (amiodaroneThisCycle) {
					messageText = "You've already given amiodarone this cycle! ";
					amiodaroneDueCycle = 0;
					DispatchFeedback(Feedback.Blunder);
			} else if (amiodaroneDueCycle == numberOfCorrectCycles && amiodaroneDueCycle != 0) {
				messageText = "Excellent! Amiodarone appropriately administered. ";
				amiodaroneThisCycle = true;
				amiodaroneDueCycle = 0;
			} else {
				messageText = "Oops! Amiodarone should be given after the third shock. You've given " + numberOfShockableCycle +
				" appropriate shocks. ";
				if (numberOfShockableCycle < 3f && numberOfShockableCycle > 0f) {
					messageText += " You're a bit early! ";
				}
				drugFails++;
				DispatchFeedback(Feedback.Blunder);
			}
			SendMessage (messageText, 0, 8, true);
		}
  	}

	public void ArrangeButtons()
	{
		float height = Screen.height;
		float width = Screen.width/7f;

		RectTransform rt = nonCPRButtons.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (width, 1f);

		rt = examinationButtons.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (width, 1f);

		rt = CPRButtons.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (width, 1f);
		
		float y = topButtonY;
		float gap = 70f;

		for (int i = 0; i < buttonList.Count; i++)
		{
			rt = buttonList [i].GetComponent<RectTransform>();
			if (buttonList [i] == resumeButton) {
				rt.sizeDelta = new Vector2 (width * 0.9f, height / 10f);
			} else {
				rt.sizeDelta = new Vector2 (width * 0.9f, height / 10f);
			}

			if (buttonList[i].active)
			{
				if (debugging) {
					Debug.Log (buttonList [i]);
				}
				buttonList[i].transform.position = new Vector3
					(buttonList[i].transform.position.x, y, buttonList[i].transform.position.z);
				y -= height * 0.11f;
			}
		}

		y = topButtonY;
		for (int i = 0; i < examinationButtonList.Count; i++)
		{
			if (examinationButtonList[i].active)
				rt = examinationButtonList [i].GetComponent<RectTransform>();
				rt.sizeDelta = new Vector2 (width * 0.9f, height/10f);
			{
				examinationButtonList[i].transform.position = new Vector3
					(examinationButtonList[i].transform.position.x, y, examinationButtonList[i].transform.position.z);
				y -= height * 0.11f;
			}
		}
	}

    public void AssessResponse()
    {
        if (!signs_of_life_check)
        {
            string message = "";
            if (patient.conscious)
            {
                message = "You: \"How you feeling John?\"\n\n.John: ";
                List<string> responses = new List<string>();
                responses.Add("\"Overworked and underpaid.\"");
                responses.Add("\"If I were any better, I'd be you.\"");
                responses.Add("\"As fine as a maiden’s flaxen hair.\"");
                responses.Add("\"Living the dream.\"");
                responses.Add("\"Can't complain. Nobody listens to me anyway.\"");
                responses.Add("\"I can't complain! It's against the Company Policy.\"");
                responses.Add("\"If I had a tail, I would wag it.\"");
                responses.Add("\"Better than I was, but not nearly as good as I'm going to be.\"");
                responses.Add("\"Not great, but my hair looks awesome, right?.\"");
                responses.Add("\"I have a pulse, so I must be okay.\"");
                responses.Add("\"All right so far, but there's still time for everything to go horribly wrong.\"");
                responses.Add("\"Do you want the short or long version?\"");
                responses.Add("\"I’m pretty sure I am not obligated to tell you.\"");
                responses.Add("\"Surviving, I guess.\"");
                responses.Add("\"In need of some peace and quiet.\"");
                responses.Add("\"I'm sober. Take from that what you will.\"");
                responses.Add("\"On a scale from one to punching someone in the face? A three.\"");
                responses.Add("\"I'm imagining myself having a fabulous holiday.\"");
                responses.Add("\"Happy and you know it.\"");
                responses.Add("\"In order to answer the question, I need to take you back about ten years. Do you have a moment?\"");
                responses.Add("\"Like I'm living a life of denial and suppressed rage.\"");
                responses.Add("\"Fair to partly cloudy.\"");
                responses.Add("\"Groovy!\"");
                responses.Add("\"Not bad. Could be better. Could be payday.\"");
                responses.Add("\"Oh terrible. But I'm used to it.\"");
                responses.Add("\"What's with all the questions? You from the police?\"");
                responses.Add("\"You go first so we can compare.\"");
                responses.Add("\"Living the dream. But half the time it's a nightmare.\"");
                responses.Add("\"Dangerously close to fabulous.\"");
                responses.Add("\"How much will you pay me if I tell you?\"");
                responses.Add("\"I'm taking over the world.\"");
                responses.Add("\"Rollin' with the punches.\"");
                responses.Add("\"So good I have to sit on my hands to keep myself from clapping.\"");
                responses.Add("\"They told me you would ask me that.\"");
                responses.Add("\"Somewhere between blah and meh.\"");
                responses.Add("\"I’ll let you know when I figure it all out.\"");
                responses.Add("\"Do you want an honest answer or the answer you were expecting?\"");
                responses.Add("\"Different day, same drab existence.\"");
                responses.Add("\"Trying not to burst into tears. I get an A for effort, right?\"");
                responses.Add("\"Just hug me and leave it at that.\"");
                int rand = UnityEngine.Random.Range(0, responses.Count);
                message += responses[rand];

				DispatchInsight(Insights.ResponseVerbal);
            }
            else if (MAP <= 0f)
            {
                message = "You get absolutely no response from John, even to painful stimulus. ";
                examineResponseButton.GetComponent<Text>().text = "Check signs of life";
				signs_of_life_check = true;

				DispatchInsight(Insights.ResponseNone);
            }
            else
            {
                message = "John groans in response to painful stimulus. ";
				DispatchInsight(Insights.ResponseGroan);
            }
            SendMessage(message, 0, 3, true);
        }
        else
        {
            StartCoroutine(SignsOfLifeCheck());
        }
		AddClinicalInformation("Response");
    }

	public void AssessAirway () {
		if (animationTesting.goDoc2.activeSelf) {
			string message = "John is being bagged by one of your colleagues. ";
			SendMessage (message, 0, 2, true);
		}
		else if (patient.conscious) {
			string message = "John is conscious and talking. The airway seems to be fine. ";
			SendMessage (message, 0, 2, true);
			DispatchInsight(Insights.AirwayClear);
		} else if (respRate == 0) {
			DispatchInsight(Insights.BreathingNone);
			if (patient.airwayObstructed && patient.airwayObstruction != "Tongue") {
				string message = "You can see ";
				if (patient.airwayObstruction == "Vomit") {
					message += "vomit";
					DispatchInsight(Insights.AirwayVomit);
				} else {
					message += "blood";
					DispatchInsight(Insights.AirwayBlood);
				}
				message += " at the back of the mouth. You also notice John isn't breathing. ";
				SendMessage (message, 0, 2, true);
			} else {
				string message = "You cannot see anything in the airway. However, John doesn't appear to be breathing. ";
				message += "\n\n(Select the bag-valve-mask from the airway drawer to start bagging him.)";
				SendMessage (message, 0, 2, true);
			}
		} else if (patient.airwayObstructed) {
			if (patient.airwayObstruction == "Tongue") {
				string message = "John is snoring heavily. ";
				SendMessage (message, 0, 2, true);
				DispatchInsight(Insights.BreathingSnoring);
			} else {
				string message = "John is making gurgling noises. ";
				if (patient.airwayObstruction == "Blood") {
					message += "You can see blood at the back of his throat. ";
					DispatchInsight(Insights.AirwayBlood);
				} else if (patient.airwayObstruction == "Vomit") {
					message += "You can see vomit at the back of his throat. ";
					DispatchInsight(Insights.AirwayVomit);
				}
				SendMessage (message, 0, 2, true);
			}
		} else {
			string message = "The airway seems clear. ";
			SendMessage (message, 0, 2, true);
			DispatchInsight(Insights.AirwayClear);
		}
		AddClinicalInformation("Airway");
	}

	public void AssessBreathing () {
		string message = "";
		if (respRate == 0 && !animationTesting.goDoc2.activeSelf) {
			message = "John isn't breathing! ";
			DispatchInsight(Insights.BreathingNone);
		} else if (patient.airwayObstructed && !animationTesting.goDoc2.activeSelf) {
			message = "John seems to be see-saw breathing! ";
			DispatchInsight(Insights.BreathingSeeSaw);
		} else {
			if (animationTesting.goDoc2.activeSelf) {
				if (patient.diagnosis == "Pneumothorax" || patient.diagnosis == "Asthma" || patient.diagnosis == "Anaphylaxis") {
					message = "Your colleague tells you there is a lot of resistance to ventilation. ";
					DispatchInsight(Insights.VentilationResistance);
				} else {
					message = "The patient is being bagged effectively. ";
				}
			} else {
				message = "John is breathing at a rate of " + respRate + " breaths per minute. ";
				DispatchMeasurement(Insights.MeasuredRespRate, respRate);
			}
			if (patient.diagnosis != "Pneumothorax") {
				message += "Chest exapansion is equal. ";
				DispatchInsight(Insights.BreathingEqualChestExpansion);
			}
			if (patient.pulmonaryOedema) {
				message += "There are bibasal crepitations. ";
				DispatchInsight(Insights.BreathingBibasalCrepitations);
			} else if (patient.diagnosis == "Asthma" || patient.diagnosis == "Anaphylaxis") {
				message += "There is wheeze throughout the lung fields. ";
				DispatchInsight(Insights.BreathingWheeze);
			} else if (patient.diagnosis == "Pneumonia") {
				message += "There are coarse crepitations at the right base. ";
				DispatchInsight(Insights.BreathingCoarseCrepitationsAtBase);
			} else if (patient.diagnosis == "Pneumothorax") {
				message += "There is reduced air entry and hyperresonant percussion on the left, " +
				"and you notice the trachea is deviated to the right. ";
				DispatchInsight(Insights.BreathingPneumothoraxSymptoms);
			} else {
				message += "The lung fields sound clear. ";
			}
		}
		if (drawerSatsProbe.activeSelf) {
			message += "\n\n(Select the finger probe from the breathing drawer and hit \"View Monitor\" to see sats.)";
		} else {
			message += "\n\n(Hit \"View Monitor\" to see sats.)";
		}
		SendMessage (message, 0, 2, true);
		AddClinicalInformation("Breathing");
	}

	public void AssessCirculation () {
		string message = "";
		message = "The radial pulse is ";
		if (MAP > 60f) {
			message += "palpable. ";
			DispatchInsight(Insights.RadialPulsePalpable);
		} else {
			message += "too weak to palpate. ";
			DispatchInsight(Insights.RadialPulseNonPalpable);
		}
		if (control.pacing && control.capture) {
			message += "The carotid pulse is palpable. The heart rate is " + control.paceRate.ToString ("0");
			DispatchMeasurement(Insights.MeasuredHeartRate, control.paceRate);
		} else {
			message += "The carotid pulse is palpable. The heart rate is " + heartRate.ToString ("0");
			DispatchMeasurement(Insights.MeasuredHeartRate, heartRate);
		}
		message += "bpm. Heart sounds are ";
		if (patient.diagnosis == "Pneumothorax" || patient.diagnosis == "PericardialEffusion") {
			message += "muffled. ";
			DispatchInsight(Insights.HeartSoundsMuffled);
		} else if (patient.diagnosis == "Asthma" || patient.diagnosis == "Anaphylaxis") {
			message += "difficult to hear over the wheeze. ";
			DispatchInsight(Insights.BreathingWheeze);
		} else {
			message += "normal. ";
			DispatchInsight(Insights.HeartSoundsNormal);
		}
		if (drawerAline.activeSelf && drawerBPCuff.activeSelf) {
			message += "\n\n(To check the BP, choose the pressure cuff or the arterial line from the " +
			"circulation drawer and hit \"View Monitor\".)";
		} else if (drawerCmPads.activeSelf && drawerDefibPads.activeSelf) {
			message += "\n\nHit \"View Monitor\" to see BP.\n\n(You can attach the cardiac monitor and defib pads to see the rhythm if you wish.)";
		} else {
			message += "\n\nHit \"View Monitor\" to see BP. ";
		}
		SendMessage (message, 0, 2, true);
		AddClinicalInformation("Circulation");
	}

	public void AssessDisability () {
		string message = "";
		if (patient.conscious) {
			if (MAP < 50 || sats < 82) {
				message = "AVPU score = V. ";
				DispatchInsight(Insights.AVPU_V);
			} else {
				message = "AVPU score = A. ";
				DispatchInsight(Insights.AVPU_A);
			}
		}else {
			message = "AVPU score = U. ";
			DispatchInsight(Insights.AVPU_U);
		}
		if (patient.diagnosis == "Opiates") {
			message += "Pupils are pinpoint. ";
			DispatchInsight(Insights.PupilsPinpoint);
		} else {
			message += "Pupils equal and reactive. ";
			DispatchInsight(Insights.PupilsNormal);
		}
		message += "The capillary glucose is ";
		float sugar = UnityEngine.Random.Range (4.5f, 9.0f);
		DispatchMeasurement(Insights.MeasuredCapillaryGlucose, sugar);
		message += sugar.ToString ("0");
		if (patient.conscious) {
			message += ".\n\nJohn cannot remember what regular medication he takes. ";
		} else {
			message += ".\n\nYou do not have a medication history available. ";
		}
		SendMessage (message, 0, 2, true);
		AddClinicalInformation("Disability");
	}

	public void AssessExposure () {
		string msg = "John's temperature is"; 
		msg += patient.temperature.ToString();
		msg += "*c. There is nothing much to see otherwise. ";

		DispatchMeasurement(Insights.MeasuredTemperature, patient.temperature);

		if (patient.exposureFindings != null) {
			DispatchInsight(patient.exposureFindings);

			switch (patient.exposureFindings) {
				case Insights.ExposureRash:
					msg = "John feels peripherally warm, and a blanching, erythematous rash is appearing all over. ";
					break;
				case Insights.ExposurePeripherallyShutdown:
					msg = "John appears peripherally shut-down. ";
					break;
				case Insights.ExposureStainedUnderwear:
					msg = "John's underwear is heavily stained with a dark, tarry, offensive-smelling substance. ";
					break;
			}
		}

		SendMessage (msg, 0, 2, true);
		AddClinicalInformation("Exposure");
	}

	public void AtropineGiven() {
		if (!doubleClickBlocker) {
			string message = "";
			if (cardiacArrest) {
				message = "Atropine isn't a drug you need during cardiac arrest. ";
				drugFails++;
				DispatchFeedback(Feedback.Blunder);
			} else {
				if (patient.diagnosis == "Arrhythmia" && !patient.bradyCardia && rhythmNumber == 1) {
					message = "John has an unstable tachycardia. Giving atropine is probably not the smartest thing "
					+ "you'll ever do. ";
					drugFailsNCPR++;
					DispatchFeedback(Feedback.Blunder);
				} else if (atropineRunning) {
					message = "Atropine should be given every 3-5 minutes. Give your last dose a chance to work. ";
					DispatchFeedback(Feedback.Blunder);
				} else if (!drawerVenflon.activeSelf) {
					message = "You give a 500mcg bolus of atropine. ";
					/*if (patient.diagnosis == "Arrhythmia" && rhythmNumber == 1) {
						message += " After five minutes, the heart rate hasn't budged. ";
					} else {
						StartCoroutine (AtropineTimer ());
					}*/
					StartCoroutine (AtropineTimer ());
				} else {
					message = "The patient has no line for giving IV drugs! ";
					DispatchFeedback(Feedback.Blunder);
				}
			}
			SendMessage (message, 0, 2, true);
		}
	}

	public void AttachPads () {
		if (Clickable) {
			drawerDefibPads.SetActive (false);
			animationTesting.AttachPads (true);
		}
	}

	public void AverageTimeOffChest()
	{
		float total = (minsOffChest * 60f) + secondsOffChest;
		float ave = total / rhythmChecks;
		if (ave < 60f)
		{
			averageTimeOffChestString = ave.ToString("00.00");
		}
		else
		{
			averageTimeOffChestString = "> 1 min! ";
		}
		TimeOffChest();
	}

	public void Bloods () {
		if (Clickable) {
			string message = "You send routine bloods to the lab. ";
            AddClinicalInformation("Bloods");
			SendMessage (message, 0, 2, true);
			drawerVacutainers.SetActive (false);
		}
	}

	public void BPCuffOn () {
		if (Clickable) {
			if (drawerAline.activeSelf) {
				nibpButton.SetActive (true);
			}
			drawerBPCuff.SetActive (false);
			animationTesting.bpCuff.SetActive (true);
			control.aLineScript.bpCuffOn = true;
			StartCoroutine (control.aLineScript.BPChecker ());
		}
	}

	public void BVM () {
		if (Clickable) {
			if (patient.conscious || respRate > 7) {
				string message = "";
				if (patient.airwayObstructed) {
					message = "Your colleague tells you to sort out the airway before deciding whether to bag the patient. ";
				} else {
					message = "John is breathing for himself and doesn't tolerate the bag-valve mask. ";
				} 
				SendMessage (message, 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
			} else {
				if (patient.airwayObstructed && patient.airwayObstruction != "Tongue") {
					string message = "You ask someone to start bagging, but they tell you the airway seems to be full of gunk. ";
					SendMessage (message, 0, 2, true);
					DispatchFeedback(Feedback.Blunder);
				} else {
					patient.airwayObstructed = false;
					DeactivateCameras ();
					mainCam.enabled = true;
					drawerBVM.SetActive (false);
					airwayManoeuvresButton.SetActive (false);
					SetResps (12);
					ArrangeButtons ();
					if (animationTesting.nrbMask.activeSelf) {
						animationTesting.nrbMask.SetActive (false);
						drawerNRBMask.SetActive (true);
					}
					PlaySequence ("Bagging");
					if (patient.diagnosis == "Pneumothorax" || patient.diagnosis == "Asthma" || patient.diagnosis == "Anaphylaxis") {
						string message = "Your colleague starts bagging but tells you they feel a lot of resistance to air entry. ";
						SendMessage (message, 0, 3, false);
					}
				}
			}
		}
	}

	public void CameraToDefaultView()
	{
		SwitchCPRButtons();
		if (cardiacArrest)
		{
			resumeText.text = "Resume CPR";
		}
		defibFocus = false;
		DeactivateCameras();
		mainCam.enabled = true;
		if (timerWasActive && !shockFail) {
			cprTimer.SetActive(true);
		}
		mainCamera.transform.position = originalCamPos;
		mainCamera.transform.rotation = originalCamRot;
	}

	public void CameraToDefaultView(bool abortShock)
	{
		//Debug.Log("Returning to default view, defibFocus = " + defibFocus);
		if (!controller.active)
		{
			CameraToDefaultView();
		}
		else
		{
			if (abortShock)
			{
				SwitchCPRButtons();
				if (debugging) {
					Debug.Log ("Switching CPR buttons from CameraToDefaultView");
					Debug.Log ("defibView = " + defibView);
				}
				control.noShock = true;
				control.Shock();
				resumeText.text = "Resume CPR";
				mainCamera.transform.position = originalCamPos;
				mainCamera.transform.rotation = originalCamRot; mainCam.enabled = true;
				DeactivateCameras();
				mainCam.enabled = true;
				if (timerWasActive) {
					cprTimer.SetActive(true);
				}
			}
		}
	}

    public void CardiacArrestSetter() {
        if (debugging) {
            Debug.Log("Arrest caused by " + arrestCause);
        }
        if (!drawersAllClosed)
        {
            StartCoroutine(TryCardiacArrest());
        }
        else
        {
            string message = "";
            patient.conscious = false;
            offChest = true;
            NextRhythm();
            MAP = 0f;
            control.MAP = 0f;
            control.aLineScript.ClientChangeBP(MAP);
            if (animationTesting.goDoc2.activeSelf)
            {
                cardiacArrest = true;
                CPRButtons.SetActive(true);
                nonCPRButtons.SetActive(false);
                examinationButtons.SetActive(false);
                chestCompressionButton.SetActive(true);
                drawersOpeningArrestDebug = true;
                DeactivateCameras();
                mainCam.enabled = true;
                defibFocus = false;
                if (!drawerDefibPads.activeSelf)
                {
                    attachPadsButton.SetActive(false);
                }
                if (!drawerVenflon.activeSelf)
                {
                    iVaccessButton.SetActive(false);
                    if (!animationTesting.drip.activeSelf)
                    {
                        fluidsButton.SetActive(true);
                    }
                    drugsButton.SetActive(true);
                }
                ArrangeButtons();
                message = "Your colleague who is bagging the patient tells you they can no longer " +
                        "feel a pulse! ";
                checkRhythmText.text = "Rhythm check";
                SendMessage(message, 0, 3, true);
            }
            else
            {
                PlaySequence("Arrested");
                message = "John has stopped breathing! ";
				DispatchInsight(Insights.BreathingNone);
            }
            SendMessage(message, 0, 3, true);
        }
	}

	void CardiacArrestEnder () {
		cardiacArrest = false;
		if (CPRButtons.activeSelf) {
			CPRButtons.SetActive (false);
		}
		dontSuspendCanvas = true;
		chestCompressionButton.SetActive (false);
		baggingDuringCPRButton.SetActive (false);
		ArrangeButtons ();
		drawersOpeningArrestDebug = false;
		string message = "John has a pulse! ";
		cprTimer.SetActive (false);
		sats = patient.sats + patient.oxygenResponse;
		SetSats (sats);
		SetResps (12);
		SendMessage (message, 2, 2, true);
	}

    public void CardiacArrestCallFinished ()
    {
        mainCam.cullingMask = cameraMask;
        messageScreen.SetActive(true);
    }

    void CardiacArrestZeroMAP()
    {
        if (cardiacArrest)
        {
            MAPList = new List<float>();

            for (int x = 0; x < 5; x++)
            {
                MAPList.Add(0f);
            }
        }
        else
        {
            MAPList = patient.MAPList;
        }
    }

	void CanvasPosition()
	{
		if (canvasSlideLeft || canvasSlideLeftExaminationOnly || canvasSlideRight || canvasSlideRightExaminationOnly) {

			if (debugging) {
				Debug.Log ("Canvas Position function starting");
			}

			if (CPRButtons.activeSelf) {
				if (canvasSlideLeft) {
					float fraction = (Time.time - canvasLerpTimeStamp) / 0.2f;
					if (fraction < 1f) {
						Vector3 startPos = new Vector3 (posX, CPRButtons.transform.position.y, CPRButtons.transform.position.z);
						Vector3 endPos = new Vector3 (0f, CPRButtons.transform.position.y, CPRButtons.transform.position.z);
						CPRButtons.transform.position = Vector3.Lerp (startPos, endPos, fraction);
					} else {
						CPRButtons.transform.position = new Vector3 (0f, CPRButtons.transform.position.y,
							CPRButtons.transform.position.z);
						canvasSlideLeft = false;
					}
				} else if (canvasSlideRight) {
					float fraction = (Time.time - canvasLerpTimeStamp) / 0.2f;
					if (fraction < 1f) {
						Vector3 startPos = new Vector3 (posX, CPRButtons.transform.position.y, CPRButtons.transform.position.z);
						Vector3 endPos = new Vector3 (0f, CPRButtons.transform.position.y, CPRButtons.transform.position.z);
						CPRButtons.transform.position = Vector3.Lerp (endPos, startPos, fraction);
					} else {
						CPRButtons.transform.position = new Vector3 (posX, CPRButtons.transform.position.y,
							CPRButtons.transform.position.z);
						posX = CPRButtons.transform.position.x;
						canvasSlideRight = false;
					}
				}
			} else if (nonCPRButtons.activeSelf) {
				if (canvasSlideLeft) {
					float fraction = (Time.time - canvasLerpTimeStamp) / 0.2f;
					if (fraction < 1f) {
						Vector3 startPos = new Vector3 (posX, nonCPRButtons.transform.position.y, nonCPRButtons.transform.position.z);
						Vector3 endPos = new Vector3 (0f, nonCPRButtons.transform.position.y, nonCPRButtons.transform.position.z);
						nonCPRButtons.transform.position = Vector3.Lerp (startPos, endPos, fraction);
						if (examinationButtons.transform.position != examinationButtonsOffScreen) {
							examinationButtons.transform.position = Vector3.Lerp (examinationButtonsOnScreen, examinationButtonsOffScreen, fraction);
						}
					} else {
						nonCPRButtons.transform.position = new Vector3 (0f, nonCPRButtons.transform.position.y,
							nonCPRButtons.transform.position.z);
						examinationButtons.transform.position = examinationButtonsOffScreen;
						canvasSlideLeft = false;
					}
				} else if (canvasSlideRight) {
					float fraction = (Time.time - canvasLerpTimeStamp) / 0.2f;
					if (fraction < 1f) {
						Vector3 startPos = new Vector3 (posX, nonCPRButtons.transform.position.y, nonCPRButtons.transform.position.z);
						Vector3 endPos = new Vector3 (0f, nonCPRButtons.transform.position.y, nonCPRButtons.transform.position.z);
						nonCPRButtons.transform.position = Vector3.Lerp (endPos, startPos, fraction);
						if (mainCam.enabled) {
							examinationButtons.transform.position = Vector3.Lerp (examinationButtonsOffScreen, examinationButtonsOnScreen, fraction);
						}
					} else {
						nonCPRButtons.transform.position = new Vector3 (posX, nonCPRButtons.transform.position.y,
							nonCPRButtons.transform.position.z);
						posX = nonCPRButtons.transform.position.x;
						if (mainCam.enabled) {
							examinationButtons.transform.position = examinationButtonsOnScreen;
						}
						canvasSlideRight = false;
					}
				} else if (canvasSlideLeftExaminationOnly) {
					float fraction = (Time.time - canvasLerpTimeStamp) / 0.2f;
					if (fraction < 1f) {
						examinationButtons.transform.position = Vector3.Lerp (examinationButtonsOnScreen, examinationButtonsOffScreen, fraction);
					} else {
						examinationButtons.transform.position = examinationButtonsOffScreen;
						canvasSlideLeftExaminationOnly = false;
					}
				} else if (canvasSlideRightExaminationOnly && nonCPRButtons.transform.position.x != 0f) {
					float fraction = (Time.time - canvasLerpTimeStamp) / 0.2f;
					if (fraction < 1f) {
						examinationButtons.transform.position = Vector3.Lerp (examinationButtonsOffScreen, examinationButtonsOnScreen, fraction);
					} else {
						if (debugging) {
							Debug.Log ("Moving examination buttons only on screen");
						}
						examinationButtons.transform.position = examinationButtonsOnScreen;
						canvasSlideRightExaminationOnly = false;
					}
				}
			}
		}
	}

	public void CanvasSlider()
	{
		if (debugging) {
			Debug.Log ("Canvas slider");
		}

		//HAD TO ADD THESE TWO LINES TO FIX A BUG:
		canvasSlideLeftExaminationOnly = false;
		canvasSlideRightExaminationOnly = false;

		canvasLerpTimeStamp = Time.time;

		if (messageNumber == 0) {
			if (scene == "DemoCPR") {
				if (CPRButtons.activeSelf) {
					/*if (CPRButtons.transform.position.x == 0f) {
					canvasSlideRight = true;
				}
				if (CPRButtons.transform.position.x == posX) {
					canvasSlideLeft = true;
				}*/
					CPRButtons.SetActive (false);
				} else {
					CPRButtons.SetActive (true);
				}
			} else if ((scene == "Conscious" || scene == "Unconscious") && !cardiacArrest) { 
				if (nonCPRButtons.activeSelf) {
					/*if (nonCPRButtons.transform.position.x == 0f) {
				canvasSlideRight = true;
				}
				if (nonCPRButtons.transform.position.x == posX) {
					canvasSlideLeft = true;
				}*/
					nonCPRButtons.SetActive (false);
					examinationButtons.SetActive (false);
				} else {
					nonCPRButtons.SetActive (true);
					if (mainCam.enabled) {
						examinationButtons.SetActive (true);
					}
				}
			}
		}
	}

	public void CanvasSliderExaminationOnly() {
		if (!cardiacArrest) {
			if (debugging) {
				Debug.Log ("Canvas Slider Ex Only");
			}
			canvasLerpTimeStamp = Time.time;
			/*
		if (examinationButtons.transform.position.x == examinationButtonsOffScreen.x && mainCam.enabled) {
				canvasSlideRightExaminationOnly = true;
		}
		if (examinationButtons.transform.position.x == examinationButtonsOnScreen.x) {
			canvasSlideLeftExaminationOnly = true;
		}*/
			if (examinationButtons.activeSelf) {
				examinationButtons.SetActive (false);
			} else {
				examinationButtons.SetActive (true);
			}
		}
	}

	public void ChangeFailText(string text)
	{
		failText.text = text;
	}

	public bool CheckFailScreen(bool trueFalse)
	{
		if (failScreen.activeSelf)
		{
			ActivateDeactivateFailScreen(false);
			return true;
		}
		else
		{
			return trueFalse;
		}
  
	}

	IEnumerator CPRButtonDelaySwitch()
	{
		if (debugging) {
			Debug.Log ("CPR delay switch");
		}
		yield return new WaitForSeconds(0.2f);
		RhythmTextButtonTextChange();
		HsTsHintToggle();
		if (defibView) {
			if (debugging) {
				Debug.Log ("Defib focussing");
			}
			resumeButton.SetActive (false);
			nonResumeButtons.SetActive (true);
			defibView = false;
		} else {
			nonResumeButtons.SetActive (false);
			resumeButton.SetActive (true);
			defibView = true;
		}
		ArrangeButtons();
	}

	public void CPRquality (int yValue) {
		// This function is triggered from AnimationTesting Update() cycle
		// Will send value of 2 when at full recoil,
		// 1 every time compression passes midpoint (going up or down),
		// 0 when at full compression,
		// 3 when 3/4 of way down (going down),
		// 4 when 3/4 of way up (going up)

		// prevCPRheight starts at default of 1
		if (cprQualityTimer == 1f && compressionsCompleted == 0) {
			cprQualityTimer = Time.time;
		}
		if (yValue == 2 && cprMidwayPassed) {
			prevCPRheight = 2;
			cprMidwayPassed = false;
		} else if (yValue == 0 && cprMidwayPassed) {
			prevCPRheight = 0;
			cprMidwayPassed = false;
		} else if (yValue == 1) {
			//Debug.Log ("Halfway");
			if (cprThirdwayPassed) {
				compressionsCompleted++;
				Debug.Log ("Compressions: " + compressionsCompleted);
				if (prevCPRheight == 4) {
					//Function for ensure full recoil
					//animationTesting.debugText.text = "Make sure you recoil fully! ";
				} else if (prevCPRheight == 3) {
					//Function for push harder
					//animationTesting.debugText.text = "Push harder! ";
				}
			}
			cprMidwayPassed = true;
			cprThirdwayPassed = false;
		} 

		if (compressionsCompleted == 6) {
			compressionsCompleted = 0;
			if (((Time.time - cprQualityTimer) / 6f) < 0.5f) {
				//Function for slower
				//animationTesting.debugText.text += "Slow down!" + ((Time.time - cprQualityTimer) / 6f);
			} else if (((Time.time - cprQualityTimer) / 6f) > 0.6f) {
				//Function for faster
				//animationTesting.debugText.text += "Speed up!" + ((Time.time - cprQualityTimer) / 6f);
			}
			cprQualityTimer = Time.time;
		}


	}

	void CPRTimerUpdate() {
		if (cprTimer.activeSelf) {
			cprTimeSecs += Time.deltaTime;
			if (cprTimeSecs >= 60f) {
				cprTimeSecs -= 60f;
				cprTimeMins += 1;
			}
			cprTimerText.text = "CPR timer:\n" + cprTimeMins + ":" + cprTimeSecs.ToString ("00.00");
			DispatchMemo(cprTimerText.text);
		}
	}

	public void DeactivateCameras()
	{
		//This function called by all Action buttons, so easy way to block double-clicks:
		StartCoroutine (DeactiveClicksHalfASecond ());

		if (headTiltChinLiftButton.activeSelf) {
			headTiltChinLiftButton.SetActive (false);
			jawThrustButton.SetActive (false);
		}
		//Sort out any open drawers:
		if (airwayDrawerOpen)
		{
			drawerStart = Time.time;
			airwayDrawerClosing = true;
		}
		if (airwayText.GetComponent<Text>().text == "Close drawer")
		{
			airwayText.GetComponent<Text>().text = "Airway Drawer";
		}
		if (breathingDrawerOpen)
		{
			drawerStart = Time.time;
			breathingDrawerClosing = true;
		}
		if (breathingText.GetComponent<Text>().text == "Close drawer")
		{
			breathingText.GetComponent<Text>().text = "Breathing Drawer";
		}
		if (circulationDrawerOpen)
		{
			drawerStart = Time.time;
			circulationDrawerClosing = true;
		}
		if (circulationText.GetComponent<Text>().text == "Close drawer")
		{
			circulationText.GetComponent<Text>().text = "Circulation Drawer";
		}
		if (drugDrawerOpen)
		{
			drawerStart = Time.time;
			drugDrawerClosing = true;
		}
		if (cardiacArrest) {
			if (drugText.GetComponent<Text> ().text == "Close drawer") {
				drugText.GetComponent<Text> ().text = "Drugs";
			}
		} else {
			if (drugTextNCPR.GetComponent<Text> ().text == "Close drawer") {
				drugTextNCPR.GetComponent<Text> ().text = "Drug Drawer";
			}
		}
		if (useDefibButton.GetComponentInChildren<Text> ().text == "Back") {
			useDefibButton.GetComponentInChildren<Text> ().text = "Defibrillator";
		}

		//Just faffing for practice at not having to hook up so many objects to script in editor
		if (cardiacArrest) {
			monitorTexts = monitorButton.GetComponentsInChildren<Text> ();
		} else {
			monitorTexts = monitorButtonNCPR.GetComponentsInChildren<Text> ();
		}
		foreach (Text monitorText in monitorTexts) {
			if (monitorText.text == "Back") {
				monitorText.text = "View monitor";
			}
		}
		if (viewingMonitor) {
			viewingMonitor = false;
		}
		mainCam.enabled = false;
		defibCam.enabled = false;
		defibFocus = false;
		drawerCam.enabled = false;
		monitorCam.enabled = false;
		cprCam.enabled = false;
        signsOfLifeCam.enabled = false;
		if (bvmDeactivatedTemp) {
			drawerBVM.SetActive (true);
			bvmDeactivatedTemp = false;
		}
	}

	public void StartGame() {
		if (firstRun) {
			firstRun = false;
			if (scene == "DemoCPR") {
				//CPRButtons.SetActive (true);
				canvasBVM.SetActive(true);
				if (SystemInfo.supportsAccelerometer)
				{
					buttonPushCPR.SetActive(true);
				}
			} else if (scene == "Conscious" || scene == "Unconscious" || scene == "Training") {
				nonCPRButtons.SetActive (true);
				examinationButtons.SetActive (true);
			}
			ArrangeButtons ();
		} else {
			if (scene == "DemoCPR" && !CPRButtons.activeSelf) {
				CanvasSlider ();
			}
			if ((scene == "Conscious" || scene == "Unconscious" || scene == "Training") && !nonCPRButtons.activeSelf) {
				CanvasSlider ();
			}
		}
		dontSuspendCanvas = false;
		if (debugging) {
			Debug.Log ("Last message in series");
		}
		messageNumber = 0;
		messageList = new List<string> ();
		doubleClickBlocker = false;
		StartCoroutine (DeactiveClicksHalfASecond ());
		/*if ((CPRButtons.activeSelf && CPRButtons.transform.position.x == 0f) ||
		(nonCPRButtons.activeSelf && nonCPRButtons.transform.position.x == 0f)) {
		SuspendCanvas ();
	}*/
		
		if (Time.timeScale == 0f) {
			Unpause();
			menuIcon.SetActive (true);
			DisplayAIMenu();
			if (debugging) {
				Debug.Log ("Time started!");
			}
		}
	}

	public void OnMessageRead() {
		
		//This starts the offChest timer as soon as the player has clicked the OK button:
		if (messageText.text == "John has stopped breathing!") {
			offChest = true;
			if (debugging) {
				Debug.Log ("Off chest");
			}
		}

		//Starts the game when user has read lead-in
		if (messageNumber >= messageList.Count) {
			StartGame();
		} else if (messageNumber < messageList.Count) {
			if (debugging) {
				Debug.Log ("More messages to send");
			}
			//dontSuspendCanvas = true;
			SendMessage (messageList [messageNumber], 0, 2, true);
			messageNumber++;
		}
	}

	public void DeactiveClicksHalfASecondTrigger () {
		StartCoroutine (DeactiveClicksHalfASecond ());
	}

	IEnumerator DeactiveClicksHalfASecond () {
		Clickable = false;
		//Is actually 0.7s to allow for drawers to open/close and examination menu to slide:
		yield return new WaitForSeconds (0.7f); 
		Clickable = true;
	}

	void DeactivateDefibButtons () {
		for (int i = 0; i < defibButtons.Count; i++) {
			if (debugging) {
				Debug.Log ("Deactivating defib button " + i);
			}
			defibButtons [i].SetActive (false);
		}
		airwayDrawer.SetActive (false);
	}

	void DemoEnd()
	{
		resumeGameButton.SetActive (false);
		CPRButtons.transform.position = new Vector3 (0f, CPRButtons.transform.position.y,
			CPRButtons.transform.position.z);
		Pause();
		bool didWell = true;
		demoEndText.text += "Well done, John regained output!\n\n";
		demoEndText.text += "You were off the chest for an average of ";
		float off = secondsOffChest / (rhythmChecks - 1f);
		demoEndText.text += off.ToString ("0.00");
		demoEndText.text += " seconds per rhythm check. ";
		if (off > 5f) {
			demoEndText.text += " (You should be aiming for less than 5 seconds.)";
			didWell = false;
		} else {
			demoEndText.text += " Excellent!\n\n";
		}
			
		if (failedCycles > 0)
		{
			if (failedCycles == 1)
			{
				demoEndText.text += " You had to retry one rhythm check. ";
				didWell = false;
			}
			else
			{
				demoEndText.text += " You had to retry " + failedCycles + "  rhythm checks. ";
				didWell = false;
			}
		}
		if (amiodaroneDueCycle < numberOfCorrectCycles && amiodaroneDueCycle != 0)
		{
			demoEndText.text += " You forgot to give amiodarone. ";
			didWell = false;
		}
		if (adrenalineDueCycle < numberOfCorrectCycles)
		{
			demoEndText.text += " You didn't give all the doses of adrenaline. ";
			didWell = false;
		}
		if (drugFails > 0)
		{
			demoEndText.text += " You were a little off with the drugs. ";
			didWell = false;
		}
		if (!fluidsGiven)
		{
			demoEndText.text += " You forgot to give fluids. ";
			didWell = false;
		}
		if (!didWell)
		{
			demoEndText.text += " But hey, practice makes perfect!\n\n";
		}
		else
		{
			demoEndText.text += "A perfectly run arrest, good work!\n\n";
		}
		demoEndText.text += "(Use the menu icon at the top of the screen to return to the Main Menu.)";
		demoEndScreen.SetActive(true);
	}

	public void DemoEnd (string failer) {
		resumeGameButton.SetActive (false);
		CPRButtons.transform.position = new Vector3 (0f, CPRButtons.transform.position.y,
			CPRButtons.transform.position.z);
		Pause();
		demoEndText.text = failer;
		demoEndScreen.SetActive(true);
	}

	public void Done () {
		bool didWell = true;

        if (scene == "Training")
        {
            Vector3 endPos = new Vector3(0f, nonCPRButtons.transform.position.y, nonCPRButtons.transform.position.z);
            nonCPRButtons.transform.position = endPos;
            examinationButtons.transform.position = examinationButtonsOffScreen;
            Pause();
            demoEndText.text = "";
            
            if (patient.training_scenario == 1)
            {
                if (clinical_information_obtained.ContainsValue("Circulation_examination")
                    && clinical_information_obtained.ContainsValue("ECG")
                    && clinical_information_obtained.ContainsValue("BP")
                    && clinical_information_obtained.ContainsValue("Bloods"))
                {
                    demoEndText.text += "Well done! You performed an examination of the cardiovascular system, checked the blood pressure, verified the cardiac rhythm and sent blood tests.\n\n" +
                        "Those are the key components of the \"C\" stage of the ABCDE assessment. ";
                    //Code to alter user's profile to reflect that scenario has been completed here.
					didWell = false;
                } else
                {
					didWell = true;
                    demoEndText.text += "Oops! You forgot to:\n\n";
                    if (!clinical_information_obtained.ContainsValue("Circulation_examination"))
                    {
                        demoEndText.text += "- examine the circulatory system\n";
                    }
                    if (!clinical_information_obtained.ContainsValue("ECG"))
                    {
                        demoEndText.text += "- obtain cardiac monitoring";
                        if (!drawerCmPads.activeSelf)
                        {
                            demoEndText.text += " (you did attach a cardiac monitor but you need to view the screen to obtain the reading)";
                        } else if (!drawerDefibPads.activeSelf)
                        {
                            demoEndText.text += " (you did attach defib pads but you need to switch on the defibrillator and view the screen to obtain the reading)";
                        }
                        demoEndText.text += "\n";
                    }
                    if (!clinical_information_obtained.ContainsValue("BP"))
                    {
                        demoEndText.text += "- check the BP";
                        if (!drawerBPCuff.activeSelf)
                        {
                            demoEndText.text += " (you did attach the cuff but you need to view the monitor to obtain the reading)";
                        } else if (!drawerAline.activeSelf)
                        {
                            demoEndText.text += " (you did insert an arterial line but you need to view the monitor to obtain the reading)";
                        }
                        demoEndText.text += "\n";
                    }
                    if (!clinical_information_obtained.ContainsValue("Bloods"))
                    {
                        demoEndText.text += "- take bloods\n";
                    }
                    demoEndText.text += "\nGive it another try! (Click the menu icon at the top of the screen and hit \"Main menu\"";
                }
            }
            demoEndScreen.SetActive(true);
        } else { 
            Vector3 endPos = new Vector3(0f, nonCPRButtons.transform.position.y, nonCPRButtons.transform.position.z);
            nonCPRButtons.transform.position = endPos;
            examinationButtons.transform.position = examinationButtonsOffScreen;
            Pause();
            demoEndText.text += "Thanks for playing! Here's how you did:\n\n";
            if (MAP >= 60f && sats >= 88 && !patient.airwayObstructed && respRate > 7)
            {
                demoEndText.text += "You stabilised John, good work! ";
                if (drugFailsNCPR > 0)
                {
                    didWell = false;
                    demoEndText.text += " Although you made ";
                    if (drugFailsNCPR == 1)
                    {
                        demoEndText.text += "an error";
                    }
                    else
                    {
                        demoEndText.text += "a couple of errors";
                    }
                    demoEndText.text += "with the drugs - perhaps an area for some work. ";
                    if (failedShocks > 0)
                    {
                        demoEndText.text += " It might also be worth a bit of practice with the defibrillator, as you " +
                        "didn't seem entirely comfortable with this. ";
                    }
                }
                else if (failedShocks > 0)
                {
                    didWell = false;
                    demoEndText.text += " Although you weren't quite perfect with the defibrillator - maybe something to practise. ";
                }
                if (control.capture && !control.safeCapture)
                {
                    didWell = false;
                    demoEndText.text += "\n\nYou needed to turn the pacing current up 10mA above the capture to allow for " +
                        "increased impedance over time. ";
                }
                if (rhythmChecks == 0 && didWell)
                {
                    demoEndText.text += " We didn't identify any particular areas for improvement on this occasion. ";
                }
            }
            else
            {
                didWell = false;
                demoEndText.text += "Sadly, you didn't manage to stabilise John on this occasion. ";
                if (patient.airwayObstructed)
                {
                    demoEndText.text += " The airway is obstructed. Next time, assess the airway immediately " +
                    " and if you find any issues, try suctioning, simple manoeuvres and adjuncts to clear it. ";
                }
                if (respRate < 8)
                {
                    demoEndText.text += " He is hypoventilating, which could be fixed by bag-valve-mask ventilation. ";
                }
                if (sats < 88 && drawerNRBMask.activeSelf && drawerBVM.activeSelf)
                {
                    demoEndText.text += " He is hypoxic and you didn't give oxygen. ";
                }
                if (MAP < 60f && drawerFluids.activeSelf && !patient.pulmonaryOedema)
                {
                    demoEndText.text += " He is hypotensive and there are no fluids running. ";
                }
                else if (MAP < 60f)
                {
                    demoEndText.text += " He is hypotensive. ";
                }
                if (patient.diagnosis == "Arrhythmia" && !patient.bradyCardia && rhythmNumber == 1)
                {
                    demoEndText.text += " He has an unstable tachyarrhythmia, which is an indication for cardioversion. ";
                }
                else if (patient.diagnosis == "Arrhythmia" && patient.bradyCardia)
                {
                    if (control.capture && !control.safeCapture)
                    {
                        demoEndText.text += " You needed to turn the pacing current up 10mA above the capture to allow for " +
                            "increased impedance over time. ";
                    }
                    else if (control.pacing && !control.capture)
                    {
                        demoEndText.text += " You needed to turn the pacing current up to get capture. ";
                    }
                    else if (!control.pacing)
                    {
                        demoEndText.text += " He has an unstable bradycardia, which is an indication for pacing. ";
                    }
                }
                if (drugFailsNCPR > 0)
                {
                    demoEndText.text += " Additionally, you made ";
                    if (drugFailsNCPR == 1)
                    {
                        demoEndText.text += "an error";
                    }
                    else
                    {
                        demoEndText.text += "a couple of errors";
                    }
                    demoEndText.text += " with the drugs - perhaps an area for some work. ";
                    if (failedShocks > 0)
                    {
                        demoEndText.text += " It might also be worth a bit of practice with the defibrillator, as you " +
                            "didn't seem entirely comfortable with this. ";
                    }
                }
                else if (failedShocks > 0)
                {
                    demoEndText.text += " Also, you weren't quite perfect with the defibrillator - maybe something to practise. ";
                }
            }
            if (rhythmChecks > 0)
            {
                didWell = false;
                demoEndText.text += "\n\nUnfortunately, John arrested due to " + arrestCause + ". " +
                "During the arrest, you were off the chest for an average of ";
                float off = secondsOffChest / (rhythmChecks - 1f);
                demoEndText.text += off.ToString("0.00");
                demoEndText.text += " seconds per rhythm check";
                if (off > 5f)
                {
                    demoEndText.text += " (you should be aiming for less than 5 seconds),";
                }
                else
                {
                    demoEndText.text += ",";
                }
                if (failedCycles > 0)
                {
                    if (failedCycles == 1)
                    {
                        demoEndText.text += " you had to retry one rhythm check,";
                        didWell = false;
                    }
                    else
                    {
                        demoEndText.text += " you had to retry " + failedCycles + "  rhythm checks,";
                        didWell = false;
                    }
                }
                if (amiodaroneDueCycle < numberOfCorrectCycles && amiodaroneDueCycle != 0)
                {
                    demoEndText.text += " you forgot to give amiodarone,";
                    didWell = false;
                }
                if (adrenalineDueCycle < numberOfCorrectCycles)
                {
                    demoEndText.text += " you didn't give all the doses of adrenaline,";
                    didWell = false;
                }
                if (drugFails > 0)
                {
                    demoEndText.text += " you were a little off on the timings of the drugs,";
                    didWell = false;
                }
                if (!fluidsGiven)
                {
                    demoEndText.text += " you forgot to give fluids,";
                    didWell = false;
                }
                if (!didWell)
                {
                    demoEndText.text += " and so it may be worth a bit more practice. ";
                }
            }
            if (didWell)
            {
                demoEndText.text += " Excellent work! ";
            }
            else
            {
                demoEndText.text += "\n\nDon't worry if you're not perfect yet, though. That's exactly what we're here for! ";
            }
            demoEndScreen.SetActive(true);
        }

		DispatchMemo(demoEndText.text);
		if (didWell) {
			DispatchFeedback(Feedback.Success);
		} else {
			DispatchFeedback(Feedback.Failure);
		}
	}

	void DrawerUpdate()
	{
		if (airwayDrawerOpening) {
			if (DrawerUpdateHelper (airwayDrawer, true)) {
				airwayDrawerOpening = false;
				airwayDrawerOpen = true;
				SetDrawerCam ("airway");
			}
		} else if (airwayDrawerClosing) {
			if (DrawerUpdateHelper (airwayDrawer, false)) {
				airwayDrawerClosing = false;
				airwayDrawerOpen = false;
			}
		}
		if (breathingDrawerOpening)
		{
			if (DrawerUpdateHelper (breathingDrawer, true)) {
				breathingDrawerOpening = false;
				breathingDrawerOpen = true;
				SetDrawerCam ("breathing");
			}
		}
		else if (breathingDrawerClosing)
		{
			if (DrawerUpdateHelper (breathingDrawer, false)) {
				breathingDrawerClosing = false;
				breathingDrawerOpen = false;
			}
		}
		if (circulationDrawerOpening)
		{
			if (DrawerUpdateHelper (circulationDrawer, true)) {
				circulationDrawerOpening = false;
				circulationDrawerOpen = true;
				SetDrawerCam ("circulation");
			}
		}
		else if (circulationDrawerClosing)
		{
			if (DrawerUpdateHelper (circulationDrawer, false)) {
				circulationDrawerClosing = false;
				circulationDrawerOpen = false;
			}
		}
		if (drugDrawerOpening)
		{
			if (DrawerUpdateHelper (drugDrawer, true)) {
				drugDrawerOpening = false;
				drugDrawerOpen = true;
				SetDrawerCam ("drugs");
			}
		}
		else if (drugDrawerClosing)
		{
			if (DrawerUpdateHelper (drugDrawer, false)) {
				drugDrawerClosing = false;
				drugDrawerOpen = false;
			}
		}
		if (!airwayDrawerOpen && !airwayDrawerOpening &&
		    !breathingDrawerOpen && !breathingDrawerOpening &&
		    !circulationDrawerOpen && !circulationDrawerOpening &&
			!drugDrawerOpen && !drugDrawerOpening && !drawersAllClosed) {
			drawersAllClosed = true;
			Debug.Log ("Drawers all closed");
			if (mainCam.enabled) {
				CanvasSliderExaminationOnly ();
			}
		}
	}

	bool DrawerUpdateHelper (GameObject drawer, bool opening) {
		if (opening) {
			float y = drawer.transform.localPosition.y;
			float z = drawer.transform.localPosition.z;
			Vector3 openPos = new Vector3 (drugDrawerOpenPos.x, y, z);
			Vector3 closedPos = new Vector3 (drawerClosedPos.x, y, z);
			float x = Time.time - drawerStart;
			float frac = x / 0.5f;
			if (frac < 1f) {
				drawer.transform.localPosition = Vector3.Lerp (closedPos, openPos, frac);
				return false;
			} else {
				drawer.transform.localPosition = openPos;
				return true;
			}
		} else {
			float y = drawer.transform.localPosition.y;
			float z = drawer.transform.localPosition.z;
			Vector3 openPos = new Vector3 (drugDrawerOpenPos.x, y, z);
			Vector3 closedPos = new Vector3 (drawerClosedPos.x, y, z);
			float x = Time.time - drawerStart;
			float frac = x / 0.5f;
			if (frac < 1f)
			{
				drawer.transform.localPosition = Vector3.Lerp(openPos, closedPos, frac);
				return false;
			}
			else
			{
				drawer.transform.localPosition = closedPos;
				return true;
			}
		}
	}

	public void DumpCharge()
	{
		control.DumpCharge();
	}

	public void Fail()
	{
		control.Fail();
	}

	public void Fluids () {
		if (Clickable) {
			if (!drawerVenflon.activeSelf) {
				drawerFluids.SetActive (false);
				animationTesting.drip.SetActive (true);
				fluidsRunning = true;
				StartCoroutine (FluidRunningTimer ());
				StartCoroutine (FluidResponseTimer ());
				GiveFluids ();
			} else {
				SendMessage ("The patient doesn't have a cannula for fluids!", 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
			}
		}
	}

	public void FocusOnDefibFromButton () {
		if (Clickable) {
			FocusOnDefib ();
		}
	}

	public void FocusOnDefib()
	{
		if (defibFocus) {
			if (animationTesting.goDoc2.activeSelf && !animationTesting.doc2BendingForwards) {
				PlaySequence ("Bagging");
			}
			DeactivateCameras ();
			mainCam.enabled = true;
			if (timerWasActive && cardiacArrest) {
				cprTimer.SetActive(true);
			}
			if (!cardiacArrest && !examinationButtons.activeSelf) {
				CanvasSliderExaminationOnly ();
			}
		} else {
			DeactivateCameras ();
			defibCam.enabled = true;
			if (drawerBVM.activeSelf) {
				drawerBVM.SetActive (false);
				bvmDeactivatedTemp = true;
			}
			defibFocus = true;
			if (cprTimer.activeSelf) {
				cprTimer.SetActive(false);
				timerWasActive = true;
			}
			if (!control.heartRateLabel.activeSelf) {
				StartCoroutine (DefibHintTimer ());
			}
			if (!cardiacArrest && examinationButtons.activeSelf) {
				CanvasSliderExaminationOnly ();
			}
			if (animationTesting.goDoc1.activeSelf && animationTesting.defibPads.activeSelf && control.defibReady) {
				resumeText.text = "Resume CPR";
			} else {
				useDefibButton.GetComponentInChildren<Text> ().text = "Back";
				resumeText.text = "Back";
			}
			/*Vector3 defibCamPos = defibCamera.transform.position;
	mainCamera.transform.position = defibCamPos;
	mainCamera.transform.rotation = Quaternion.Euler (0f, 0f, 0f);*/
		}
	}

	public void Guedel () {
		if (Clickable) {
			if (patient.conscious) {
				string message = "Funnily enough, your fully conscious patient is not terribly impressed " +
				                "when you try to shove an oropharyngeal airway in his mouth. ";
				SendMessage (message, 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
			} else {
				string message = "You site an oropharyngeal airway successfully. ";
				SendMessage (message, 0, 2, true);
				if (patient.airwayObstruction == "Tongue") {
					patient.airwayObstructed = false;
				}
				drawerGuedel.SetActive (false);
				airwayManoeuvresButton.SetActive (false);
				if (headTiltChinLiftButton.activeSelf) {
					headTiltChinLiftButton.SetActive (false);
					jawThrustButton.SetActive (false);
				}
				ArrangeButtons ();
				animationTesting.guedel.SetActive (true);
			}
		}
	}

	public void GiveFluids()
	{
		fluidsGiven = true;
	}

	public void GoToMenu () {
		Pause();
		Vector3 mainCamMenuRot = new Vector3 (0f, 90f, 0f);
		menuIcon.SetActive(false);
		aiMenu.SetActive (false);
		//mainCam.transform.rotation = Quaternion.Euler (mainCamMenuRot);
		if (gameCanvas.activeSelf) {
			lastCanvas = "game";
			gameCanvas.SetActive (false);
		} else if (rhythmPracticeCanvas.activeSelf) {
			lastCanvas = "rhythm";
			rhythmPracticeCanvas.SetActive (false);
		} else if (abgCanvas.activeSelf) {
			lastCanvas = "abg";
			abgCanvas.SetActive (false);
		}
		menu.SetActive (true);
	}

	void HsTsHintToggle ()
	{
		if (numberOfCorrectCycles > 0 && !hsTsHintTextShown)
		{
			StartCoroutine (HsTsEnumerator ());
		}
	}

	IEnumerator HsTsEnumerator () {
		yield return new WaitForSeconds (5);
		if (nonResumeButtons.activeSelf) {
			SendMessage ("You don't have to wait for the full two minutes before the next rhythm check - " +
				"when you're happy you've done everything you need to do this cycle, press the \"Next rhythm check\" button " +
				"and the timer will skip ahead to keep up with you.\n\n" +
				"Before you do that, though, make sure you can list the reversible causes of cardiac arrest (4 Hs and 4 Ts) off by heart!", 0, 0, true);
			hsTsHintTextShown = true;
		}
	}

	public void LearningZone () {
		if (menuPlayButtons.activeSelf) {
			menuPlayButtons.SetActive (false);
			menuLearningZone.SetActive (true);
		} else {
			menuPlayButtons.SetActive (true);
			menuLearningZone.SetActive (false);
		}
	}

    public void LoadPatient()
    {
        if (debugging)
        {
            Debug.Log("Starting serializer");
        }
        serializer.LoadHub();
    }

    public void LoadPatient(Patient p)
    {
        if (debugging)
        {
            Debug.Log("Back to hub, loading patient");
        }
        patient = p;
        patient.LoadingFunctions();
        Debug.Log("Patient loaded " + patient.diagnosis);
    }

	public bool MidazolamGiven () {
		if (debugging) {
			Debug.Log ("Midaz");
		}
		if (Clickable) {
			string message = "";
			if (cardiacArrest) {
				message += "Midazolam isn't a drug you need during cardiac arrest. (At least, not for the patient...)";
				drugFails++;
				DispatchFeedback(Feedback.Blunder);
				SendMessage (message, 0, 2, true);
				return false;
			} else if (!patient.conscious) {
				message += "John is already unconscious - maybe giving midazolam isn't such a hot idea... ";
				drugFailsNCPR++;
				DispatchFeedback(Feedback.Blunder);
				SendMessage (message, 0, 2, true);
				return false;
			} else if (drawerVenflon.activeSelf) {
				message += "The patient has no line for giving IV drugs! ";
				SendMessage (message, 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
				return false;
			} else {
				message += "You give midazolam. ";

				if (respRate != 0) {
					respRate -= UnityEngine.Random.Range (0, 20);
					if (respRate <= 0) {
						SetResps(0);
						message += " Unfortunately, it has caused John to stop breathing. ";
						StartCoroutine (HypoventilationTimer ());
					} else {
						message += " John is now adequately sedated. ";
					}
					SetResps (respRate);
				}
				PlaySequence ("LieIdle");
				patient.conscious = false;
				SendMessage (message, 0, 2, true);
				return true;
			}
		} else {
			return false;
		}
	}

	public void MonitorPadsOn () {
		if (Clickable) {
			animationTesting.CardiacMonitorPadsOn ();
			drawerCmPads.SetActive (false);
			control.monitorPadsToComeOn = true;
			control.monitorHeartRateText.SetActive (true);
			control.monitorHeartRateText.GetComponent<TextMesh> ().text = "WAIT";
			RespOn ();
		}
	}

	public float NextHeartRate()
	{
		if (randomRhythm)
		{
			float randomRate = UnityEngine.Random.Range(10f, 200f);
			return randomRate;
		}
		else
		{
			return heartRate;
		}
	}

	public string NextRhythm()
	{
		//CODE FOR PATIENT HAVING ARRESTED IN A NON-ARREST SCENARIO:
		/* NB: When a patient arrests, the rhythm number is 2 (I can't find where this is actually set!), 
		 * and this increases by one each rhythm check.
		 * When rhythm number > 3, it reverts to 0 - i.e. the rhythm and MAP that the scenario started with.
		 * So set rhythm number and MAP at position 0 in the list as your default.
		 * If diagnosis == arrhythmia, then rhythm and MAP as position 1 are what you want following successful DCCV.
		 * Rhythms at positions 2 and 3 in the List should be arrest rhythms with MAP == 0. */

		if (cardiacArrest && !patient.arrest) {
			if (patient.diagnosis == "Arrhythmia") {
				//ARRHYTHMIA SCENE WORKS REVERTING TO NSR AFTER 1 SHOCK
				//SO IF USER HAS ALREADY DELIVERED THAT SHOCK, NEED TO GO BACK TO THIS AFTER ARREST:
				if (rhythmNumber == 2) {
					rhythmList [0] = rhythmList [1];
					MAPList [0] = MAPList [1];
					heartRateList [0] = heartRateList [1];
				} else if (rhythmNumber == 1) {
					rhythmNumber = 2;
				} 
			} else if (rhythmNumber == 0) {
				rhythmNumber = 1;
			}
			if (rhythmNumber > 3) {
				rhythmNumber = 0;
			}
			rhythm = rhythmList[rhythmNumber];
			heartRate = heartRateList[rhythmNumber];
			MAP = MAPList [rhythmNumber];
			rhythmNumber++;
			control.RemoteChangeECG (rhythm);

			if (debugging) {
				Debug.Log ("Rhythm number : " + (rhythmNumber - 1) + " Rhythm: " + rhythm +
				" MAP: " + MAP);
			}
		//CODE FOR ANY OTHER SCENE WITHOUT RANDOM RHYTHM:
		} else if (!randomRhythm) {
			if (rhythmNumber > 3) {
				rhythmNumber = 0;
			}
			if (debugging) {
				Debug.Log ("Rhythm number: " + rhythmNumber);
			}
			rhythm = rhythmList[rhythmNumber];
			heartRate = heartRateList[rhythmNumber];
			if (cardiacArrest) {
				MAP = 0f;
			} else {
				MAP = MAPList [rhythmNumber];
			}
			if (cardiacArrest) {
				if (numberOfCorrectCycles > 3) {
					MAP = 60f;
				} else {
					if (debugging) {
						Debug.Log ("Setting MAP to 0");
					}
					MAP = 0f;
				}
			}
			rhythmNumber++;
			control.RemoteChangeECG (rhythm);

		//CODE FOR RANDOM RHYTHM:
		} else {
			if (cardiacArrest) {
				rhythm = patient.RandomRhythm (true);
				if (debugging) {
					Debug.Log ("Setting random rhythm to " + rhythm);
				}
				control.RemoteChangeECG (rhythm);
			} else {
				rhythm = patient.RandomRhythm (false);
			}

			if (patient.scene == "CPR" || patient.scene == "DemoCPR")
			{
				if (numberOfCorrectCycles > 3)
				{
					MAP = 60f;
				}
				else
				{
					if (debugging) {
						Debug.Log ("Setting MAP to 0");
					}
					MAP = 0f;
				}
			}
			else
			{
				MAP = MAPList[rhythmNumber];
				if (MAP > 55f && MAP < 62f) {
					MAP = 55f;
				} else if (MAP >= 62f && MAP < 70f) {
					MAP = 70f;
				}
				if (!cardiacArrest && MAP == 0f) {
					MAP = 70f;
				}
			}
			rhythmNumber++;
			control.RemoteChangeECG (rhythm);
		}
		return rhythm;
	}

	public void NIBP () {
		if (!drawerBPCuff.activeSelf && drawerAline.activeSelf) {
			control.aLineScript.bpText.text = "WAIT";
			StartCoroutine (NIBPdelay ());
		}
	}

	IEnumerator NIBPdelay () {
		yield return new WaitForSeconds (3);
		string bp = control.aLineScript.bpSys + "\n" + control.aLineScript.bpDias;
		control.aLineScript.bpText.text = bp;
	}

	public void NRBMask () {
		if (Clickable) {
			if (drawerBVM.activeSelf) {
				if (respRate == 0) {
					string message = "You could put oxygen on the patient, but it might be more helpful if he was actually breathing... ";
					SendMessage (message, 0, 2, true);
				} else if (patient.airwayObstructed) {
					string message = "Putting oxygen on the patient is a good idea, " +
						"but one of your colleagues thinks you should sort out the airway first. ";
					SendMessage (message, 0, 2, true);
				} else {
					drawerNRBMask.SetActive (false);
					animationTesting.nrbMask.SetActive (true);
					int newsats = patient.sats + patient.oxygenResponse;
					StartCoroutine (SatsIncrease (newsats));
				}
			} else {
				string message = "John is being bagged right now, switching to an ordinary oxygen mask " +
				                 "probably isn't a great idea. ";
				SendMessage (message, 0, 2, true);
			}
		}
	}

	public void OpenCloseDrawerFromButton (string button) {
		if (debugging) {
			Debug.Log ("OpenClose");
		}
		if (Clickable) {
			Clickable = false;
			StartCoroutine (DeactiveClicksHalfASecond ());
			if (button == "Airway") {
				OpenCloseAirwayDrawer ();
			} else if (button == "Breathing") {
				OpenCloseBreathingDrawer ();
			} else if (button == "Circulation") {
				OpenCloseCirculationDrawer ();
			} else if (button == "Drugs") {
				OpenCloseDrugDrawer ();
			}
			if (drawersAllClosed && examinationButtons.activeSelf) {
				CanvasSliderExaminationOnly ();
			}
		}
	}

	void OpenCloseAirwayDrawer()
	{
		if (breathingDrawerOpen) {
			OpenCloseBreathingDrawer ();
		}
		if (circulationDrawerOpen) {
			OpenCloseCirculationDrawer ();
		}
		if (drugDrawerOpen) {
			OpenCloseDrugDrawer ();
		}
		if (!airwayDrawerOpen)
		{
			DeactivateCameras ();
			mainCam.enabled = true;
			airwayText.GetComponent<Text>().text = "Close drawer";
			drawerStart = Time.time;
			airwayDrawerOpening = true;

		} else
		{
			DeactivateCameras(); //this function also closes the drawer
			mainCam.enabled = true;
		}
	}

	void OpenCloseBreathingDrawer()
	{
		if (airwayDrawerOpen) {
			OpenCloseAirwayDrawer ();
		}
		if (circulationDrawerOpen) {
			OpenCloseCirculationDrawer ();
		}
		if (drugDrawerOpen) {
			OpenCloseDrugDrawer ();
		}
		if (!breathingDrawerOpen)
		{
			DeactivateCameras();
			mainCam.enabled = true;
			breathingText.GetComponent<Text>().text = "Close drawer";
			drawerStart = Time.time;
			breathingDrawerOpening = true;

		}
		else
		{
			DeactivateCameras(); //this function also closes the drawer
			mainCam.enabled = true;
		}
	}

	void OpenCloseCirculationDrawer()
	{
		if (airwayDrawerOpen) {
			OpenCloseAirwayDrawer ();
		}
		if (breathingDrawerOpen) {
			OpenCloseBreathingDrawer ();
		}
		if (drugDrawerOpen) {
			OpenCloseDrugDrawer ();
		}
		if (!circulationDrawerOpen)
		{
			DeactivateCameras();
			mainCam.enabled = true;
			circulationText.GetComponent<Text>().text = "Close drawer";
			drawerStart = Time.time;
			circulationDrawerOpening = true;

		}
		else
		{
			DeactivateCameras(); //this function also closes the drawer
			mainCam.enabled = true;
		}
	}

	void OpenCloseDrugDrawer()
	{
		drawersOpeningArrestDebug = false;
		if (airwayDrawerOpen) {
			OpenCloseAirwayDrawer ();
		}
		if (breathingDrawerOpen) {
			OpenCloseBreathingDrawer ();
		}
		if (circulationDrawerOpen) {
			OpenCloseCirculationDrawer ();
		}
		if (!drugDrawerOpen)
		{
			DeactivateCameras();
			mainCam.enabled = true;
			if (cardiacArrest) {
				drugText.GetComponent<Text> ().text = "Close drawer";
			} else {
				drugTextNCPR.GetComponent<Text> ().text = "Close drawer";
			}
			drawerStart = Time.time;
			drugDrawerOpening = true;
		}
		else
		{
			DeactivateCameras(); //this function also closes the drawer
			mainCam.enabled = true;
		}
	}

	public void PlaySequence(string animation)
	{
		animationTesting.PlaySequence(animation);
	}

	public void PlaySound(string sound)
	{
		if (soundOn) {
			if (sound == "Beep") {
				if (defibCam.isActiveAndEnabled) {
					soundPlayer.GetComponent<SoundPlayer> ().PlaySound (sound);
				}
			} else {
				soundPlayer.GetComponent<SoundPlayer> ().PlaySound (sound);
			}
		}
	}

	//Actually this is for all messages, not just "PulseMessages"
	public IEnumerator PulseMessageEnumerator(string message, int delay, int timer, bool button)
	{
		if (debugging) {
			Debug.Log ("PulseMessageEnumerator starting, dsp =" + dontSuspendCanvas);
		}
		
		if (headTiltChinLiftButton.activeSelf) {
			headTiltChinLiftButton.SetActive (false);
			jawThrustButton.SetActive (false);
		}

		yield return new WaitForSeconds(delay);
		if (!button)
		{
			subtitles.text += message;
			yield return new WaitForSeconds(timer);

			subtitles.text = subtitles.text.Substring(message.Length);

			OnMessageRead();
		}
		else
		{
			Clickable = false;
			if (!dontSuspendCanvas) {
				SuspendCanvas ();
			} else {
				useDefibButton.GetComponentInChildren<Text> ().text = "Back";
				animationTesting.goDoc1.SetActive (false);
				offChest = false;
			}
			dontSuspendCanvas = false;
			doubleClickBlocker = true;
			messageText.text = message;
			//messageBackground.GetComponent<RectTransform>().sizeDelta = messageText.g
			messageScreen.active = true;

			//OnMessageRead() will be activated by the "OK" button on the message
		}
	}

	public void Quit()
	{
		Application.Quit ();
	}

	public void Reload() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  	}

	void Resizer () {

		float ratio = Screen.width / 1136f;
		float heightRatio = Screen.height / 640f;
		menuIcon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f * ratio, 100f * ratio);
		shockIcon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (50f * ratio, 100f * ratio);
		notIcon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (75f * ratio, 75f * ratio);
		messageBackground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (800f * ratio, 300f * ratio);
		shockOrNotMessageHolder.GetComponent<RectTransform> ().sizeDelta = new Vector2 (400f * ratio, 200f * ratio);
		shockOrNotOKButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f * ratio, 60f * ratio);
		rhythmPracticeMessageHolder.GetComponent<RectTransform> ().sizeDelta = new Vector2 (400f * ratio, 200f * ratio);
		pauseQuit.GetComponent<RectTransform> ().sizeDelta = new Vector2 (140f * ratio, 70f * ratio);
		pauseReload.GetComponent<RectTransform> ().sizeDelta = new Vector2 (140f * ratio, 70f * ratio);
		pauseResume.GetComponent<RectTransform> ().sizeDelta = new Vector2 (140f * ratio, 70f * ratio);

		Vector3 newby = new Vector3 (Screen.width/2f,
			580f * heightRatio, menuIcon.transform.position.z);
		menuIcon.transform.position = newby;

		newby = new Vector3 (Screen.width/2f - (250f*ratio),
			Screen.height - (45f * heightRatio), menuIcon.transform.position.z);
		shockIcon.transform.position = newby;
		newby = new Vector3 (Screen.width/2f + (250f*ratio),
			600f * heightRatio, menuIcon.transform.position.z);
		notIcon.transform.position = newby;
		newby = new Vector3 (568f * ratio, 150f * heightRatio, 0f);
		messageButton.transform.position = newby;
		newby = new Vector3 (568f * ratio, 120f * heightRatio, 0f);
		shockOrNotOKButton.transform.position = newby;
		rhythmPracticeOKButton.transform.position = newby;
		newby = new Vector3 (568f * ratio, 224f * heightRatio, 0f);
		pauseQuit.transform.position = newby;
		newby = new Vector3 (568f * ratio, 324f * heightRatio, 0f);
		pauseReload.transform.position = newby;
		newby = new Vector3 (568f * ratio, 424f * heightRatio, 0f);
		pauseResume.transform.position = newby;

		if (largeScreen) {
			foreach (GameObject b in buttonList) {
				b.GetComponentInChildren<Text> ().resizeTextMaxSize = 32;
			}
			foreach (GameObject b in examinationButtonList) {
				
				b.GetComponentInChildren<Text> ().resizeTextMaxSize = 32;
			}
			actionMenuText.resizeTextMaxSize = 36;
			examinationMenuText.resizeTextMaxSize = 36;
			cprMenuText.resizeTextMaxSize = 36;
			menu = GameObject.Find ("LargeScreenMenu");
			menuPlayButtons = GameObject.Find ("LSPlayButtons");
			menuPauseButtons = menu.transform.Find ("LSPauseButtons").gameObject;
			menuLearningZone = menu.transform.Find ("LSLearningZone").gameObject;
			messageScreenOkButton.GetComponent<RectTransform> ().localScale = Vector3.one * 2f;
			canvasBVM.GetComponent<RectTransform> ().sizeDelta = new Vector2 (16f, 40f);
			canvasBVM.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (300f, -160f, 0f);
		}
	}

	public void RespOn () {
		control.respScriptToComeOn = true;
		control.respScript.respText.text = "WAIT";
	}

	public void ResumeCanvas()
	{
		CanvasSlider();
	}

	public void ResumeCPR()
	{
		StopAllSounds();
		controller.GetComponent<Control>().DumpCharge();
		animationTesting.PlaySequence("Back");
	}

	public void ReturnFromMenu () {
		if (menuPlayButtons.activeSelf) {
			if (debugging) {
				Debug.Log ("Menu Play Buttons active");
			}
			menuPlayButtons.SetActive (false);
			menuPauseButtons.SetActive (true);
		} else {
			if (scene != "abgPractice") {
				Unpause();
			}
			menuIcon.SetActive (true);
			DisplayAIMenu();
		}
		menu.SetActive (false);
		if (lastCanvas == "game" && scene!="AI") {
			gameCanvas.SetActive (true);
		} else if (lastCanvas == "rhythm") {
			rhythmPracticeCanvas.SetActive (true);
		} else if (lastCanvas == "abg") {
			abgCanvas.SetActive (true);
		}

		//Object activation cascades from here:
		animationObject.SetActive (true);

		//GAMEPLAY STARTS HERE
		if (scene != "RhythmPractice" && scene != "abgPractice") {
			StartCoroutine (AirwayObstructionTimer ());
			StartCoroutine (HypoventilationTimer ());
			StartCoroutine (FluidResponseTimer ());
			StartCoroutine (OxygenTimer ());
			if (patient.bradyCardia) {
				StartCoroutine (PacingChecker ());
			}
		}
	}

	public void RhythmCheck()
	{
		mainCamera.transform.position = new Vector3(-2.9f, 0.04f, 0.4f);
		mainCamera.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		DeactivateCameras();
		defibCam.enabled = true;
	}

	public void RhythmPractice (string thisRhythm) {
		if (RhythmPracticeBool(thisRhythm)) {
			rhythmPracticeMessageText.text = "Excellent! ";
			rhythmPracticeCorrectInt++;
			if (rhythmPracticeCorrectInt > 1) {
				rhythmPracticeMessageText.text += rhythmPracticeCorrectInt + " in a row. ";
			}
			if (rhythmPracticeCorrectInt == 10) {
				rhythmPracticeMessageText.text += "\n\nYou're smashing it! ";
			}
			rhythmPracticeCorrect = true;
			StartCoroutine (RhythmPracticeCorrectEnum (1.5f));
		} else {
			rhythmPracticeCorrectInt = 0;
			rhythmPracticeMessageText.text = "Not quite! That was " + RhythmToMeaningful (rhythm);
			rhythmPracticeMessageText.text += ". Take another look... ";
			rhythmPracticeCorrect = false;
			StartCoroutine (RhythmPracticeCorrectEnum (2.5f));
		}

		rhythmPracticeMessageScreen.SetActive (true);
		rhythmPracticeButtonPanel.SetActive (false);
	}

	public bool RhythmPracticeBool(string thisRhythm) {
		if (rhythm == thisRhythm) {
			return true;
		} else {
			return false;
		}
	}

	public void RhythmPracticeContinue () {
		if (rhythmPracticeCorrect) {
			//Couple of lines to stop duplicate rhythms:
			string nextRhythm = patient.RandomRhythm (true);
			while (nextRhythm == rhythm || nextRhythm == "bigeminy") {
				nextRhythm = patient.RandomRhythm (true);
			}

			rhythm = nextRhythm;
			control.RemoteChangeECG (rhythm);
			if (rhythm == "af" || rhythm == "nsr") {
				control.RemoteChangeHeartRate (UnityEngine.Random.Range (30f, 90f));
			} else if (rhythm == "svt") {
				control.RemoteChangeHeartRate (UnityEngine.Random.Range (130f, 200f));
			}
			rhythmPracticeWaiting = true;
		} else {
			rhythmPracticeButtonPanel.SetActive (true);
		}
		Unpause();
		rhythmPracticeMessageScreen.SetActive (false);

	}

	public void RhythmPracticeResume () {
		StartCoroutine (RhythmPracticeResumeEnum ());
	}

	IEnumerator RhythmPracticeCorrectEnum (float wait) {
		yield return new WaitForSeconds (wait);
		RhythmPracticeContinue ();
	}

	IEnumerator RhythmPracticeResumeEnum () {
		yield return new WaitForSeconds (1);
		rhythmPracticeButtonPanel.SetActive (true);
	}
		
	 void RhythmTextButtonTextChange()
	{
		if (cardiacArrest) {
			if (checkRhythmText.text != "Next rhythm check") {
				checkRhythmText.text = "Next rhythm check";
			}
		}
	}

	public void SatsProbeOn () {
		if (Clickable) {
			drawerSatsProbe.SetActive (false);
			animationTesting.satsProbe.SetActive (true);
			control.satsScriptToComeOn = true;
			control.satsScript.satsText.text = "WAIT";
		}
	}

    public void SendMessage(string message, int delay, int timer, bool button)
    {
        //Do not use "StopAllCoroutines()" here - will mess with the SwitchCPRButtons() function
		if (nonBlockingMode) {
			button = false;
		}

		DispatchMemo(message);
        StartCoroutine(PulseMessageEnumerator(message, delay, timer, button));
    }

	void SetDrawerCam (string drawer) {
		if (mainCam.enabled && !drawersOpeningArrestDebug) {
			mainCam.enabled = false;
			drawerCam.enabled = true;
			if (drawersAllClosed) {
				drawersAllClosed = false;
			}
		} else if (drawersOpeningArrestDebug) {
			if (airwayDrawerOpen) {
				OpenCloseAirwayDrawer ();
			}
			if (breathingDrawerOpen) {
				OpenCloseBreathingDrawer ();
			}
			if (circulationDrawerOpen) {
				OpenCloseCirculationDrawer ();
			}
			if (drugDrawerOpen) {
				OpenCloseDrugDrawer ();
			}
		}

		if (drawer == "airway") {
			drawerCam.transform.position = new Vector3 (drawerCam.transform.position.x,
				8.4f, drawerCam.transform.position.z);
		} else if (drawer == "breathing") {
			drawerCam.transform.position = new Vector3 (drawerCam.transform.position.x,
				5f, drawerCam.transform.position.z);
		} else if (drawer == "circulation") {
			drawerCam.transform.position = new Vector3 (drawerCam.transform.position.x,
				0f, drawerCam.transform.position.z);
		} else if (drawer == "drugs" && Screen.width > 2000f) {
			drawerCam.transform.position = new Vector3 (drawerCam.transform.position.x,
				-6.4f, drawerCam.transform.position.z);
		} else if (drawer == "drugs") {
			drawerCam.transform.position = new Vector3 (drawerCam.transform.position.x,
				-8.4f, drawerCam.transform.position.z);
		}
	}

	public void SetPatientFactors () {
		scene = patient.scene;
		//Variables listed by ABCDE approach:
		//("airwayObstructed" doesn't need to be set as no other scripts access it - can just
		//be accessed as patient.airway when needed. Same true for some other variables.)
		sats = patient.sats;
		respRate = patient.respRate;
		randomRhythm = patient.randomRhythm;
		rhythmList = patient.rhythmList;
		MAPList = patient.MAPList;
	}

	public void SetRandomGame () {

	}

	private void SetResps (int newResps) {
		respRate = newResps;
		control.respScript.ClientChangeResps (newResps / 40f);
	}

	private void SetSats (int newSats) {
		float satsFloat = (float)newSats;
		if (satsGenerator.activeSelf) {
			satsGenerator.GetComponent<Sats> ().ClientChangeSats ((satsFloat - 70f) / 30f);
		}
	}

	public void Shock()
	{
		Control control = controller.GetComponent<Control>();
		control.Change(false);
	}
    
	public void ShockOrNot (bool shocking) {
		shockOrNotButtonPanel.SetActive(false);

		if (ShockOrNotChecker (shocking)) {
			//Code for success here
			shockOrNotRounds++;
			shockOrNotRoundsText.text = "Rounds - " + shockOrNotRounds.ToString ("0");
			shockOrNotWaiting = true;
			if (shockOrNotRounds < 10f) {
				//Couple of lines to stop duplicate rhythms:
				string nextRhythm = patient.RandomRhythm (true);
				while (nextRhythm == rhythm) {
					nextRhythm = patient.RandomRhythm (true);
				}
				rhythm = nextRhythm;
				control.RemoteChangeECG (rhythm);
				if (rhythm == "af" || rhythm == "nsr") {
					control.RemoteChangeHeartRate (UnityEngine.Random.Range (30f, 90f));
				} else if (rhythm == "svt") {
					control.RemoteChangeHeartRate (UnityEngine.Random.Range (130f, 200f));
				}
			} else {
				Pause();
				shockOrNotMessageScreen.SetActive(true);
				string thisRhyth = RhythmToMeaningful (rhythm);
				shockOrNotMessageText.text = "Nicely done!\n\n";
				shockOrNotMessageText.text += "Your time was " + shockOrNotTotal.ToString("00.00") + " seconds.\n\n";
				/*if (fbHolder.loggedIn) {
					shockOrNotMessageText.text += "Click the Facebook icon below to submit your score and see how your friends are doing! ";
					shockOrNotOKButton.SetActive (false);
					submitFBbutton.SetActive (true);
				} else {
					shockOrNotMessageText.text += "To see how your friends are doing and submit your own score, log in to Facebook from the home page.\n\n";
					shockOrNotOKButton.SetActive (true);
				}*/
			}
		} else {
			//Code for failure here
			Pause();
			shockOrNotMessageScreen.SetActive(true);
			menuIcon.SetActive (false);
			aiMenu.SetActive (false);
			string thisRhyth = RhythmToMeaningful (rhythm);
			shockOrNotMessageText.text = "Oops! That was " + thisRhyth + ", which is ";
			if (rhythm == "vt" || rhythm == "torsades" || rhythm == "vf") {
				shockOrNotMessageText.text += "shockable.\n\n";
			} else {
				shockOrNotMessageText.text += "non-shockable (PEA).\n\n";
			}
			shockOrNotMessageText.text += "Keep studying, and better luck next time!\n\n(Press \"OK\" to return to the main menu.)";
		}
	}

	private bool ShockOrNotChecker(bool shocking) {
		if (shocking) {
			if (rhythm == "vt" || rhythm == "torsades" || rhythm == "vf") {
				return true;
			} else {
				return false;
			}
		} else {
			if (rhythm == "vt" || rhythm == "torsades" || rhythm == "vf") {
				return false;
			} else {
				return true;
			}
		}
	}

	public void ShockOrNotContinue () {
		Reload ();
	}

	public void ShockOrNotResume () {
		shockOrNotWaiting = false;
		StartCoroutine (ShockOrNotResumeEnum ());
	}

	IEnumerator ShockOrNotResumeEnum () {
		yield return new WaitForSeconds (1);
		shockOrNotButtonPanel.SetActive (true);
	}

	public void ShockOrNotSubmitScore () {
		float shockOrNotSubTot = shockOrNotTotal * 1000f;
		string thisScore = shockOrNotSubTot.ToString ("0");
		//fbHolder.SetScore (thisScore);
	}

    IEnumerator SoundWithDelay (string sound, int delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(sound);
                   }
		
	public void StartChestCompressions () {
		if (checkRhythmText.text == "Pulse check") {
			SendMessage ("Maybe you should check for a pulse before starting CPR", 0, 2, true);
		} else {
			offChest = false;
			if (debugging) {
				Debug.Log (secondsOffChest);
			}
			PlaySequence ("CPR");
			if (chestCompressionButton.activeSelf) {
				chestCompressionButton.SetActive (false);
				ArrangeButtons ();
			}
		}
	}

	public void StartBaggingWithCPR () {
		patient.airwayObstructed = false;
		drawerBVM.SetActive (false);
		PlaySequence ("Bagging");
	}

	public void StopAllSounds()
	{
		soundPlayer.GetComponent<SoundPlayer>().StopAllSounds();
	}

	private string RhythmToMeaningful (string str) {
		if (str == "mobitzI") {
			return "Mobitz I";
		} else if (str == "mobitzII") {
			return "Mobitz II";
		} else if (str == "chb") {
			return "complete heart block";
		} else if (str == "aflutter") {
			return "atrial flutter";
		} else if (str != "bigeminy" && str != "torsades") {
			return str.ToUpper ();
		} else {
			return str;	
		}
	}

	public void SoundToggle () {
		soundOn = !soundOn;
		if (soundOn) {
			PlayerPrefs.SetInt ("Sound", 0);
		} else {
			PlayerPrefs.SetInt ("Sound", 1);
		}
	}

	public void SuspendCanvas(int seconds)
	{
		StartCoroutine(SuspendCanvasEnumerator(seconds));
	}

	//Just a forwarder to CanvasSlider()
	public void SuspendCanvas()
	{
		CanvasSlider();
	}

	IEnumerator SuspendCanvasEnumerator(int seconds)
	{
	/* "dontSuspendCanvas" is a lazy fix for a bug, where canvas would disappear
	 * if user clicked the OK button to quickly during the rhythm check after a patient had arrest
	 * (to do with switching from CPR to non-CPR buttons). The function that ends a cardiac arrest
	 * sets dontSuspendCanvas to true */
			
		if (!dontSuspendCanvas) {
			CanvasSlider ();
			yield return new WaitForSeconds (seconds);
			CanvasSlider ();
			ArrangeButtons ();
		}
	}

	public void SwitchCPRButtons()
	{
		//The delay gives the canvas time to slide off screen before changing button around, for a slicker look
		StartCoroutine(CPRButtonDelaySwitch());
	}

	public void TemporaryDrugMessage()
	{
		string messageText = "Oops! The only cardiac drugs you need in an arrest are adrenaline and amiodarone. ";
		SendMessage(messageText, 0, 8, true);
	}    

	public void TimeOffChest()
	{
		if (secondsOffChest > 60f)
		{
			minsOffChest += 1f;
			secondsOffChest -= 60f;
		}
		timeOffChestText.text = "Time off chest:\n";
		timeOffChestText.text += minsOffChest.ToString("00") + ":";
		timeOffChestText.text += secondsOffChest.ToString("0.00");
		timeOffChestText.text += "\n\nRhythm checks:\n";
		timeOffChestText.text += rhythmChecks.ToString("00");
		timeOffChestText.text += "\n\nAverage per check:\n" + averageTimeOffChestString;

	}

	//Toggled directly by "Rhythm Check" and "Resume CPR buttons" via GUI
	public void ToggleOffChest()
	{
		//Updates average time off chest each time CPR is resumed
		if (offChest)
		{
			AverageTimeOffChest();
		}

		offChest = !offChest;

		Debug.Log ("Off chest : " + offChest);
	}

	public void Venflon () {
		if (Clickable) {
			drawerVenflon.SetActive (false);
			animationTesting.cannula.SetActive (true);
		}
	}

	public void ViewMonitor() {
		if (Clickable) {
			if (scene == "CPR") {
				monitorTexts = monitorButton.GetComponentsInChildren<Text> ();
			} else {
				monitorTexts = monitorButtonNCPR.GetComponentsInChildren<Text> ();
			}
			if (!viewingMonitor) {
				DeactivateCameras (); //Must come before button text change and viewingMonitor change (resets them both)
				monitorCam.enabled = true;
				if (examinationButtons.activeSelf) {
					CanvasSliderExaminationOnly ();
				}
				//Just faffing for practice at not having to hook up so many objects to script in editor
				foreach (Text monitorText in monitorTexts) {
					monitorText.text = "Back";
				}
				viewingMonitor = true;
			} else {
				DeactivateCameras (); //Will automatically set button text back to "View monitor"
				mainCam.enabled = true;
				if (!examinationButtons.activeSelf) {
					CanvasSliderExaminationOnly ();
				}
			}
		}
  	}

	public void Yankeur () {
		if (Clickable) {
			if (patient.conscious) {
				string message = "You try to use the Yankeur suction catheter on your conscious patient.\n\n" +
				                 "He has some suggestions about where else you could stick it. ";
				SendMessage (message, 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
			} else if (patient.airwayObstructed && patient.airwayObstruction != "Tongue") {
				string message = "The "; 
				if (patient.airwayObstruction == "Vomit") {
					message += "vomit";
				} else {
					message += "blood";
				}
				message += " clears with some suction. ";
				if (drawerGuedel.activeSelf == true) {
					patient.airwayObstruction = "Tongue";
				} else {
					patient.airwayObstructed = false;
				}
				SendMessage (message, 0, 2, true);
				drawerYankeur.SetActive (false);

			} else if (!patient.airwayObstructed || patient.airwayObstruction == "Tongue") {
				string message = "There's not really anything to suction out of the airway. ";
				SendMessage (message, 0, 2, true);
				DispatchFeedback(Feedback.Blunder);
			}
		}
	}

	//GAMEPLAY ENUMERATORS
	//Note: important that delay is introduced at beginning of timer functions that are triggered by start gameplay.
	//Allows scene to set itself up before elements are called by these functions:
	IEnumerator AdenosineTimer() {
		control.adenosine = true;
		yield return new WaitForSeconds (5);
		if (rhythm == "svt") {
			NextRhythm ();
		}
		control.adenosine = false;
	}

	IEnumerator AirwayObstructionTimer () {
		if (!cardiacArrest) {
			yield return new WaitForSeconds (2);
			if (patient.airwayObstructed && !cardiacArrest) {
				sats -= 1;
				SetSats (sats);
				if (sats < 65) {
					arrestCause = "hypoxia secondary to airway obstruction";
					CardiacArrestSetter ();
				}
			} else if (!airwayCleared) {
				airwayCleared = true;
				sats = patient.sats;
				if (drawerNRBMask.activeSelf == false || animationTesting.goDoc2.activeSelf) {
					sats += patient.oxygenResponse;
				}
				SetSats (sats);
			}
			StartCoroutine (AirwayObstructionTimer ());
		}
	}

    IEnumerator AsthmaTimer ()
    {
        if (!cardiacArrest)
        {
            yield return new WaitForSeconds(2);
        }
    }

	IEnumerator AtropineTimer () {
		atropineRunning = true;
		int i = 0;
		while (i < 10) {
			heartRate += patient.atropineResponsiveness / 10f;
			control.RemoteChangeHeartRate (heartRate);
			yield return new WaitForSeconds (18);
			i++;
		}
		atropineRunning = false;
	}

	IEnumerator DefibHintTimer () {
		yield return new WaitForSeconds (10);
		if (defibFocus && !control.heartRateLabel.activeSelf) {
			if (scene == "DemoCPR") {
				string message = "To deliver a shock, you can interact with the defib directly by pressing any of the buttons." +
				                 "\n \n(Start by switching it on.)\n\nIf you think the rhythm is non-shockable, press the \"Resume CPR\" button in the Action Menu " +
				                 "on the left of the screen to resume CPR. ";
				SendMessage (message, 0, 2, true);
			} else {
				string message = "To use the defib, you can interact with it directly by pressing any of the buttons." +
					"\n \n(Start by switching it on.)\n\nRemember to synchronise shocks for a patient who is not in cardiac arrest, " +
					"and remember that to pace a patient you need both the defib pads and three-lead monitoring attached. ";
				SendMessage (message, 0, 2, true);
			}
		}
		yield return null;
	}

	IEnumerator FluidRunningTimer () {
		yield return new WaitForSeconds (120);
		fluidsRunning = false;
		drawerFluids.SetActive (true);
		animationTesting.drip.SetActive (false);
		string message = "Those fluids have run through. ";
		SendMessage (message, 0, 2, true);
	}

	IEnumerator FluidResponseTimer () {
		if (!cardiacArrest && patient.fluidResponsiveness != 0f && !stable_patient) {
			if (fluidsRunning && MAP < 120f) {
				yield return new WaitForSeconds (10);
				if (!cardiacArrest) {
					MAP += patient.fluidResponsiveness / 12f;
					control.aLineScript.ClientChangeBP (MAP / 120f);
				}
				StartCoroutine(FluidResponseTimer ());
			} else {
				if (patient.fluidRefractoriness != 0) {
					yield return new WaitForSeconds (patient.fluidRefractoriness);
					if (!cardiacArrest) {
                        if (debugging)
                        {
                            Debug.Log("Changing MAP from fluids. Current MAP = " + MAP);
                        }
                        MAP--;
                        control.aLineScript.ClientChangeBP(MAP / 120f);
                        Debug.Log("MAP from fluid response = " + MAP + " (FR = " + patient.fluidRefractoriness);
                        
					}
					if (MAP < 20f && !cardiacArrest) {
						arrestCause = "hypovolaemia";
						CardiacArrestSetter ();
					}
					StartCoroutine (FluidResponseTimer ());
				}
			}
		}
	}

    //Actually this time just makes the patient arrest if their sats are too low for too long.
	IEnumerator OxygenTimer () {
		if (!cardiacArrest && !stable_patient) {
			int i = 0;
			yield return new WaitForSeconds (60);
			i++;
			if (!cardiacArrest) {
				if (i == 1) {
					if (sats < 75) {
						CardiacArrestSetter ();
						arrestCause = "hypoxia";
						yield return null;
					}
				} else if (i == 2) {
					if (sats < 80) {
						CardiacArrestSetter ();
						arrestCause = "hypoxia";
						yield return null;
					}
				} else if (i == 3) {
					if (sats < 85) {
						CardiacArrestSetter ();
						arrestCause = "hypoxia";
						yield return null;
					}
				} else if (i > 4) {
					yield return null;
				}
			}
		}
	}

	public IEnumerator PacingChecker () {
		if (control.pacing && control.capture && MAP < 100f) {
			MAP+=3f;
			control.aLineScript.ClientChangeBP (MAP / 120f);
			if (debugging) {
				Debug.Log ("Changing MAP to " + MAP);
			}
		}
		yield return new WaitForSeconds (3);
		StartCoroutine (PacingChecker ());
	}

	IEnumerator RhythmPracticeStarter () {
		float height = Screen.height / 10f;
		float width = Screen.width;

		RectTransform rt = topBar.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (1f, height);

		rt = bottomBar.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (1f, height);
		bottomBar.transform.position = new Vector3 (bottomBar.transform.position.x,
			height,
			bottomBar.transform.position.z);

		List<GameObject> rhythmButtons = new List<GameObject> ();
		rhythmButtons.Add(nsrButton);
		rhythmButtons.Add(mobitzIButton);
		rhythmButtons.Add(mobitzIIButton);
		rhythmButtons.Add(afButton);
		rhythmButtons.Add(aFlutterButton);
		rhythmButtons.Add(svtButton);
		rhythmButtons.Add(bigeminyButton);
		rhythmButtons.Add(vtButton);
		rhythmButtons.Add(vfButton);
		rhythmButtons.Add(chbButton);
		rhythmButtons.Add(torsadesButton);

		for (int i = 0; i < rhythmButtons.Count; i++) {
			rt = rhythmButtons [i].GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2 (width/11f, height*0.9f);
			float z = rhythmButtons [i].transform.position.z;
			if (rhythmButtons [i] == chbButton) {
				rhythmButtons [i].transform.position = new Vector3 (width/10f, height * 9.95f, z);
			} else if (rhythmButtons [i] == torsadesButton) {
				rhythmButtons [i].transform.position = new Vector3 (width/10f * 9f, height * 9.95f, z);
			} else {
				rhythmButtons [i].transform.position = new Vector3 (width/10f * (i+1f), height * 0.95f, z);
			}
		}

		yield return new WaitForSeconds (1);
		rhythmPracticeMessageText.text = "Practice your rhythm recognition skills by identifying as many rhythms " +
			"as you can!\n\nReady in 4... ";
		yield return new WaitForSeconds (1);
		rhythmPracticeMessageText.text = "Practice your rhythm recognition skills by identifying as many rhythms " +
			"as you can!\n\nReady in 3... ";
		yield return new WaitForSeconds (1);
		rhythmPracticeMessageText.text = "Practice your rhythm recognition skills by identifying as many rhythms " +
			"as you can!\n\nReady in 2... ";
		yield return new WaitForSeconds (1);
		rhythmPracticeMessageText.text = "Practice your rhythm recognition skills by identifying as many rhythms " +
			"as you can!\n\nReady in 1... ";
		yield return new WaitForSeconds (1);
		screenMask.SetActive (false);
		rhythmPracticeMessageScreen.SetActive (false);
		rhythmPracticeButtonPanel.SetActive (true);
		menuIcon.SetActive (true);
		DisplayAIMenu();
	}

	IEnumerator ShockOrNotStarter () {
		yield return new WaitForSeconds (2);
		shockOrNotSplash.SetActive (false);
		shockOrNotCanvas.SetActive (true);
		yield return new WaitForSeconds (1);
		shockOrNotMessageText.text = "Practice your arrest rhythm recognition skills by seeing how quickly you can " +
			"decide which rhythms are shockable!\n\nGet 10 in a row correct to join the Facebook leaderboard!\n\nReady in 4... ";
		yield return new WaitForSeconds (1);
		shockOrNotMessageText.text = "Practice your arrest rhythm recognition skills by seeing how quickly you can " +
			"decide which rhythms are shockable!\n\nGet 10 in a row correct to join the Facebook leaderboard!\n\nReady in 3... ";
		yield return new WaitForSeconds (1);
		shockOrNotMessageText.text = "Practice your arrest rhythm recognition skills by seeing how quickly you can " +
			"decide which rhythms are shockable!\n\nGet 10 in a row correct to join the Facebook leaderboard!\n\nReady in 2... ";
		yield return new WaitForSeconds (1);
		shockOrNotMessageText.text = "Practice your arrest rhythm recognition skills by seeing how quickly you can " +
			"decide which rhythms are shockable!\n\nGet 10 in a row correct to join the Facebook leaderboard!\n\nReady in 1... ";
		yield return new WaitForSeconds (1);
		screenMask.SetActive (false);
		shockOrNotOKButton.SetActive (true);
		shockOrNotMessageScreen.SetActive (false);
		shockOrNotButtonPanel.SetActive (true);
		shockOrNotTotal = 0f;
		menuIcon.SetActive (true);
		DisplayAIMenu();
	}

	IEnumerator HypoventilationTimer () {
		if (!cardiacArrest && !stable_patient) {
			DispatchFeedback(Feedback.Blunder);
			yield return new WaitForSeconds (patient.respRate + 2);
			if (patient.respRate < 8 && drawerBVM.activeSelf && !cardiacArrest && !patient.airwayObstructed) {
                sats -= 1;
                SetSats(sats);
                if (sats < 65)
                {
                    arrestCause = "hypoxia secondary to hypoventilation";
                    CardiacArrestSetter();
                }
			} else if (!drawerBVM.activeSelf && !baggingStarted) {
				baggingStarted = true;
				sats = patient.sats + patient.oxygenResponse;
				SetSats (sats);
				SetResps (12);
			}
			StartCoroutine (HypoventilationTimer ());
		}
	}

	IEnumerator SatsIncrease (int newSats) {
		while (newSats > sats && sats < 100) {
			sats++;
			SetSats (sats);
			yield return new WaitForSeconds (1);
		}
	}

	IEnumerator TryCardiacArrest () {
		if (!drawersAllClosed) {
			yield return new WaitForSeconds (1);
			StartCoroutine (TryCardiacArrest ());
		} else {
			CardiacArrestSetter ();
			yield return null;
		}
	}

    IEnumerator SignsOfLifeCheck ()
    {
        Clickable = false;
        if (!dontSuspendCanvas)
        {
            SuspendCanvas();
        }
        else
        {
            useDefibButton.GetComponentInChildren<Text>().text = "Back";
            animationTesting.goDoc1.SetActive(false);
            offChest = false;
        }
        dontSuspendCanvas = false;
        doubleClickBlocker = true;
        DeactivateCameras();
        signsOfLifeCam.enabled = true;
        int[] numbers = new int[] { 9,8,7,6,5,4,3,2,1 };
        foreach (int number in numbers)
        {
            yield return new WaitForSeconds(1);
            signsOfLifeText.text = "Checking for carotid pulse...\n\n" + number;
        }
        yield return new WaitForSeconds(1);
        signsOfLifeText.text = "There's no pulse and no respiratory effort! ";
        yield return new WaitForSeconds(3);
        cardiacArrest = true;
        CPRButtons.SetActive(true);
        nonCPRButtons.SetActive(false);
        examinationButtons.SetActive(false);
        chestCompressionButton.SetActive(true);
        drawersOpeningArrestDebug = true;
        DeactivateCameras();
        mainCam.enabled = true;
        defibFocus = false;
        if (!animationTesting.goDoc2.activeSelf)
        {
            baggingDuringCPRButton.SetActive(true);
        }
        if (!drawerDefibPads.activeSelf)
        {
            attachPadsButton.SetActive(false);
        }
        if (!drawerVenflon.activeSelf)
        {
            iVaccessButton.SetActive(false);
            if (!animationTesting.drip.activeSelf)
            {
                fluidsButton.SetActive(true);
            }
            drugsButton.SetActive(true);
        }
        signsOfLifeButton.SetActive(false);
        checkRhythmText.text = "Rhythm check";
        ArrangeButtons();
        CanvasSlider();
    }

	// Update is called once per frame
    void Update()
    {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		//Debug.Log ("Frame rate: " + 1f / deltaTime);

        DrawerUpdate();
        CanvasPosition();
		AirwayManoeuvresButtonsMove ();
		CPRTimerUpdate ();
        if (offChest)
        {
            secondsOffChest += Time.deltaTime;
            TimeOffChest();
        }

		if (Input.GetKeyDown (KeyCode.Escape)) {
			GoToMenu ();
		}
		if (activateRhythmPractice) {
			ActivateRhythmPractice ();
		} else if (activateABGpractice) {
			ActivateABGPractice ();
		} else if (activateShockOrNot) {
			ActivateShockOrNot ();
		}

		if (shockOrNotCanvas.activeSelf && shockOrNotButtonPanel.activeSelf) {
			shockOrNotTotal += Time.deltaTime;
			shockOrNotTimer.text = "Time - " + shockOrNotTotal.ToString("00.00");
		}
        
        //Following function allows for the fact that the user might hit the view monitor button (thus triggering the "AddClinicalInformation" button) before
        //the traces are actually being displayed on the screen
        if (viewingMonitor || defibFocus)
        {
            if (viewingMonitor)
            {
                if ((control.aLineScript.aLineOn || control.aLineScript.bpCuffOn) && !clinical_information_obtained.ContainsValue("BP"))
                {
                    clinical_information_obtained.Add(Time.time, "BP");
					DispatchMeasurement(Insights.MeasuredMAP, MAP);
                }
                if (control.monitorPadsOn && !clinical_information_obtained.ContainsValue("ECG"))
                {
                    clinical_information_obtained.Add(Time.time, "ECG");
					DispatchECG();
                }
                if (control.satsScript.satsOn && !clinical_information_obtained.ContainsValue("Sats"))
                {
                    clinical_information_obtained.Add(Time.time, "Sats");
					DispatchMeasurement(Insights.MeasuredSats, sats);
                }
                if (control.respScript.respOn && !clinical_information_obtained.ContainsValue("Resps"))
                {
                    clinical_information_obtained.Add(Time.time, "Resps");
					DispatchMeasurement(Insights.MeasuredResps, respRate);
                }
            }
            else if (defibFocus)
            {
                if (animationTesting.defibPads.activeSelf && control.defibReady && !clinical_information_obtained.ContainsValue("ECG"))
                {
                    clinical_information_obtained.Add(Time.time, "ECG");
					DispatchECG();
                }
            }
        }

        if (Input.GetKeyDown("q")) TogglePause();
    }

	public void Pause() {
		Debug.Log("Pausing");
		Time.timeScale = 0;
	}

	public void Unpause() {
		Debug.Log("Unpausing");
		Time.timeScale = masterTimeScale;
	}

	public void TogglePause() {
		if (Time.timeScale == 0) Unpause();
		else Pause();
	}
}
