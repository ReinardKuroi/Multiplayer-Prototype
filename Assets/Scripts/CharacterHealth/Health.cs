using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHealth {
	public class Health : IHealth {
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
			if (PCharacter.CharacterArmor.DamageReductionPercentage.TryGetValue (damage.Type, out reduction))
				damagetaken = damage.Amount * (100 - reduction) / 100;
			else
				damagetaken = damage.Amount;
			HP -= damagetaken;
			Debug.LogFormat ("Character <b><color=blue>{0}</color></b> took <color=red>{1}</color> damage of type <color=brown>{2}</color>", PCharacter, damagetaken, damage.Type);
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
}