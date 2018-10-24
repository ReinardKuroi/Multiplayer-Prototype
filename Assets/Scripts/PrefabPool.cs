using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PrefabPool : MonoBehaviour, IPunPrefabPool {

	private Queue<GameObject> pool;

	public GameObject Prefab;

	public void Awake () {
		pool = new Queue<GameObject> ();

		PhotonNetwork.PrefabPool = this;
	}

	public GameObject Instantiate (string prefabId, Vector3 position, Quaternion rotation) {
		if (pool.Count > 0) {
			GameObject g = pool.Dequeue ();
			g.transform.position = position;
			g.transform.rotation = rotation;
			g.SetActive (true);

			return g;
		}
		return Instantiate (Prefab, position, rotation);
	}

	public void Destroy (GameObject gameObject) {
		gameObject.SetActive (false);

		pool.Enqueue (gameObject);
	}
}