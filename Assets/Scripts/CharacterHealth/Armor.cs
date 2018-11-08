using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public class Armor {
		Dictionary<string, int> derp = new Dictionary<string, int> ();
		public Dictionary<string, int> DamageReductionPercentage { get { return derp; } }

		public IHealthUser PCharacter { get; protected set; }

		public Armor (IHealthUser user, int i = 0, string t = "Physical") {
			PCharacter = user;
			derp.Add (t, i);
		}

		public Armor (IHealthUser user, Dictionary<string, int> _derp) {
			PCharacter = user;
			foreach (var v in _derp)
				derp.Add (v.Key, v.Value);
		}

		public Armor Equip (Armor armor) {
			Armor currentArmor = this;
			Armor newArmor = new Armor (PCharacter, armor.DamageReductionPercentage);
			PCharacter.CharacterArmor = newArmor;
			return currentArmor;
		}
	}
}