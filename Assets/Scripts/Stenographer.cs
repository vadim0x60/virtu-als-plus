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
        OnMemo(sender, insight.ToString());
    }

    public void OnMeasurement(object sender, Measurement measurement) 
    { 
        // Newer measures will overwite older ones, it's intended behavior
        measurements[measurement.Measurable] = measurement.Value;
        insightAges[(int)measurement.Measurable] = 1;
        OnMemo(sender, measurement.ToString());
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

    public void Recollect(Action<float> reportObservation)
    {
        int insightAge, idx;
        float insightRelevance;

        for (idx = 0; idx < insightsCount; idx++) {
            insightAge = insightAges[idx];
            insightRelevance = insightAge > 0 ? 1 / (float)insightAge : 0;
            reportObservation(insightRelevance);
        }

        foreach (Insights mi in Measurement.MeasurableInsights) {
            float observation = measurements.ContainsKey(mi) ? measurements[mi] : DefaultMeasurement;
            reportObservation(observation);
        }

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

