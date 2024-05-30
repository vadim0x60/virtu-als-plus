using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Networker : NetworkBehaviour {
	public ALine aLineScript;
	public Resp respScript;
	public Sats satsScript;
	public Control controller;
		
	// Use this for initialization
	void Start () {
		controller.remoteConnected = true;
	}

	// Update is called once per frame
	void Update () {
		if (controller.remoteConnected != NetworkClient.active) {
			controller.remoteConnected = NetworkClient.active;
		}
	}

	[Command]
	public void CmdChangeRhythm(Insights rhythm) {
		RpcChangeRhythm (rhythm);
	}

	[Command]
	public void CmdChangeHR (float value) {
		RpcChangeHR (value);
	}

	[Command]
	public void CmdChangeBP (float value) {
		RpcChangeBP (value);
	}

	[Command]
	public void CmdChangeResps (float value) {
		RpcChangeResps (value);
	}

	[Command]
	public void CmdChangeSats (float value) {
		RpcChangeSats (value);
	}

	[ClientRpc]
	public void RpcChangeRhythm(Insights rhythm) {
		Debug.Log ("Network rhythm: " + rhythm);
		controller.RemoteChangeECG (rhythm);
	}

	[ClientRpc]
	public void RpcChangeHR (float value) {
		controller.RemoteChangeHeartRate (value);
	}

	[ClientRpc]
	public void RpcChangeBP (float value) {
		aLineScript.ClientChangeBP (value);
	}

	[ClientRpc]
	public void RpcChangeResps (float value) {
		respScript.ClientChangeResps (value);
	}

	[ClientRpc]
	public void RpcChangeSats (float value) {
		satsScript.ClientChangeSats (value);
	}
}
