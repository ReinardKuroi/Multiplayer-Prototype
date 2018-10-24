using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks {

	[Tooltip("The UI Panel to let the player enter their name and press Play")]
	[SerializeField]
	private GameObject controlPanel;

	[Tooltip("The UI Label to inform the player that they are connecting")]
	[SerializeField]
	private GameObject progressLabel;

	[Tooltip("THe maximum amount of players per room")]
	[SerializeField]
	private byte maxPlayersPerRoom = 4;

	private bool isConnecting;

	/// <summary>
	/// The game version.
	/// </summary>
	string gameVersion = "1.0";

	void Awake () {
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	void Start () {
		progressLabel.SetActive (false);
		controlPanel.SetActive (true);
	}

	public void Connect() {
		isConnecting = true;

		progressLabel.SetActive (true);
		controlPanel.SetActive (false);

		if (PhotonNetwork.IsConnected) {
			PhotonNetwork.JoinRandomRoom ();
		} else {
			PhotonNetwork.GameVersion = gameVersion;
			PhotonNetwork.ConnectUsingSettings ();
		}
	}
	/// <summary>
	/// Called when the client is connected to the Master Server and ready for matchmaking and other tasks.
	/// </summary>
	/// <remarks>The list of available rooms won't become available unless you join a lobby via LoadBalancingClient.OpJoinLobby.
	/// You can join rooms and create them even without being in a lobby. The default lobby is used in that case.</remarks>
	public override void OnConnectedToMaster () {
		Debug.Log ("OnConnecteedToMasters() was called");
		if (isConnecting)
			PhotonNetwork.JoinRandomRoom ();
	}
	/// <summary>
	/// Called after disconnecting from the Photon server. It could be a failure or an explicit disconnect call
	/// </summary>
	/// <remarks>The reason for this disconnect is provided as DisconnectCause.</remarks>
	/// <param name="cause">Cause.</param>
	public override void OnDisconnected (DisconnectCause cause) {
		progressLabel.SetActive (false);
		controlPanel.SetActive (true);
		Debug.LogWarningFormat ("OnDisconnected() was called, reason: {0}", cause);
	}
		
	public override void OnJoinRandomFailed (short returnCode, string message) {
		Debug.Log ("No random room available, creating room...");
		PhotonNetwork.CreateRoom (null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
	}

	public override void OnJoinedRoom () {
		Debug.Log ("Joined room");
		PhotonNetwork.LoadLevel ("RoomWithCubes");
	}

	public void Exit () {
		Application.Quit ();
	}
}