using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterHealth;

namespace CharacterWeapon {
	public class Melee : MonoBehaviour {
		GameObject owner;
		Weapon knife;
		Animation attackAnimation;

		void Awake () {
			knife = new Weapon (30);
			attackAnimation = GetComponent<Animation> ();
		}

		public void Attack () {
			if (!attackAnimation.isPlaying)
				attackAnimation.Play ("SwordMeleeAttack");
		}

		void OnTriggerEnter (Collider other) {
			knife.Attack (() => { return other.gameObject.GetComponent<IHealthUser> (); });
		}
	}
}