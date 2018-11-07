using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public class Damage : IDamage {
		public string Type { get; protected set; }
		public int Amount { get; protected set; }

		public Damage (int i = 0, string t = "Physical") {
			Type = t;
			Amount = i;
		}
	}
}