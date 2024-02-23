using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;
using System.Text;
using System;

public class MemoChannel : SideChannel, IObserver<string> {
	public MemoChannel(Guid channelId) {
        this.channelId = channelId;
    }

    public void OnNext(string memo) {
        using (var msgOut = new OutgoingMessage())
        {
            msgOut.WriteString(stringToSend);
            QueueMessageToSend(memo);
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

    public void OnCompleted()
    {
    }
}
