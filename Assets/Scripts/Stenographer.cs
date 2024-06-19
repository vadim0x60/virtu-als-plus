using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;

class Stenographer: SideChannel
{
    private int[] insightAges;
    private readonly int insightsCount;

    private Dictionary<Insights, float> measurements = new Dictionary<Insights, float>();
    public float DefaultMeasurement = 0f;

    public void OnInsight(object sender, Insights insight) 
    { 
        insightAges[(int)insight] = 1;
    }

    public void OnMeasurement(object sender, Measurement measurement) 
    { 
        // Newer measures will overwite older ones, it's intended behavior
        measurements[measurement.Measurable] = measurement.Value;
        insightAges[(int)measurement.Measurable] = 1;
    }

    public void OnMemo(object sender, string memo) {
        using (var msgOut = new OutgoingMessage())
        {
            msgOut.WriteString(memo);
            QueueMessageToSend(msgOut);
        }
    }

    protected override void OnMessageReceived(IncomingMessage msg) {
        var receivedString = msg.ReadString();
        Debug.Log("Memo received: " + receivedString);
    }

    public void OnError(Exception e) 
    {
        throw e;
    }

    private void ageInsights()
    {
        for (int idx = 0; idx < insightsCount; idx++) {
            if (insightAges[idx] > 0) {
                insightAges[idx]++;
            }
        }
    }

    public void summarizeEpisode() 
    {
        int insightAge;

        foreach(Insights insightType in Enum.GetValues(typeof(Insights))) {
            insightAge = insightAges[(int)insightType];
            if (insightAge == 1) {
                if (Measurement.MeasurableInsights.Contains(insightType)) {
                    OnMemo(this, $"{insightType}: {measurements[insightType]}");
                }
                else {
                    OnMemo(this, insightType.ToString());
                }
            }
        }
    }

    public void Recollect(Action<float> reportObservation)
    {
        int insightAge, idx;
        float observation;

        foreach(Insights insightType in Enum.GetValues(typeof(Insights))) {
            insightAge = insightAges[(int)insightType];
            observation = insightAge > 0 ? 1 / (float)insightAge : 0;
            reportObservation(observation);
        }
        
        foreach (Insights mi in Measurement.MeasurableInsights) {
            observation = measurements.ContainsKey(mi) ? measurements[mi] : DefaultMeasurement;
            reportObservation(observation);
        }

        summarizeEpisode();
        ageInsights();
    }

    public Stenographer(Guid channelId) {
        ChannelId = channelId;

        insightsCount = Enum.GetNames(typeof(Insights)).Length;
        insightAges = new int[insightsCount];

        for (int idx = 0; idx < insightsCount; idx++) {
            insightAges[idx] = -1;
        }
    }
}

