using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Spawner : MonoBehaviour {
	
	public GameObject Prefab;
	public Vector3 Position;

	public virtual void Spawn () {
		PhotonNetwork.Instantiate (Prefab.name, Position, Quaternion.identity, 0);
	}
}