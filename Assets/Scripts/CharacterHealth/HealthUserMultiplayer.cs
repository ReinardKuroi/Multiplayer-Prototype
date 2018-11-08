using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IManager;

using Photon.Pun;

namespace CharacterHealth {
	[RequireComponent(typeof(PhotonView))]
	public class HealthUserMultiplayer : HealthUser {
		public PhotonView photonView;

		protected override void Awake () {
			base.Awake ();
			photonView = GetComponent<PhotonView> ();
		}

		public override int TakeDamage (Damage damage) {
			photonView.RPC ("OnDamage", RpcTarget.Others, damage.Amount, damage.Type);
			return base.TakeDamage (damage);
		}

		[PunRPC]
		void OnDamage (int i, string s) {
			Damage damage = new Damage (i, s);
			base.TakeDamage (damage);
		}

		public override void EquipPlatedArmor ()
		{
			photonView.RPC ("OnPlatedArmor", RpcTarget.All);
		}

		[PunRPC]
		void OnPlatedArmor () {
			base.EquipPlatedArmor ();
		}

		public override void EquipMageRobes ()
		{
			photonView.RPC ("OnMageRobes", RpcTarget.All);
		}

		[PunRPC]
		void OnMageRobes () {
			base.EquipMageRobes ();
		}

		public override void UnequipArmor ()
		{
			photonView.RPC ("OnNoArmor", RpcTarget.All);
		}

		[PunRPC]
		void OnNoArmor () {
			base.UnequipArmor ();
		}

		public override void Die () {
			if (photonView.InstantiationId == 0) {
				base.Die ();
			} else if (photonView.IsMine) {
				Debug.LogFormat ("<color=pink>{0} has commit die~</color>", name);
				PhotonNetwork.Destroy (this.gameObject);
			}
		}
	}
}