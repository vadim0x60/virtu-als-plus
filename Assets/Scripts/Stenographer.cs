using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Stenographer : IObserver<Insights>, IObserver<Measurement>
{
    private int[] insightAges;
    private readonly int insightsCount;

    private Dictionary<Insights, float> measurements = new Dictionary<Insights, float>();
    public float DefaultMeasurement = 0f;

    public void OnNext(Insights insight) 
    { 
        insightAges[(int)insight] = 1;
    }

    public void OnNext(Measurement measurement) 
    { 
        // Newer measures will overwite older ones, it's intended behavior
        measurements[measurement.Measurable] = measurement.Value;
    }

    public void OnError(Exception e) 
    {
        throw e;
    }

    public void OnCompleted()
    {
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

    public Stenographer() {
        insightsCount = Enum.GetNames(typeof(Insights)).Length;
        insightAges = new int[insightsCount];

        for (int idx = 0; idx < insightsCount; idx++) {
            insightAges[idx] = -1;
        }
    }
}

