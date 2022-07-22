using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
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
    void AssertClickability() {
        if (!hub.clickable) {
            throw new GameNotClickableException();
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
        hub.Subscribe((IObserver<Insights>)stenographer);
        hub.Subscribe((IObserver<Measurement>)stenographer);
        hub.Subscribe((IObserver<Feedback>)rewardProfile);
        defibController = hub.controller.GetComponent<Control>();
        defibOn = hub.defibOnDefibButton.GetComponent<DefibOn>();
    }

    public override void CollectObservations(VectorSensor vs)
    {
        if (rewardProfile.Done) { 
            EndEpisode();
        }
        else {
            AssertClickability();

            stenographer.Recollect(vs.AddObservation);
            rewardProfile.OnNext(Feedback.Tick);
        }
    }

    // Update is called once per frame
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AssertClickability();

        int action = actionBuffers.DiscreteActions[0];
        if (action > 0) {
            actionCount++;
        }

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
                PlayerFocus = Focus.Defibrillator;
                break;
            case 16:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 17:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 18:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 19:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 20:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 21:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 22:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 23:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 24:
                PlayerFocus = Focus.Defibrillator;
                break;
            case 25:
                PlayerFocus = Focus.Defibrillator;
                break;
            // Assessment:
            case 26:
                PlayerFocus = Focus.General;
                hub.AssessResponse();
                break;
            case 27:
                PlayerFocus = Focus.General;
                hub.AssessAirway();
                break;
            case 28:
                PlayerFocus = Focus.General;
                hub.AssessBreathing();
                break;
            case 29:
                PlayerFocus = Focus.General;
                hub.AssessCirculation();
                break;
            case 30:
                PlayerFocus = Focus.General;
                hub.AssessDisability();
                break;
            case 31:
                PlayerFocus = Focus.General;
                hub.AssessExposure();
                break;
            case 32:
                PlayerFocus = Focus.Defibrillator;
                // Assess defibrillator
                break;
            case 33:
                PlayerFocus = Focus.Monitor;
                // Assess monitor
                break;
            // FINISH:
            case 34:
                break;
        }

        if (!AdviceMode) {
            switch (action) {
                case 0:
                    // Do nothing
                    break;
                case 1:
                    hub.ABG();
                    break;
                case 2:
                    hub.AirwayManoeuvresButton();
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
                    hub.Venflon();
                    break;
                case 9:
                    hub.Yankeur();
                    break;
                case 10:
                    hub.Bloods();
                    break;
                case 11:
                    hub.BPCuffOn();
                    break;
                case 12:
                    hub.BVM();
                    break;
                case 13:
                    hub.Guedel();
                    break;
                case 14:
                    hub.NRBMask();
                    break;
                // Defibrillator:
                case 15:
                    defibOn.OnMouseDown();
                    break;
                case 16:
                    defibOn.AttachPads ();
                    hub.AttachPads ();
                    break;
                case 17:
                    hub.Shock();
                    break;
                case 18:
                    hub.ToggleOffChest ();
                    defibController.Charge ();
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
                    defibController.Pace();
                    break;
                // FINISH:
                case 34:
                    hub.Done();
                    break;
            }
        }
    }
}

public class GameNotClickableException : InvalidOperationException {

}