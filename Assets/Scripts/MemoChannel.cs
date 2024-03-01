using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;
using System.Text;
using System;

public class MemoChannel : SideChannel {
	public MemoChannel(Guid channelId) {
        ChannelId = channelId;
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

    public void OnCompleted()
    {
    }
}
