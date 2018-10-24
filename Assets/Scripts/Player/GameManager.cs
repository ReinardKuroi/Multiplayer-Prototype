using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks {

	[Tooltip("Player prefab")]
	public GameObject playerPrefab;

	private Spawner playerSpawner;

	void Start () {
		playerSpawner = GetComponent<Spawner> ();
		if (playerSpawner == null)
			playerSpawner = gameObject.AddComponent<Spawner> ();
		playerSpawner.Prefab = playerPrefab;
		playerSpawner.Position = new Vector3 (0f, 2f, 0f);

		if (playerPrefab == null)
			Debug.LogError ("Missing player prefab reference");
		else {
			if (PlayerController.LocalPlayerInstance == null) {
				playerSpawner.Spawn ();
			} else {
				Debug.LogFormat ("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
			}
		}
	}

	public override void OnLeftRoom () {
		SceneManager.LoadScene (0);
	}

	public void LeaveRoom () {
		PhotonNetwork.LeaveRoom ();
	}

	void LoadArena () {
		if (!PhotonNetwork.IsMasterClient) {
			Debug.LogError ("Trying to load a level but we are not the master client");
		}
		Debug.LogFormat ("Loading level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
		PhotonNetwork.LoadLevel ("Room loaded");
	}

	public override void OnPlayerEnteredRoom (Player other) {
		Debug.LogFormat ("Player {0} connected", other.NickName);

		if (PhotonNetwork.IsMasterClient) {
			Debug.LogFormat ("MasterClient {0}", PhotonNetwork.IsMasterClient);

//			LoadArena ();
		}
	}

	public override void OnPlayerLeftRoom (Player other) {
		Debug.LogFormat ("Player {0} left room", other.NickName);

		if (PhotonNetwork.IsMasterClient) {
			Debug.LogFormat ("MasterClient {0}", PhotonNetwork.IsMasterClient);
//			LoadArena ();
		}
	}
}