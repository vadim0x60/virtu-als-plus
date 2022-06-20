using System;

public interface RewardRecorder {
    void Blunder();
    void Success();
    void Failure();
    void Tick();
}

[Serializable]
public class RewardCenter : RewardRecorder {
    public float FailureReward;
    public float SuccessReward;
    public float BlunderReward;
    public float TimestepReward;

    public Action<float> AddReward;

    public void Blunder() {AddReward(BlunderReward);}
    public void Success() {AddReward(SuccessReward);}
    public void Failure() {AddReward(FailureReward);}
    public void Tick() {AddReward(TimestepReward);}
}