using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public interface IDamage {
		string Type { get; }
		int Amount { get; }
	}
}