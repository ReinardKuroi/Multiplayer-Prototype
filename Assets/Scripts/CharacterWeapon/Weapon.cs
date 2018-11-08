using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterHealth;

namespace CharacterWeapon {
	public class Weapon {
		public Damage WeaponDamage { get; protected set; }

		public Weapon (int i = 0, string t = "Physical") {
			WeaponDamage = new Damage (i, t);
		}

		public virtual void Attack (GetTarget f) {
			IHealthUser target = f ();
			if (target == null)
				return;

			target.TakeDamage (WeaponDamage);
		}

		public virtual void Attack (IHealthUser target) {
			if (target == null)
				return;

			target.TakeDamage (WeaponDamage);
		}
	}

	public delegate IHealthUser GetTarget ();
}