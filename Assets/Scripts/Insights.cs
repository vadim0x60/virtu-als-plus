using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public enum Insights
{
    ResponseVerbal,
    ResponseGroan,
    ResponseNone,
    AirwayClear,
    AirwayVomit,
    AirwayBlood,
    AirwayTongue,
    BreathingNone,
    BreathingSnoring,
    BreathingSeeSaw,
    BreathingEqualChestExpansion,
    BreathingBibasalCrepitations,
    BreathingWheeze,
    BreathingCoarseCrepitationsAtBase,
    BreathingPneumothoraxSymptoms,
    VentilationResistance,
    RadialPulsePalpable,
    RadialPulseNonPalpable,
    HeartSoundsMuffled,
    HeartSoundsNormal,
    AVPU_A,
    AVPU_U,
    AVPU_V,
    PupilsPinpoint,
    PupilsNormal,
    ExposureRash,
    ExposurePeripherallyShutdown,
    ExposureStainedUnderwear,
    HeartRhythm0,
    HeartRhythm1,
    HeartRhythm2,
    HeartRhythm3,
    HeartRhythm4,
    MeasuredHeartRate,
    MeasuredRespRate,
    MeasuredCapillaryGlucose,
    MeasuredTemperature,
    MeasuredMAP,
    MeasuredSats,
    MeasuredResps
}

public struct Measurement {
    public readonly Insights Measurable;
    public readonly float Value;

    public static readonly ReadOnlyCollection<Insights> MeasurableInsights = Array.AsReadOnly(new Insights[] {
        Insights.MeasuredHeartRate,
        Insights.MeasuredRespRate,
        Insights.MeasuredCapillaryGlucose,
        Insights.MeasuredTemperature, 
        Insights.MeasuredMAP,
        Insights.MeasuredSats,
        Insights.MeasuredResps
    });

    public Measurement(Insights measurable, float value) {
        this.Measurable = measurable;
        this.Value = value;
    }
}