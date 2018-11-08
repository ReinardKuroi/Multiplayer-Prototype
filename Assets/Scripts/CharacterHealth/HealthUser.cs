using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IManager;

using Photon.Pun;

namespace CharacterHealth {
	public class HealthUser : MonoBehaviour, IHealthUser {
		public Health CharacterHP { get; private set; }
		public Armor CharacterArmor { get; set; }

		protected static Armor platedArmor;
		protected static Armor noArmor;
		protected static Armor mageRobes;

		InputManager iManager;

		protected virtual void Awake () {
			CharacterHP = new Health (this);
			CharacterArmor = new Armor (this);
			iManager = InputManager.Instance;
			iManager.AddCommand (KeyCode.Alpha1, UnequipArmor);
			iManager.AddCommand (KeyCode.Alpha2, EquipMageRobes);
			iManager.AddCommand (KeyCode.Alpha3, EquipMageRobes);
			platedArmor = new Armor (this, 40);
			noArmor = new Armor (this);
			mageRobes = new Armor (this, new Dictionary<string, int> () {
				{"Physical", 10},
				{"Arcane", 70}
			});
		}

		public virtual int TakeDamage (Damage damage) {
			return CharacterHP.TakeDamage (damage);
		}

		public virtual void EquipPlatedArmor () {
			CharacterArmor.Equip (platedArmor);
		}

		public virtual void EquipMageRobes () {
			CharacterArmor.Equip (mageRobes);
		}

		public virtual void UnequipArmor () {
			CharacterArmor.Equip (noArmor);
		}

		public virtual void Die () {
			Debug.LogFormat ("<color=pink>{0} has commit die~</color>", name);
			Destroy (this.gameObject);
		}
	}
}