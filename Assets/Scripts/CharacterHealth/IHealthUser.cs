using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public interface IHealthUser {
		IHealth CharacterHP { get; }
		IArmor CharacterArmor { get; }

		void Die ();
	}
}