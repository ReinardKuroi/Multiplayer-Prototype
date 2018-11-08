using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public interface IHealthUser {
		Health CharacterHP { get; }
		Armor CharacterArmor { get; set; }

		int TakeDamage (Damage damage);
		void Die ();
	}
}