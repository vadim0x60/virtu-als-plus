﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private DefibOn defibOn;
    private Control defibController;

    private Stenographer stenographer;
    private MemoChannel memoChannel = new MemoChannel(new Guid ("bdb17919-c516-44da-b045-a2191e972dec"));

    private Focus currentFocus = Focus.General;

    private void toggleFocus() {
        switch(currentFocus) {
            case Focus.AirwayDrawer:
                hub.OpenCloseDrawerFromButton("Airway");
                break;
            case Focus.CirculationDrawer:
                hub.OpenCloseDrawerFromButton("Circulation");
                break;
            case Focus.DrugsDrawer:
                hub.OpenCloseDrawerFromButton("Drugs");
                break;
            case Focus.BreathingDrawer:
                hub.OpenCloseDrawerFromButton("Breathing");
                break;
            case Focus.Defibrillator:
                hub.FocusOnDefib();
                break;
            case Focus.Monitor:
                hub.ViewMonitor();
                break;
        }
    }

    public Focus PlayerFocus {
        get {return currentFocus;}
        set {
            if (currentFocus != value) {
                toggleFocus();
                currentFocus = value;
                toggleFocus();
            }
        }
    }


    public DirectPlayer() {
        rewardProfile.AddReward = AddReward;
        stenographer = new Stenographer();
        MaxStep = 0;
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

        defibController = hub.controller.GetComponent<Control>();
        defibOn = hub.defibOnDefibButton.GetComponent<DefibOn>();
    }

    public override void OnEpisodeBegin()
    {
        actionCount = 0;
    }

    public void OnClickabilityChange(object sender, bool clickable) {
        if (clickable && actionQueue.Count > 0) {
            act(actionQueue.Dequeue());
        }
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
        act(action);
    }

    private void act(int action) 
    {
        switch (action) {
            case 0:
                // Do nothing
                break;
            case 1:
                PlayerFocus = Focus.BreathingDrawer;
                break;
            case 2:
                PlayerFocus = Focus.General;
                break;
            case 3:
                PlayerFocus = Focus.DrugsDrawer;
                break;
            case 4:
                PlayerFocus = Focus.DrugsDrawer;
                break;
            case 5:
                PlayerFocus = Focus.DrugsDrawer;
                break;
            case 6:
                PlayerFocus = Focus.DrugsDrawer;
                break;
            case 7:
                PlayerFocus = Focus.DrugsDrawer;
                break;
            case 8:
                PlayerFocus = Focus.CirculationDrawer;
                break;
            case 9:
                PlayerFocus = Focus.AirwayDrawer;
                break;
            case 10:
                PlayerFocus = Focus.CirculationDrawer;
                break;
            case 11:
                PlayerFocus = Focus.CirculationDrawer;
                break;
            case 12:
                PlayerFocus = Focus.AirwayDrawer;
                break;
            case 13:
                PlayerFocus = Focus.AirwayDrawer;
                break;
            case 14:
                PlayerFocus = Focus.BreathingDrawer;
                break;
            // Defibrillator:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
                PlayerFocus = Focus.Defibrillator;
                break;
            // Assessment:
            case 28:
                PlayerFocus = Focus.General;
                hub.AssessResponse();
                break;
            case 29:
                PlayerFocus = Focus.General;
                hub.AssessAirway();
                break;
            case 30:
                PlayerFocus = Focus.General;
                hub.AssessBreathing();
                break;
            case 31:
                PlayerFocus = Focus.General;
                hub.AssessCirculation();
                break;
            case 32:
                PlayerFocus = Focus.General;
                hub.AssessDisability();
                break;
            case 33:
                PlayerFocus = Focus.General;
                hub.AssessExposure();
                break;
            case 34:
                PlayerFocus = Focus.Defibrillator;
                // Assess defibrillator
                break;
            case 35:
                PlayerFocus = Focus.Monitor;
                // Assess monitor
                break;
            // FINISH:
            case 36:
                break;
        }

        if (!AdviceMode) {
            if (hub.Clickable) {
                GameObject clickee = null;

                switch (action) {
                    case 0:
                        // Do nothing
                        break;
                    case 1:
                        clickee = hub.drawerABG;
                        break;
                    case 2:
                        clickee = hub.airwayManoeuvresButton;
                        break;
                    case 3:
                        hub.AtropineGiven();
                        break;
                    case 4:
                        hub.AdenosineGiven();
                        break;
                    case 5:
                        hub.AdrenalineGiven();
                        break;
                    case 6:
                        hub.AmiodaroneGiven();
                        break;
                    case 7:
                        hub.MidazolamGiven();
                        break;
                    case 8:
                        clickee = hub.drawerVenflon;
                        break;
                    case 9:
                        clickee = hub.drawerYankeur;
                        break;
                    case 10:
                        clickee = hub.drawerVacutainers;
                        break;
                    case 11:
                        clickee = hub.drawerBPCuff;
                        break;
                    case 12:
                        clickee = hub.drawerBVM;
                        break;
                    case 13:
                        clickee = hub.drawerGuedel;
                        break;
                    case 14:
                        clickee = hub.drawerNRBMask;
                        break;
                    // Defibrillator:
                    case 15:
                        clickee = defibOnDefibButton;
                        break;
                    case 16:
                        clickee = hub.attachPadsButton;
                        break;
                    case 17:
                        clickee = hub.contol.shockButton;
                        break;
                    case 18:
                        clickee = hub.control.chargeButton;
                        break;
                    case 19:
                        defibController.ChangePaceCurrent("down");
                        break;
                    case 20:
                        defibController.ChangePaceCurrent("up");
                        break;
                    case 21:
                        defibController.EnergyDown();
                        break;
                    case 22:
                        defibController.EnergyUp();
                        break;
                    case 23:
                        defibController.ChangePaceRate("down");
                        break;
                    case 24:
                        defibController.ChangePaceRate("up");
                        break;
                    case 25:
                        clickee = hub.control.paceButton;
                        break;
                    case 26:
                        clickee = hub.control.pacePauseButton;
                        break;
                    case 27:
                        clickee = hub.control.syncButton;
                        break;
                    // FINISH:
                    case 36:
                        hub.Done();
                        break;

                    if (clickee != null) {
                        if (clickee.activeSelf) clickee.OnClick();
                        else {
                            rewardProfile.OnFeedback(this, Feedback.Blunder);
                            memoChannel.OnMemo("... this button seems to be disabled ...");
                        }
                    }
                }

                if (action > 0) {
                    actionCount++;
                }
            }
            else {
                actionQueue.Enqueue(action);
            }
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