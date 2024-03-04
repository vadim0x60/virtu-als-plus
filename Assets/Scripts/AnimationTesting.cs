using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimationTesting : MonoBehaviour
{
    public bool debugging = true;
    public bool playing = false;

    public GameObject goDoc1;
    public GameObject goDoc2;
    public GameObject goDoc2HandLeft;
    public GameObject goPatient;
    public GameObject goBVM;
    public GameObject goBed;
    public GameObject goSittingProps;
    public GameObject defibPads;
    public GameObject cmPads;
    public GameObject cmPadsLying;
    public GameObject cannula;
    public GameObject defibController;
    public GameObject mainCam;
    public GameObject screen;
    public GameObject pulseText;
    public GameObject jawThrustCam;
    public GameObject drugDrawer;
    public GameObject drip;
    public GameObject followBone;
    public GameObject satsProbe;
    public GameObject bpCuff;
    public GameObject nrbMask;
    public GameObject controlObject;
    public GameObject guedel;
    public GameObject cprObject;

    public Text resumeText;
    public Text debugText;

    public Hub hub;

    public Control controller;

    Animation animDoc1;
    Animation animDoc2;
    Animation animPatient;
    Animation animBVM;
    Animation animBed;

    //Had to lift these out of the SoundPlayer script for timing issues
    public AudioSource deliveringShock;
    public AudioSource shockDelivered;
    public AudioSource nonShockableRhythm;

    Vector3 jawThrustCamPos;
    Quaternion jawThrustCamRot;

    bool doc1BendingForwards = true;
    public bool doc2BendingForwards = true;
    bool checkingSignsOfLife = false;
    bool headTilt = false;
    bool jawThrust = false;
    bool nrbMaskWasActive = false;
    bool airwayWasObstructed = false;
    bool speechEnabled = false;
    bool actualCPR = false;

    private string scene = "";
    public string lastPlayed = "";

    private float testTimer = 0f;
    private float previousCPRpos = 0f;
    private float compPercent = 0f;

    private int compressions = 0;
    private int cprFails = 0;

    public Slider cprSlider;

    private List<float> compressionSpeed;
    private List<string> cprStandard;

    //For assessing framerate
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    // Use this for initialization
    void Start()
    {
        if (debugging)
        {
            Debug.Log("Animation script starting...");
        }
        scene = hub.scene;

        speechEnabled = hub.soundPlayer.GetComponent<SoundPlayer>().speechEnabled;

        animDoc1 = goDoc1.GetComponent<Animation>();
        animDoc2 = goDoc2.GetComponent<Animation>();
        animPatient = goPatient.GetComponent<Animation>();
        animBVM = goBVM.GetComponent<Animation>();
        animBed = goBed.GetComponent<Animation>();

        jawThrustCamPos = jawThrustCam.transform.position;
        jawThrustCamRot = jawThrustCam.transform.rotation;

        // Speed up bagging
        animDoc2["D2_BVM"].speed = 1.5f;
        animPatient["P_BVM"].speed = 1.5f;
        animBVM["BVM_Compress"].speed = 1.5f;
        animDoc1["D1_CPR_CC_Resume"].speed = 1.5f;

        // Link BVM to doctors hand
        goBVM.transform.SetParent(goDoc2HandLeft.transform);
        //cmPads.transform.SetParent (followBone.transform);


        if (scene == "CPR" || scene == "DemoCPR") {
            //Hide extra equipment
            defibPads.SetActive(false);
            cannula.SetActive(false);
            defibController.SetActive(false);
            drip.SetActive(false);

            // Link BVM to doctors hand
            goBVM.transform.SetParent(goDoc2HandLeft.transform);

            // Set initial scene
            animBed.Play("Bed_LyingPos");
            animPatient.Play("P_Lying_Pose");
            animDoc2.Play("D2_BVM_Idle");
            goDoc1.active = true;
            goDoc2.active = true;

            //Remember this should be 30, 2!
            StartCoroutine(CPR(30, 2));

        } else if (scene == "Conscious") {
            //Hide extra equipment
            defibPads.SetActive(false);
            cmPads.SetActive(false);
            cannula.SetActive(false);
            defibController.SetActive(false);
            drip.SetActive(false);

            // Set initial scene
            StartCoroutine(SitIdle());
        } else if (scene == "Unconscious") {
            //Hide extra equipment
            NRBmaskLying(true);
            defibPads.SetActive(false);
            cmPads.SetActive(false);
            cannula.SetActive(false);
            defibController.SetActive(false);
            drip.SetActive(false);

            // Set initial scene
            StartCoroutine(LieIdle());
        } else if (scene == "AI")
        {
            //Hide extra equipment
            NRBmaskLying(true);
            defibPads.SetActive(false);
            cmPads.SetActive(false);
            cannula.SetActive(false);
            drip.SetActive(false);
            if (debugging)
            {
                Debug.Log("Animation script AI set-up starting...");
            }
            doc1BendingForwards = false;
            //AttachPads(true);

            // Set initial scene
            StartCoroutine(LieIdle());
        }

        controlObject.SetActive(true);
        if (debugging)
        {
            Debug.Log("Control object set active by AnimationTesting");
        }

        compressionSpeed = new List<float>();
        cprStandard = new List<string>();
    }

    public void NRBmaskLying(bool lying) {
        if (lying) {
            nrbMask.transform.localPosition = new Vector3(0f, 0.873f, 0.308f);
            Vector3 nrbRot = Quaternion.ToEulerAngles(nrbMask.transform.localRotation);
            nrbRot = new Vector3(-55.943f, nrbRot.y, nrbRot.z);
            nrbMask.transform.localRotation = Quaternion.Euler(nrbRot);

            bpCuff.transform.localPosition = new Vector3(0.554f, 0.82518f, 0.269f);
            nrbRot = new Vector3(44.719f, 18.78f, -166.60f);
            bpCuff.transform.localRotation = Quaternion.Euler(nrbRot);
            if (cmPads.activeSelf) {
                cmPads.SetActive(false);
                cmPadsLying.SetActive(true);
            }
        } else {
            Vector3 zeroPos = new Vector3(0f, 0f, 0f);
            nrbMask.transform.localPosition = zeroPos;
            nrbMask.transform.localRotation = Quaternion.Euler(zeroPos);
            bpCuff.transform.localPosition = zeroPos;
            bpCuff.transform.localRotation = Quaternion.Euler(zeroPos);
            if (cmPads.activeSelf) {
                cmPads.SetActive(true);
                cmPadsLying.SetActive(false);
            }
        }
    }

    public void CardiacMonitorPadsOn() {
        if (hub.patient.conscious) {
            cmPads.SetActive(true);
        } else {
            cmPadsLying.SetActive(true);
        }
    }

    public void PlaySequence(string anim)
    {
        if (debugging) {
            Debug.Log("PlaySequence " + anim);
        }
        //WAIT FOR CERTAIN ANIMATIONS TO FINISH
        if (!playing)
        {
            StopAllCoroutines();
            animDoc1["D1_CPR_CC_Pause"].time = 0;
            animDoc1["D1_CPR_CC_Pause"].speed = 1.0f;
            animDoc2["D2_JawThrust"].speed = 1.0f;
            if (jawThrust)
            {
                animPatient["P_JawThrust"].speed = -1f;
                animPatient["P_JawThrust"].time = animPatient["P_JawThrust"].length;
                animDoc2["D2_JawThrust"].speed = -1f;
                animDoc2["D2_JawThrust"].time = animDoc2["D2_JawThrust"].length;
                StartCoroutine(JawThrust(anim));
            }

            else if (headTilt)
            {
                animPatient["P_HeadTilt"].speed = -1f;
                animPatient["P_HeadTilt"].time = animPatient["P_HeadTilt"].length;
                animDoc1["D1_HeadTilt"].speed = -1f;
                animDoc1["D1_HeadTilt"].time = animDoc1["D1_HeadTilt"].length;
                StartCoroutine(HeadTilt(anim));
            }

            else
            {
                switch (anim)
                {

                    case "CPR":
                        StartCoroutine(CPR(30, 2));
                        break;

                    case "BVM":
                        StartCoroutine(BVM());
                        break;

                    case "Bagging":
                        StartCoroutine(Bagging());
                        break;

                    case "LieIdle":
                        StartCoroutine(LieIdle());
                        break;

                    case "SitIdle":
                        StartCoroutine(SitIdle());
                        break;

                    case "WrongDefib":
                        if (defibPads.active)
                        {
                            StartCoroutine(WrongDefib());
                        }
                        break;

                    case "Defib":
                        if (defibPads.active)
                        {
                            StartCoroutine(Defib());
                        }
                        break;

                    case "Arrested":
                        StartCoroutine(Arrested());
                        break;

                    case "RhythmCheck":
                        StartCoroutine(RhythmCheck());
                        break;

                    case "RhythmCheck_AI":
                        StartCoroutine(RhythmCheck_AI());
                        break;

                    case "SignsOfLifeCheck":
                        StartCoroutine(SignsofLifeCheck());
                        break;

                    case "Back":
                        StartCoroutine(Back());
                        break;

                    case "JawThrust":
                        StartCoroutine(JawThrust());
                        break;

                    case "HeadTilt":
                        StartCoroutine(HeadTilt());
                        break;

                    case "Idle":
                        StartCoroutine(DocsIdle());
                        break;

                    case "PulseCheck":
                        StartCoroutine(PulseCheck());
                        break;

                    case "CPR_one_doc":
                        StartCoroutine(CPR_one_doc());
                        break;

                    case "CPR_AI":
                        StartCoroutine(CPR_AI());
                        break;

                    case "Defib_AI":
                        StartCoroutine(Defib_AI());
                        break;

                    case "CPR_pause_AI":
                        StartCoroutine(CPR_pause_AI());
                        break;

                    case "LastPlayed":
                        LastPlayed();
                        break;

                    default:
                        Debug.LogError("No animation sequence with name : " + anim);
                        break;
                }
            }
        }
    }

    public void AttachPads(bool attach)
    {
        if (attach)
        {
            defibPads.SetActive(true);
            hub.drawerDefibPads.SetActive(false);
            defibController.SetActive(true);
        } else
        {
            defibPads.SetActive(false);
            defibController.SetActive(false);
        }
    }

    IEnumerator CPR(int compressionLoops, int bvmLoops)
    {
        lastPlayed = "CPR";
        // Set model visibility
        goDoc1.SetActive(true);

        if (!doc1BendingForwards)
        {
            animDoc1.Play("D1_CPR_CC_Resume");
        }

        if (goDoc2.active)
        {
            if (nrbMask.activeSelf) {
                nrbMask.SetActive(false);
                hub.drawerNRBMask.SetActive(true);
            }
            goBVM.SetActive(true);
            animPatient["P_BVM"].speed = 1.5f;
            animDoc2["D2_BVM"].speed = 1.5f;
            animBVM["BVM_Compress"].speed = 1.5f;
            if (!doc2BendingForwards)
            {
                animDoc2.Play("D2_BVM_Resume");
            }
        }

        // Set bed position
        animBed.Play("Bed_LyingPos");

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        if (!hub.CPRButtons.activeSelf && !hub.messageScreen.active && !hub.actualCPR)
        {
            hub.ResumeCanvas();
            hub.ArrangeButtons();
        }

        doc1BendingForwards = true;
        doc2BendingForwards = true;

        if (cmPads.activeSelf) {
            cmPads.SetActive(false);
            cmPadsLying.SetActive(true);
        }

        if (hub.actualCPR && hub.patient.arrest) {
            Debug.Log("Hub actual CPR");
            actualCPR = true;
        } else {
            while (true) {
                // Run animation sequence
                for (int i = 0; i < compressionLoops; i++) {
                    animDoc1.Play("D1_CPR_CC_Loop");
                    animPatient.Play("P_CPR_CC_Loop");
                    hub.PlaySound("CPR");

                    while (animDoc1.isPlaying || animPatient.isPlaying) {
                        yield return null;
                    }
                }

                if (goDoc2.activeSelf) {
                    animDoc1.Play("D1_CPR_CC_Pause");

                    while (animDoc1.isPlaying) {
                        yield return null;
                    }

                    for (int i = 0; i < bvmLoops; i++) {
                        animDoc2.Play("D2_BVM");
                        animPatient.Play("P_BVM");
                        animBVM.Play("BVM_Compress");
                        hub.PlaySound("Vent");

                        while (animDoc2.isPlaying || animPatient.isPlaying) {
                            yield return null;
                        }
                        if (i == 0) {
                            yield return new WaitForSeconds(1);
                        }
                    }

                    animDoc1.Play("D1_CPR_CC_Resume");
                    doc1BendingForwards = true;
                }

                while (animDoc1.isPlaying) {
                    yield return null;
                }

                yield return null;
            }
        }
    }

    IEnumerator CPR_one_doc()
    {
        //animDoc1["D1_CPR_CC_Loop"].speed = 0.65f;
        //animPatient["P_CPR_CC_Loop"].speed = 0.65f;
        int compressionLoops = 30;
        lastPlayed = "CPR";
        // Set model visibility
        goDoc1.SetActive(true);

        if (!doc1BendingForwards)
        {
            animDoc1.Play("D1_CPR_CC_Resume");
        }

        // Set bed position
        animBed.Play("Bed_LyingPos");

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        doc1BendingForwards = true;
        doc2BendingForwards = true;

        if (cmPads.activeSelf)
        {
            cmPads.SetActive(false);
            cmPadsLying.SetActive(true);
        }

        while (true)
        {
            // Run animation sequence
            for (int i = 0; i < compressionLoops; i++)
            {
                animDoc1.Play("D1_CPR_CC_Loop");
                animPatient.Play("P_CPR_CC_Loop");
                hub.PlaySound("CPR");

                while (animDoc1.isPlaying || animPatient.isPlaying)
                {
                    yield return null;
                }
            }

            while (animDoc1.isPlaying)
            {
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator CPR_AI()
    {
        //animDoc1["D1_CPR_CC_Loop"].speed = 0.65f;
        //animPatient["P_CPR_CC_Loop"].speed = 0.65f;
        lastPlayed = "CPR";
        // Set model visibility
        goDoc1.SetActive(true);

        if (!doc1BendingForwards)
        {
            animDoc1.Play("D1_CPR_CC_Resume");
        }

        if (goDoc2.active)
        {
            if (nrbMask.activeSelf)
            {
                nrbMask.SetActive(false);
                hub.drawerNRBMask.SetActive(true);
            }
            goBVM.SetActive(true);
            animPatient["P_BVM"].speed = 1.5f;
            animDoc2["D2_BVM"].speed = 1.5f;
            animBVM["BVM_Compress"].speed = 1.5f;
            if (!doc2BendingForwards)
            {
                animDoc2.Play("D2_BVM_Resume");
            }
        }

        // Set bed position
        animBed.Play("Bed_LyingPos");

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        doc1BendingForwards = true;
        doc2BendingForwards = true;

        if (cmPads.activeSelf)
        {
            cmPads.SetActive(false);
            cmPadsLying.SetActive(true);
        }
        else
        {
            while (true)
            {
                // Run animation sequence
                for (int i = 0; i < 30; i++)
                {
                    animDoc1.Play("D1_CPR_CC_Loop");
                    animPatient.Play("P_CPR_CC_Loop");
                    hub.PlaySound("CPR");

                    while (animDoc1.isPlaying || animPatient.isPlaying)
                    {
                        yield return null;
                    }
                }

                if (goDoc2.activeSelf)
                {
                    animDoc1.Play("D1_CPR_CC_Pause");

                    while (animDoc1.isPlaying)
                    {
                        yield return null;
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        animDoc2.Play("D2_BVM");
                        animPatient.Play("P_BVM");
                        animBVM.Play("BVM_Compress");
                        hub.PlaySound("Vent");

                        while (animDoc2.isPlaying || animPatient.isPlaying)
                        {
                            yield return null;
                        }
                        if (i == 0)
                        {
                            yield return new WaitForSeconds(1);
                        }
                    }

                    animDoc1.Play("D1_CPR_CC_Resume");
                    doc1BendingForwards = true;
                }

                while (animDoc1.isPlaying)
                {
                    yield return null;
                }

                yield return null;
            }
        }
    }

    IEnumerator BVM() {
        if (!animDoc1.isPlaying && !animDoc2.isPlaying) {

            if (doc1BendingForwards) {
                animDoc1.Play("D1_CPR_CC_Pause");

            }
            doc1BendingForwards = false;

            animDoc2.Play("D2_BVM");
            animPatient.Play("P_BVM");
            animBVM.Play("BVM_Compress");
            hub.PlaySound("Vent");

            while (animDoc2.isPlaying || animPatient.isPlaying) {
                yield return null;
            }

            if (lastPlayed == "BVM") {
                animDoc1.Play("D1_CPR_CC_Resume");

                while (animDoc1.isPlaying) {
                    yield return null;
                }

                doc1BendingForwards = true;


                if (compressions == 30 && compPercent >= 80f) {
                    actualCPR = false;
                    hub.actualCPR = false;
                    PlaySequence("CPR");
                    hub.CanvasSlider();
                    hub.ArrangeButtons();
                    debugText.gameObject.SetActive(false);
                    hub.canvasBVM.SetActive(false);
                    hub.DeactivateCameras();
                    hub.mainCam.enabled = true;
                    hub.SendMessage("The arrest trolley has arrived! Your team take over CPR, " +
                        "and you move to the end of the bed to oversee the arrest.", 0, 0, true);
                } else {
                    compressionSpeed.Clear();
                    cprStandard.Clear();
                    compressions = 0;
                    cprFails++;
                    if (cprFails == 3) {
                        string failer = "<b>Oops!</b>\n\n";
                        failer += "Having failed to deliver effective CPR during three consecutive cycles, ";
                        failer += "you decide it's time to let someone more experienced take over while you contact ";
                        failer += "your resuscitation officer about arranging some refresher training basic life support...\n\n";
                        failer += "(Select the menu icon at the top of the screen to return to the Main Menu)";
                        hub.DemoEnd(failer);
                    }
                }

            } else {
                lastPlayed = "BVM";
            }
            yield return null;
        }
    }

    IEnumerator Bagging()
    {
        animDoc2["D2_BVM"].speed = 1f;
        animPatient["P_BVM"].speed = 1f;
        animBVM["BVM_Compress"].speed = 1f;
        lastPlayed = "Bagging";
        if (hub.airwayManoeuvresButton.activeSelf) {
            hub.airwayManoeuvresButton.SetActive(false);
        }
        goDoc2.SetActive(true);
        goBVM.SetActive(true);
        if (goDoc1.active)
        {
            PlaySequence("CPR");
            yield return null;
        }
        else {
            if (nrbMask.activeSelf) {
                nrbMask.SetActive(false);
                hub.drawerNRBMask.SetActive(true);
            }
            // Set bed position
            animBed.Play("Bed_LyingPos");

            if (!doc2BendingForwards) {
                animDoc2.Play("D2_BVM_Resume");
            }

            while (animDoc2.isPlaying) {
                yield return null;
            }

            while (true)
            {
                animDoc2.Play("D2_BVM");
                animPatient.Play("P_BVM");
                animBVM.Play("BVM_Compress");
                hub.PlaySound("Vent");

                while (animDoc2.isPlaying || animPatient.isPlaying)
                {
                    yield return null;
                }
                yield return new WaitForSeconds(3.5f);
            }
        }
    }

    IEnumerator WrongDefib()
    {
        Debug.Log("Wrong defib");
        // Set model visibility
        /*goDoc1.SetActive(true);
        goDoc2.SetActive(true);*/


        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        if (goDoc1.active)
        {
            animDoc1.Play("D1_CPR_CC_Pause");
        }
        if (goDoc2.active)
        {
            animDoc2.Play("D2_BVM_Pause");
        }
        animPatient.Play("P_Lying_Pose");

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        animPatient.Play("P_Defib");
        hub.PlaySound("Shock");

        while (animPatient.isPlaying)
        {
            yield return null;
        }
        hub.ActivateDeactivateFailScreen(true);
    }

    IEnumerator Defib()
    {
        NRBmaskLying(true);

        playing = true;

        hub.SuspendCanvas();

        if (speechEnabled) {
            deliveringShock.Play();
        }
        hub.SendMessage("\"Everybody clear, delivering shock!\"", 0, 2, false);

        // Set model visibility
        /*goDoc1.SetActive(true);
        goDoc2.SetActive(true);*/

        // Set bed position
        animBed.Play("Bed_LyingPos");

        hub.ToggleOffChest();

        // Run animation sequence
        if (goDoc1.active)
        {
            animDoc1.Play("D1_CPR_CC_Pause");
        }

        if (goDoc2.active)
        {
            animDoc2.Play("D2_BVM_Pause");
        }

        animPatient.Play("P_Lying_Pose");

        while (animDoc1.isPlaying || animDoc2.isPlaying || deliveringShock.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        animPatient.Play("P_Defib");

        hub.PlaySound("Shock");

        while (animPatient.isPlaying)
        {
            yield return null;
        }

        if (hub.shockFail)
        {
            hub.Fail();
        }
        else if (hub.MAP == 0f)
        {
            if (speechEnabled) {
                shockDelivered.Play();
            }
            hub.SendMessage("\"Shock delivered, resume CPR.\"", 0, 2, false);
            yield return new WaitForSeconds(0.5f);
        }

        if (goDoc1.active)
        {
            animDoc1.Play("D1_CPR_CC_Resume");
            doc1BendingForwards = true;
        }
        if (goDoc2.active)
        {
            animDoc2.Play("D2_BVM_Resume");
            doc2BendingForwards = true;
        }

        hub.ToggleOffChest();

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        playing = false;

        if (goDoc1.active) {
            StartCoroutine(CPR(30, 2));
        } else {
            hub.SuspendCanvas();
            if (lastPlayed == "SitIdle") {
                PlaySequence("LieIdle");
            }
        }

        if (debugging) {
            Debug.Log("Playing = " + playing);
        }
    }

    IEnumerator CPR_pause_AI()
    {
        animDoc1.Play("D1_CPR_CC_Pause");
        while (animDoc1.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator Defib_AI()
    {
        NRBmaskLying(true);

        playing = true;

        if (speechEnabled)
        {
            deliveringShock.Play();
        }

        animBed.Play("Bed_LyingPos");

        animPatient.Play("P_Lying_Pose");

        yield return new WaitForSeconds(0.5f);

        animPatient.Play("P_Defib");

        hub.PlaySound("Shock");

        while (animPatient.isPlaying)
        {
            yield return null;
        }

        
        animDoc1.Play("D1_CPR_CC_Resume");
        doc1BendingForwards = true;
        animDoc2.Play("D2_BVM_Resume");
        doc2BendingForwards = true;

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        playing = false;

        StartCoroutine(CPR_AI());
    }

    IEnumerator LieIdle() {
        lastPlayed = "LieIdle";
        NRBmaskLying(true);
        // Set model visibility
        if (hub.scene != "AI")
        {
            goDoc1.SetActive(false);
            goDoc2.SetActive(false);
        } else
        {
            goDoc1.SetActive(true);
            goDoc2.SetActive(true);
            animDoc1["D1_CPR_CC_Pause"].time = animDoc1["D1_CPR_CC_Pause"].length;
            animDoc1["D1_CPR_CC_Pause"].speed = 0;
            animDoc1.Play("D1_CPR_CC_Pause");
            animDoc1.Play("D1_CPR_CC_Pause");
            animDoc2["D2_JawThrust"].speed = 0;
            animDoc2.Play("D2_JawThrust");
        }

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Set RR
        float speed = (float)hub.respRate / 30f;
        animPatient["P_BVM"].speed = speed;

        // Run animation sequence
        while (true)
        {
            animPatient.Play("P_BVM");

            while (animPatient.isPlaying)
            {
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator SitIdle()
    {
        lastPlayed = "SitIdle";
        NRBmaskLying(false);
        // Set model visibility
        goDoc1.SetActive(false);
        goDoc2.SetActive(false);


        // Set bed position
        animBed.Play("Bed_SitPos");

        // Set RR
        float speed = (float)hub.respRate / 30f;
        animPatient["P_BVM"].speed = speed;

        // Run animation sequence
        while (true)
        {
            animPatient.Play("P_Sit_Idle_30bmp");

            while (animPatient.isPlaying)
            {
                yield return null;
            }

            yield return null;
        }

        // Reset RR
        animPatient["P_BVM"].speed = 1.5f;
    }

    IEnumerator Arrested()
    {
        NRBmaskLying(true);
        lastPlayed = "Arrested";
        if (cmPads.activeSelf) {
            cmPads.SetActive(false);
            cmPadsLying.SetActive(true);
        }

        // Set bed position
        animBed.Play("Bed_LyingPos");
        animPatient.Play("P_Lying_Pose");

        yield return null;
    }

    IEnumerator RhythmCheck()
    {
        if (hub.checkRhythmText.text == "Pulse check") {
            string message = "There's no pulse!";
            hub.SendMessage(message, 0, 3, true);
            hub.checkRhythmText.text = "Rhythm check";
            if (goDoc1.activeSelf) {
                PlaySequence("CPR");
            }
        } else if (hub.cardiacArrest && !goDoc1.activeSelf) {
            string message = "Oops! Your patient has arrested and you haven't started chest compressions!";
            hub.SendMessage(message, 0, 3, true);
        } else if (hub.cardiacArrest && hub.rhythmChecks == 1 && !goDoc2.activeSelf) {
            string message = "Are you really going to perform a whole cycle of CPR without bagging your patient...?";
            hub.SendMessage(message, 0, 3, true);
            PlaySequence("CPR");
        } else {
            hub.ToggleOffChest();
            if (defibPads.activeSelf) {
                hub.AddRhythmCheck();
            }
            hub.SwitchCPRButtons();
            hub.SuspendCanvas(2);
            hub.messageScreen.SetActive(false);

            playing = true;

            // Set model visibility
            /*goDoc1.SetActive(true);
        goDoc2.SetActive(true);*/


            // Set bed position
            animBed.Play("Bed_LyingPos");

            // Run animation sequence
            if (goDoc1.active) {
                animDoc1.Play("D1_CPR_CC_Pause");
                doc1BendingForwards = false;
            }
            if (goDoc2.active) {
                animDoc2.Play("D2_BVM_Pause");
                doc2BendingForwards = false;
            }
            animPatient.Play("P_Lying_Pose");

            while (animDoc1.isPlaying || animDoc2.isPlaying) {
                yield return null;
            }

            if (goDoc1.activeSelf) {
                resumeText.text = "Resume CPR";
            } else {
                resumeText.text = "Back";
            }

            if (debugging) {
                Debug.Log("End of rhythm check animation");
            }
            hub.FocusOnDefib();

            playing = false;
        }
    }

    IEnumerator RhythmCheck_AI()
    {

        playing = true;

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        if (goDoc1.active)
        {
            animDoc1.Play("D1_CPR_CC_Pause");
            doc1BendingForwards = false;
        }
        if (goDoc2.active)
        {
            animDoc2.Play("D2_BVM_Pause");
            doc2BendingForwards = false;
        }
        animPatient.Play("P_Lying_Pose");

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        playing = false;
    }

    IEnumerator SignsofLifeCheck()
    {
		lastPlayed = "SignsofLifeCheck";
        if (!checkingSignsOfLife)
        {
            // Set model visibility
            /*goDoc1.SetActive(true);
            goDoc2.SetActive(true);*/
            

            // Set bed position
            animBed.Play("Bed_LyingPos");

            // Run animation sequence
            if (goDoc1.active)
            {
                animDoc1.Play("D1_CPR_CC_Pause");
                doc1BendingForwards = false;
            }
            if (goDoc2.active)
            {
                animDoc2.Play("D2_BVM_Pause");
                doc2BendingForwards = false;
            }
            animPatient.Play("P_Lying_Pose");

            while (animDoc1.isPlaying || animDoc2.isPlaying)
            {
                yield return null;
            }

            checkingSignsOfLife = true;
            /*mainCam.GetComponent<Animation>().Play("SignsOfLifeCheck");
            while (mainCam.GetComponent<Animation>().isPlaying)
            {
                yield return null;
            }*/
            hub.RhythmCheck();
            pulseText.active = true;
        }
    }

    IEnumerator Back()
    {
		hub.defibFocus = false;
        //Debug.Log(checkingSignsOfLife);
        if (checkingSignsOfLife)
        {
            pulseText.active = false;
            /*
            mainCam.GetComponent<Animation>()["SignsOfLifeCheck"].speed = -1f;
            mainCam.GetComponent<Animation>()["SignsOfLifeCheck"].time = mainCam.GetComponent<Animation>()["SignsOfLifeCheck"].length;
            mainCam.GetComponent<Animation>().Play("SignsOfLifeCheck");
            while (mainCam.GetComponent<Animation>().isPlaying)
            {
                yield return null;
            }
            mainCam.GetComponent<Animation>()["SignsOfLifeCheck"].speed = 1f;*/
            checkingSignsOfLife = false;
        /*}
        else
        {
            */
        }
        
        hub.CameraToDefaultView(true);
        hub.DumpCharge();
        hub.StopAllSounds();

		if (goDoc1.activeSelf) {
			//NB: canvas will be re-enabled once the "bending forwards" animation has finished in CPR sequence
			if (debugging) {
				Debug.Log ("Suspending canvas");
			}
			hub.SuspendCanvas ();

			if (hub.controller.active && !hub.shockFail) {
				if (controller.defibReady) {
					if (speechEnabled) {
						nonShockableRhythm.Play ();
					}
					hub.SendMessage ("\"That's a non-shockable rhythm, resume CPR.\"", 0, 2, false);
				} else {
					hub.SendMessage ("\"Resume CPR. We need to attach pads and switch the defib on.\"", 0, 2, false);
				}
				yield return new WaitForSeconds (0.5f);
			}
			StartCoroutine (CPR (30, 2));
		} else if (goDoc2.activeSelf) {
			PlaySequence ("Bagging");
		}
        else
        {
			//AFTER CARDIAC ARREST A PATIENT WILL ALWAYS BE APNOEIC WITH THIS:
            StartCoroutine(Arrested());
        }
        yield return null;
    }

    IEnumerator JawThrust()
    {
        if (hub.patient.airwayObstructed) {
			airwayWasObstructed = true;
			hub.patient.airwayObstructed = false;
		}
        playing = true;

		/*hub.headTiltChinLiftButton.SetActive (false);
		hub.jawThrustButton.SetActive (false);
		hub.CanvasSlider ();*/
        
        // Set model visibility
        //goDoc1.SetActive(false);
        goDoc2.SetActive(true);
		goDoc1.SetActive (true);
        animDoc1["D1_CPR_CC_Pause"].time = animDoc1["D1_CPR_CC_Pause"].length;
        animDoc1["D1_CPR_CC_Pause"].speed = 0;
        animDoc1.Play("D1_CPR_CC_Pause");
        
        goBVM.SetActive(false);

		if (nrbMask.activeSelf) {
			nrbMask.SetActive (false);
			nrbMaskWasActive = true;
		}

        // Set bed position
        animBed.Play("Bed_LyingPos");

        /*mainCam.transform.position = jawThrustCamPos;
        mainCam.transform.rotation = jawThrustCamRot;*/

        // Run animation sequence
        animPatient.Play("P_JawThrust");
        animDoc2.Play("D2_JawThrust");
        
        //jawThrust = true;

        yield return new WaitForSeconds(13.5f);

        //hub.CameraToDefaultView();
        playing = false;
        /*
        if (debugging)
        {
            Debug.Log("Playing = false");
        }

		string message = "";
		if (hub.respRate == 0) {
			message = "You try a jaw thrust, but it doesn't help the fact that your patient is making no respiratory effort.";
			hub.airwayManoeuvresButton.SetActive (false);
			hub.ArrangeButtons ();
		} else if (hub.patient.airwayObstructed && hub.patient.airwayObstruction != "Tongue") {
			message = "You try a jaw thrust, but the patient is still gurgling";
		} else if (hub.patient.airwayObstructed) {
			message = "The jaw thrust stops the patient's snoring. Someone suggests you insert one of the Guedels " +
			"from the resus trolley to secure the patient's airway and free your colleague up for other tasks.";
		} else {
			message = "You perform a jaw thrust, but your colleague points out that the airway wasn't obstructed beforehand and " + 
				"decides to do something more useful.";
		}
		hub.SendMessage (message, 0, 2, true);
		//PlaySequence ("LieIdle");
        */
    }

	IEnumerator JawThrust(string anim)
	{
		if (airwayWasObstructed) {
			hub.patient.airwayObstructed = true;
			airwayWasObstructed = false;
		}
		playing = true;

		// Set model visibility
		//goDoc1.SetActive(false);
		goDoc2.SetActive(true);
		goDoc1.SetActive (false);
		goBVM.SetActive(false);

		// Set bed position
		animBed.Play("Bed_LyingPos");

		/*mainCam.transform.position = jawThrustCamPos;
		mainCam.transform.rotation = jawThrustCamRot;*/

		// Run animation sequence
		animPatient.Play("P_JawThrust");
		animDoc2.Play("D2_JawThrust");

		jawThrust = false;

		while (animPatient.isPlaying || animDoc2.isPlaying)
		{
			yield return null;
		}

		yield return new WaitForSeconds(1f);

		animPatient ["P_JawThrust"].speed = 1f;
		animPatient ["P_JawThrust"].time = 0f;
		animDoc2 ["D2_JawThrust"].speed = 1f;
		animDoc2 ["D2_JawThrust"].time = 0f;

		if (nrbMaskWasActive) {
			if (!nrbMask.activeSelf) {
				nrbMask.SetActive (true);
			}
		}

		playing = false;

		PlaySequence (anim);
	}

    IEnumerator PulseCheck()
    {
        // Set model visibility
        goDoc1.SetActive(true);
        // Run animation sequence
        animPatient.Play("P_HeadTilt");
        animDoc1.Play("D1_HeadTilt");
        while (animPatient.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator HeadTilt()
    {
		if (hub.patient.airwayObstructed) {
			airwayWasObstructed = true;
			hub.patient.airwayObstructed = false;
		}
        playing = true;

		hub.headTiltChinLiftButton.SetActive (false);
		hub.jawThrustButton.SetActive (false);
		hub.CanvasSlider ();

        // Set model visibility
        goDoc1.SetActive(true);
        goDoc2.SetActive(false);
        
		if (nrbMask.activeSelf) {
			//nrbMask.SetActive (false);
			nrbMaskWasActive = true;
		}

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        animPatient.Play("P_HeadTilt");
        animDoc1.Play("D1_HeadTilt");

        headTilt = true;

        while (animPatient.isPlaying)
        {
            yield return null;
        }

        playing = false;

		string message = "";
		if (hub.respRate == 0) {
			message = "You try a head tilt, but it doesn't help the fact that your patient is making no respiratory effort.";
			hub.airwayManoeuvresButton.SetActive (false);
			hub.ArrangeButtons ();
		} else if (hub.patient.airwayObstructed && hub.patient.airwayObstruction != "Tongue") {
			message = "You try a head tilt, but the patient is still gurgling";
		} else if (hub.patient.airwayObstructed) {
			message = "The jaw thrust stops the patient's snoring. Someone suggests you insert one of the Guedels " +
				"from the resus trolley to secure the patient's airway and free your colleague up for other tasks.";
		} else {
			message = "You perform a head tilt, but your colleague points out that the airway wasn't obstructed beforehand and " + 
				"decides to do something more useful.";
		}
		hub.SendMessage (message, 0, 2, true);
		PlaySequence ("LieIdle");

    }

    IEnumerator HeadTilt(string anim)
    {
		if (airwayWasObstructed) {
			hub.patient.airwayObstructed = true;
			airwayWasObstructed = false;
		}

        playing = true;

        // Set model visibility
        goDoc1.SetActive(true);
        goDoc2.SetActive(false);
        

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        animPatient.Play("P_HeadTilt");
        animDoc1.Play("D1_HeadTilt");

		headTilt = false;

        while (animPatient.isPlaying)
        {
            yield return null;
        }

		animPatient ["P_HeadTilt"].speed = 1f;
		animPatient ["P_HeadTilt"].time = 0f;
		animDoc1 ["D1_HeadTilt"].speed = 1f;
		animDoc1 ["D1_HeadTilt"].time = 0f;

		if (nrbMaskWasActive) {
			if (!nrbMask.activeSelf) {
				nrbMask.SetActive (true);
			}
		}

        playing = false;

        PlaySequence(anim);
    }

    IEnumerator DocsIdle ()
    {
        
        goBVM.SetActive(false);

        animDoc1.Play("D1_Idle");
        animDoc2.Play("D2_Idle");

        while (animDoc1.isPlaying)
        {
            yield return null;
        }
    }

	IEnumerator Compressions () {
		
		if (!animDoc1.IsPlaying ("D1_CPR_CC_Pause") && !animDoc1.IsPlaying ("D1_CPR_CC_Resume") && !animDoc2.isPlaying) {
			if (lastPlayed == "BVM" && compressions != 0) {
				compressionSpeed.Clear();
				cprStandard.Clear ();
				compressions = 0;
				cprFails++;
				if (cprFails == 3) {
					string failer = "<b>Oops!</b>\n\n";
					failer += "Having failed to deliver effective CPR during three consecutive cycles, ";
					failer += "you decide it's time to let someone more experienced take over while you contact ";
					failer += "your resuscitation officer about arranging some refresher training basic life support...\n\n";
					failer += "(Select the menu icon at the top of the screen to return to the Main Menu)";
					hub.DemoEnd (failer);
				}
			}
				
			lastPlayed = "Comp";

			bool docForward = false;

			if (animDoc1.isPlaying || animPatient.isPlaying) {
				animDoc1.Stop ();
				animPatient.Stop ();
				float t = Time.time - testTimer;
				animDoc1 ["D1_CPR_CC_Loop"].speed = 0.6f / t;
				animPatient ["P_CPR_CC_Loop"].speed = 0.6f / t;
			} else {
				animDoc1 ["D1_CPR_CC_Loop"].speed = 1f;
				animPatient ["P_CPR_CC_Loop"].speed = 1f;
			}

			if (!doc1BendingForwards) {
				animDoc1.Play ("D1_CPR_CC_Resume");
				docForward = true;
				while (animDoc1.isPlaying) {
					yield return null;
				}

				Debug.Log ("Bending forwards");

				doc1BendingForwards = true;
				compressionSpeed.Clear();
				cprStandard.Clear ();
				compressions = 0;
			}

			animDoc1.Play ("D1_CPR_CC_Loop");
			animPatient.Play ("P_CPR_CC_Loop");

			compressions++;
			if (compressions == 60) {
				string failer = "<b>Oops!</b>\n\n";
				failer += "Hands-only CPR is for cardiac arrests where ventilation is not possible. ";
				failer += "Your colleague is ready to ventilate the patient with a bag/valve mask, ";
				failer += "and under these circumstances you should deliver two breaths for every 30 chest compressions. ";
				failer += "Better luck next time!\n\n(To review the CPR guidelines, follow the link to the Resuscitation Council UK ";
				failer += "website from the app's Learning Zone. Click the menu icon at the top of the screen to return to the Main Menu.)";
				hub.DemoEnd (failer);
			}

			debugText.text = "Compressions: " + compressions;
			if (!docForward) {
				compressionSpeed.Add (Time.time - testTimer);
				testTimer = Time.time;
				float aveSpeed = 0f;
				float goodComps = 0f;
				if (compressionSpeed.Count > 3) {
					compressionSpeed.RemoveAt (0);
					foreach (float x in compressionSpeed) {
						aveSpeed += x;
					}
					aveSpeed = aveSpeed / 3f;
					if (aveSpeed >= 0.5f && aveSpeed <= 0.6f) {
						debugText.text += "\n<b><color=#62f442>Good!</color></b>";
						cprStandard.Add ("Good");
					} else if (aveSpeed > 0.6f) {
						debugText.text += "\n<b><color=#f4e841>Speed up!</color></b>";
						cprStandard.Add ("Bad");
					} else if (aveSpeed < 0.5f) {
						debugText.text += "\n<b><color=#f45b41>Slow down!</color></b>";
						cprStandard.Add ("Bad");
					}
					if (cprStandard.Count > 1) {
						foreach (string y in cprStandard) {
							if (y == "Good") {
								goodComps++;
							}
						}
						compPercent = (goodComps / cprStandard.Count) * 100f;
						if (compPercent >= 80f) {
							debugText.text += "<color=#62f442>";
						} else {
							debugText.text += "<color=#f45b41>";
						}
						debugText.text += "\nScore: " + compPercent.ToString ("0") + "%</color>";
					}
				}

				while (animDoc1.isPlaying || animPatient.isPlaying) {
					yield return null;
				}

				yield return null;
			}
		}
	}

	void LastPlayed () {
		PlaySequence (lastPlayed);
	}

    public void ActivateCannule ()
    {
        cannula.active = true;
		hub.drawerVenflon.SetActive (false);
		if (hub.cardiacArrest && !goDoc1.activeSelf) {
			string message = "Your colleague sites an IV line at your request, but asks if " +
			                 "you should think about starting CPR before worrying about drugs and fluids."; 
			hub.SendMessage (message, 0, 2, true);
		}
    }

    public void ActivateDrip ()
    {
        drip.active = true;
		hub.drawerFluids.SetActive (false);
    }
    
	public void CanvasBVM() {
		PlaySequence ("BVM");
	}

    void ManualCPRFunction ()
    {
        if (m_lastFramerate > 0f && m_lastFramerate < 40f)
        {
            actualCPR = false;
        }
        if (actualCPR)
        {
            if (SystemInfo.supportsAccelerometer)
            {
                float newPos = ((2f - (cprObject.transform.position.y)) / 2f);
                //debugText.text += newPos.ToString ("0.00") + "\n";
                if (newPos >= 0.5f && previousCPRpos < 0.5f)
                {
                    if (Time.time - testTimer > 0.2f)
                    {
                        StartCoroutine(Compressions());
                    }
                }
                /*if (newPos == 0f) {
					newPos = 0f;
					hub.CPRquality (0);
				}  
				if (newPos >= 1) {
					hub.CPRquality (2);
				} 
				if (newPos <= 0.33f && previousCPRpos > 0.33f) {
					hub.cprThirdwayPassed = true;
					hub.prevCPRheight = 3;
				} else if (newPos >= 0.66f && previousCPRpos < 0.66f) {
					hub.cprThirdwayPassed = true;
					hub.prevCPRheight = 4;
				}
				if ((newPos <= 0.5f && previousCPRpos > 0.5f) ||
					(newPos >= 0.5f && previousCPRpos < 0.5f)) {
					hub.CPRquality (1);
				} 
				animDoc1 ["D1_CPR_CC_Loop"].time = newPos * 0.6f;
				animPatient ["P_CPR_CC_Loop"].time = newPos * 0.6f;
				animDoc1.Play ("D1_CPR_CC_Loop");
				animPatient.Play ("P_CPR_CC_Loop");
				//debugText.text = cprObject.transform.position.y.ToString("0.0");*/
                previousCPRpos = newPos;
            }
            else
            {
                if (Input.GetKeyDown("space"))
                {
                    if (Time.time - testTimer > 0.2f)
                    {
                        StartCoroutine(Compressions());
                    }
                }
            }
        }
    }

    void FrameRateChecker ()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            if (m_frameCounter > 10)
            {
                m_lastFramerate = (float)m_frameCounter / m_timeCounter;
                m_frameCounter = 0;
                m_timeCounter = 0.0f;
            }
        }
    }

    public void ButtonPushCPR ()
    {
        if (actualCPR)
        {
            actualCPR = false;
            hub.buttonPushCPR.GetComponentInChildren<Text>().text = "Chest compression";
        }
        else
        {
            if (Time.time - testTimer > 0.2f)
            {
                StartCoroutine(Compressions());
            }
        }
    }

	void Update () {
        FrameRateChecker();

        ManualCPRFunction();
	}
}
