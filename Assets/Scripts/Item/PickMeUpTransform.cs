using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class PickMeUpTransform : MonoBehaviourPunCallbacks {

	public enum State {
		Free,
		Pickd
	}

	public State state = State.Free;

	public Transform Interactor { get; private set; }

	private Rigidbody rb;
	private Transform carryLocation;

	public bool PickUp (Transform t) {
		if (state == State.Pickd)
			return false;

		photonView.RequestOwnership ();
		carryLocation = t;
		transform.position = carryLocation.position;
		Interactor = carryLocation.parent;
		transform.parent = Interactor;
		photonView.RPC ("NetPickUp", RpcTarget.All);
		return true;
	}

	public void PutDown (float force) {
		if (state == State.Free)
			return;
		
		transform.parent = null;
		photonView.RPC ("NetPutDown", RpcTarget.All, force);
		rb = GetComponent<Rigidbody> ();
		if (force > 0 && rb != null)
			rb.AddForce (Interactor.transform.forward * force);
	}

	[PunRPC]
	void NetPickUp () {
		rb = GetComponent<Rigidbody> ();
		if (rb != null)
			rb.isKinematic = true;
		state = State.Pickd;
	}

	[PunRPC]
	void NetPutDown (float force) {
		rb = GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.isKinematic = false;
		}
		state = State.Free;
	}
}