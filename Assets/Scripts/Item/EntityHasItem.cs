using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using CharacterInventory;

public class EntityHasItem : MonoBehaviour {
	public string ItemOnPickup;

	void Awake () {
		transform.tag = "EHI";
	}

	public IPrototypeItem OnInteract () {
		StartCoroutine ("DestroyObject");
		IPrototypeItem item = DataManagement.Instance.GetItem (ItemOnPickup);
		Debug.LogFormat ("Got item {0}", item.Name);
		return item;
	}

	IEnumerator DestroyObject () {
		yield return new WaitForEndOfFrame ();
		PhotonNetwork.Destroy (gameObject);
	} 
}