using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public class Armor : IArmor {
		Dictionary<string, int> derp = new Dictionary<string, int> ();
		public Dictionary<string, int> DamageReductionPercentage { get { return derp; } }

		public Armor (int i = 0, string t = "Physical") {
			derp.Add (t, i);
		}

		public Armor (Dictionary<string, int> _derp) {
			foreach (var v in _derp)
				derp.Add (v.Key, v.Value);
		}
	}
}