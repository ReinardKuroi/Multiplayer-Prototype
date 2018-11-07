using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {

	public interface IHealthUser {
		ICharacterHealth CharacterHP { get; }
		ICharacterArmor CharacterArmor { get; }

		void Die ();
	}

	public interface ICharacterHealth {
		IHealthUser PCharacter { get; }
		int HP { get; }
		int MaxHP { get; }

		int TakeDamage (IDamage damage);
		int Heal (int heal);
	}

	public class Health : ICharacterHealth {
		protected readonly int maxHP;
		public IHealthUser PCharacter { get; protected set; }
		public int HP { get; protected set; }
		public int MaxHP { get { return maxHP; } }

		public Health (IHealthUser user, int _maxHP = 100) {
			PCharacter = user;
			maxHP = _maxHP;
			HP = maxHP;
		}

		public int TakeDamage (IDamage damage) {
			int reduction = 0;
			int damagetaken = 0;
			foreach (var kvp in damage.Damages) {
				if (PCharacter.CharacterArmor.DamageReductionPercentage.TryGetValue (kvp.Key, out reduction))
					damagetaken = kvp.Value * (100 - reduction) / 100;
				else
					damagetaken = kvp.Value;
				HP -= damagetaken;
				Debug.LogFormat ("Character <b><color=blue>{0}</color></b> took <color=red>{1}</color> damage of type <color=brown>{2}</color>", PCharacter, damagetaken, kvp.Key);
			}
			if (HP <= 0) {
				HP = 0;
				PCharacter.Die ();
			}
			return HP;
		}

		public int Heal (int heal) {
			heal = (HP + heal > MaxHP) ? MaxHP - HP : heal;
			HP += heal;
			Debug.LogFormat ("Character <b><color=blue>{0}</color></b> was healed for <color=green>{1}</color>", PCharacter, heal);
			return heal;
		}
	} 

	public class Armor : ICharacterArmor {
		Dictionary<DamageType, int> derp = new Dictionary<DamageType, int> ();
		public Dictionary<DamageType, int> DamageReductionPercentage { get { return derp; } }

		public Armor () {
		}

		public Armor (DamageType t, int i) {
			derp.Add (t, i);
		}

		public Armor (Dictionary<DamageType, int> _derp) {
			foreach (var v in _derp)
				derp.Add (v.Key, v.Value);
		}
	}

	public class Damage : IDamage {
		Dictionary<DamageType, int> damages = new Dictionary<DamageType, int> ();
		public Dictionary<DamageType, int> Damages { get { return damages; } }

		public Damage (DamageType t, int i) {
			damages.Add (t, i);
		}

		public Damage (Dictionary<DamageType, int> _damages) {
			foreach (var v in _damages)
				damages.Add (v.Key, v.Value);
		}
	}

	public interface IDamage {
		Dictionary<DamageType, int> Damages { get; }
	}

	public interface ICharacterArmor {
		Dictionary<DamageType, int> DamageReductionPercentage { get; }
	}

	public enum DamageType {
		Physical,
		Holy,
		Dark,
		Arcane,
		Ligtning,
		Fire
	}
}