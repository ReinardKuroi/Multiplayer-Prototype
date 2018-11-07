using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using CharacterInventory;

public class EntityHasItem : MonoBehaviour {
	public string ItemOnPickup;

	public PrototypeItem OnInteract () {
		StartCoroutine ("DestroyObject");
		return DataManagement.Instance.GetItem (ItemOnPickup);
	}

	IEnumerator DestroyObject () {
		yield return new WaitForEndOfFrame ();
		PhotonNetwork.Destroy (gameObject);
	} 
}