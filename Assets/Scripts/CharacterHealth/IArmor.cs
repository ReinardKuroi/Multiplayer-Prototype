using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public interface IArmor {
		Dictionary<string, int> DamageReductionPercentage { get; }
	}
}