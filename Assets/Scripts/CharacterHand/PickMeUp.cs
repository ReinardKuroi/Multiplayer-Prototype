using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class PickMeUp : MonoBehaviourPunCallbacks {

	private enum State {
		Free,
		Pickd
	}

	public float response = 20f;
	public float offset = 2f;

	private State state;
	private Transform Interactor;
	private Transform Hand;
	private Rigidbody rBody;
	private float oldMass;

	void Start () {
		state = State.Free;
		rBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (state == State.Pickd && photonView.IsMine) {
			rBody.MovePosition (Hand.position);
			rBody.MoveRotation (Quaternion.Euler (Hand.rotation.eulerAngles));
		}
	}

	public bool PickUp (Transform t) {
		if (state == State.Pickd)
			return false;
		
		Interactor = t;

		GameObject g = new GameObject ("Hand");
		Hand = g.GetComponent<Transform> ();
		Hand.parent = Interactor;
		Hand.position = Interactor.position + Interactor.forward * offset;
		Hand.rotation = Interactor.rotation;

		photonView.RequestOwnership ();
		oldMass = rBody.mass;
		rBody.mass = 0f;
		rBody.velocity = Vector3.zero;
		rBody.angularVelocity = Vector3.zero;
		photonView.RPC ("SyncState", RpcTarget.All, State.Pickd, false);

		return true;
	}

	public bool PutDown (float force) {
		if (state == State.Free)
			return false;
		
		Destroy (Hand.gameObject);

		photonView.RequestOwnership ();
		rBody.mass = oldMass;
		rBody.velocity = Vector3.zero;
		rBody.angularVelocity = Vector3.zero;
		if (force > 0)
			rBody.AddForce (Interactor.forward * force);
		photonView.RPC ("SyncState", RpcTarget.All, State.Free, true);

		return true;
	}

	public bool PutDown () {
		if (state == State.Free)
			return false;

		photonView.RequestOwnership ();
		rBody.mass = oldMass;
		rBody.velocity = Vector3.zero;
		rBody.angularVelocity = Vector3.zero;
		photonView.RPC ("SyncState", RpcTarget.All, State.Free, true);

		return true;
	}

	[PunRPC]
	void SyncState (State _state, bool gravity) {
		state = _state;
		rBody.useGravity = gravity;
	}
}