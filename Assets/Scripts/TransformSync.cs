using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class TransformSync : MonoBehaviourPunCallbacks, IPunObservable {

	private Vector3 networkPosition;
	private Quaternion networkRotation;

	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.IsWriting && photonView.IsMine) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		} else if (stream.IsReading) {
			networkPosition = (Vector3) stream.ReceiveNext ();
			networkRotation = (Quaternion) stream.ReceiveNext ();
		}
	}

	void Update () {
		if (!photonView.IsMine) {
			transform.position = Vector3.Lerp (transform.position, networkPosition, Time.deltaTime * 20f);
			transform.rotation = Quaternion.Lerp (transform.rotation, networkRotation, Time.deltaTime * 20f);
		}
	}
}