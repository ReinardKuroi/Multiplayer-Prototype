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

		public bool Attack () {
			if (!attackAnimation.isPlaying) {
				attackAnimation.Play ("SwordMeleeAttack");
				return true;
			}
			return false;
		}

		void OnTriggerEnter (Collider other) {
			Debug.LogFormat ("<color=red>The blade of the weapon hit a {0}!</color>", other.gameObject.name);
			knife.Attack (() => {
				GameObject g = other.gameObject;
				Debug.LogFormat ("<color=red><b>Trying to attack a {0}</b></color>", g.name);
				return g.GetComponent<IHealthUser> ();;
			});
		}

		void OntriggerExit (Collider other) {
			Debug.LogFormat ("<color=red>The blade of the weapon left the substance of a {0}!</color>", other.gameObject.name);
		}
	}
}