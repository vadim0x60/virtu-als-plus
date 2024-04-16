using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Unity.MLAgents;
using Unity.MLAgents.SideChannels;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public enum Focus {
    General,
    AirwayDrawer,
    CirculationDrawer,
    DrugsDrawer,
    BreathingDrawer,
    Defibrillator,
    Monitor
}

public class DirectPlayer : Agent
{
    public Hub hub;
    public bool AdviceMode = false;

    private int actionCount = 0;
    public int ActionCount {get{return actionCount;}}

    public readonly RewardCenter rewardProfile = new RewardCenter {
        FailureReward = -100f,
        SuccessReward = 100f,
        BlunderReward = -5f,
        TimestepReward = -0.1f
    };

    private Stenographer stenographer = new Stenographer(new Guid ("bdb17919-c516-44da-b045-a2191e972dec"));

    public DirectPlayer() {
        rewardProfile.AddReward = AddReward;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void Initialize() {
        SideChannelManager.RegisterSideChannel(memoChannel);

        hub.InsightDispatched += stenographer.OnInsight;
        hub.MeasurementDispatched += stenographer.OnMeasurement;
        hub.FeedbackDispatched += rewardProfile.OnFeedback;
        hub.MemoDispatched += memoChannel.OnMemo;
        hub.ClickabilityChanged += OnClickabilityChange;
        rewardProfile.Done += EndEpisode;
    }

    public override void OnEpisodeBegin()
    {
        actionCount = 0;
        actionQueue.Clear();
    }

    public void OnClickabilityChange(object sender, bool clickable) {
        if (clickable) act();
    }

    public override void CollectObservations(VectorSensor vs)
    {
        stenographer.Recollect(vs.AddObservation);
        rewardProfile.OnFeedback(this, Feedback.Tick);
    }

    private Queue<int> actionQueue = new Queue<int>();

    // Update is called once per frame
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int action = actionBuffers.DiscreteActions[0];
        actionQueue.Enqueue(action);
        act();
    }

    private bool midazolamGiven = false;

    public string[] ActionLabels {
        "DoNothing",
        "CheckSignsOfLife",
        "CheckRhythm",
        "ExamineAirway",
        "ExamineBreathing",
        "ExamineCirculation",
        "ExamineDisability",
        "ExamineExposure",
        "ExamineResponse",
        "SetUpIVAccess",
        "GiveFluids",
        "ViewMonitor",
        "StartChestCompression",
        "OpenAirwayDrawer",
        "OpenBreathingDrawer",
        "OpenCirculationDrawer",
        "OpenDrugsDrawer",
        "BagDuringCPR",
        "ResumeCPR",
        "UseMonitorPads",
        "UseSatsProbe",
        "UseAline",
        "UseBloodPressureCuff",
        "AttachDefibPads",
        "UseVenflon",
        "UseBagValveMask",
        "UseNonRebreatherMask",
        "UseYankeurSucionCatheter",
        "UseGuedelAirway",
        "TakeBloodForArtherialBloodGas",
        "TakeRoutineBloods",
        "PerformAirwayManoeuvres",
        "PerformHeadTiltChinLift",
        "PerformJawThrust",
        "TakeBloodPressure",
        "TurnOnDefibrillator",
        "DefibrillatorCharge",
        "DefibrillatorCurrentUp",
        "DefibrillatorCurrentDown",
        "DefibrillatorPace",
        "DefibrillatorPacePause",
        "DefibrillatorRateUp",
        "DefibrillatorRateDown",
        "DefibrillatorSync",
        "Finish"
    }

    public GameObject[][] ActionButtons {
        get {
            return new GameObject[][] {
                new GameObject[] {},
                new GameObject[] {hub.signsOfLifeButton},
                new GameObject[] {hub.checkRhythmButton},
                new GameObject[] {hub.examineAirwayButton},
                new GameObject[] {hub.examineBreathingButton},
                new GameObject[] {hub.examineCirculationButton},
                new GameObject[] {hub.examineDisabilityButton},
                new GameObject[] {hub.examineExposureButton},
                new GameObject[] {hub.examineResponseButton},
                new GameObject[] {hub.iVaccessButton, hub.drawerVenflon},
                new GameObject[] {hub.fluidsButton, hub.drawerFluids},
                new GameObject[] {hub.monitorButton, hub.monitorButtonNCPR},
                new GameObject[] {hub.chestCompressionButton},
                new GameObject[] {hub.airwayButton},
                new GameObject[] {hub.breathingButton},
                new GameObject[] {hub.circulationButton},
                new GameObject[] {hub.drugsButton, hub.drugsButtonNCPR},
                new GameObject[] {hub.baggingDuringCPRButton},
                new GameObject[] {hub.resumeButton},
                new GameObject[] {hub.drawerCmPads},
                new GameObject[] {hub.drawerSatsProbe},
                new GameObject[] {hub.drawerAline},
                new GameObject[] {hub.drawerBPCuff},
                new GameObject[] {hub.attachPadsButton, hub.drawerDefibPads},
                new GameObject[] {hub.drawerBVM},
                new GameObject[] {hub.drawerNRBMask},
                new GameObject[] {hub.drawerYankeur},
                new GameObject[] {hub.drawerGuedel},
                new GameObject[] {hub.drawerABG},
                new GameObject[] {hub.drawerVacutainers},
                new GameObject[] {hub.airwayManoeuvresButton},
                new GameObject[] {hub.headTiltChinLiftButton},
                new GameObject[] {hub.jawThrustButton},
                new GameObject[] {hub.nibpButton},
                new GameObject[] {hub.defibOnCanvasButton, hub.defibOnDefibButton, hub.useDefibButton},
                new GameObject[] {defibController.chargeButton},
                new GameObject[] {FindObjectsOfType(typeof(CurrentUpButton), true).FirstOrDefault()},
                new GameObject[] {FindObjectsOfType(typeof(CurrentDownButton), true).FirstOrDefault()},
                new GameObject[] {defibController.paceButton},
                new GameObject[] {defibController.pacePauseButton},
                new GameObject[] {FindObjectsOfType(typeof(RateUpButton), true).FirstOrDefault()},
                new GameObject[] {FindObjectsOfType(typeof(RateDownButton), true).FirstOrDefault()},
                new GameObject[] {defibController.syncButton},
                new GameObject[] {hub.doneButton}
            }
        }
    }

    private void act() 
    {
        Control defibController = hub.controller.GetComponent<Control>();

        while (actionQueue.Any()) {
            if (!hub.Clickable) return;

            int action = actionQueue.Peek();
            bool actionFailed = false;

            foreach (GameObject clickee in ActionButtons[action]) {
                if (clickee != null && clickee.activeSelf) {
                    clickee.SendMessage("OnMouseDown");
                    clickee.SendMessage("OnMouseUp");

                    actionFailed = false;
                    break;
                }
                else actionFailed = true;
            }

            if (actionFailed) {
                rewardProfile.OnFeedback(this, Feedback.Blunder);
                memoChannel.OnMemo(this, $"... {ActionLabels[action]} is not available in current state ...");
            }
            else {
                memoChannel.OnMemo(this, ActionLabels[action]});
                if (action != 0) actionCount++;
            }

            actionQueue.Dequeue();
        }
    }

    public IEnumerator EnsureAction() {
        int actionCountSnapshot = ActionCount;

        while (ActionCount == actionCountSnapshot) {
            RequestDecision();
            yield return new WaitForSeconds(5);
        }
    }

    public void OnDestroy()
    {
        if (Academy.IsInitialized) {
            SideChannelManager.UnregisterSideChannel(memoChannel);
        }
    }
}