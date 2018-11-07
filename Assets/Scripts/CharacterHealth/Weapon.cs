using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public class Weapon {
		public Damage WeaponDamage { get; protected set; }

		public Weapon (int i = 0, string t = "Physical") {
			WeaponDamage = new Damage (i, t);
		}

		public virtual void Attack (GetTarget f) {
			IHealthUser user = f ();
			if (user == null)
				return;

			user.CharacterHP.TakeDamage (WeaponDamage);
		}
	}

	public delegate IHealthUser GetTarget ();
}