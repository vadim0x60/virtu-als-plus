using System;

public enum Feedback {
    Blunder,
    Success,
    Failure,
    Tick
}

[Serializable]
public class RewardCenter : IObserver<Feedback> {
    public float FailureReward;
    public float SuccessReward;
    public float BlunderReward;
    public float TimestepReward;

    public Action<float> AddReward;

    public bool Done = false;

    public void OnNext(Feedback feedback) {
        switch(feedback) {
            case Feedback.Success:
                AddReward(SuccessReward);
                Debug.Log("Success is a terminal insight indicating episode end");
                Done = true;
                break;
            case Feedback.Failure:
                AddReward(FailureReward);
                Debug.Log("Failure is a terminal insight indicating episode end");
                Done = true;
                break;
            case Feedback.Blunder:
                AddReward(BlunderReward);
                break;
            case Feedback.Tick:
                AddReward(TimestepReward);
                break;
        }
    }

    public void OnError(Exception e) 
    {
        throw e;
    }

    public void OnCompleted()
    {
    }
}