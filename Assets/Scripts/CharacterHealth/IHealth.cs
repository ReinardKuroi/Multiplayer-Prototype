using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public interface IHealth {
		IHealthUser PCharacter { get; }
		int HP { get; }
		int MaxHP { get; }

		int TakeDamage (IDamage damage);
		int Heal (int heal);
	}
}