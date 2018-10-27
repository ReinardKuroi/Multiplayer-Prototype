using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterHealth;
using IManager;

using UnityEngine.UI;

public class HPTest : MonoBehaviour, IHealthUser {
	public ICharacterHealth CharacterHP { get; private set; }
	public ICharacterArmor CharacterArmor { get; private set; }

	InputManager iManager;
	Armor HeavyArmor;
	Armor MageRobes;

	public Text health;

	void Awake () {
		iManager = InputManager.Instance;

		CharacterHP = new Health (this);
		CharacterArmor = new Armor ();

		HeavyArmor = new Armor (DamageType.Physical, 40);
		MageRobes = new Armor (new Dictionary<DamageType, int> () {
			{DamageType.Arcane, 20},
			{DamageType.Ligtning, 20},
			{DamageType.Dark, 10}
		});

		iManager.AddCommand (KeyCode.A, () => {
			CharacterHP.TakeDamage (new Damage (DamageType.Physical, 15));
		});
		iManager.AddCommand (KeyCode.H, () => {
			CharacterHP.Heal (20);
		});
		iManager.AddCommand (KeyCode.M, () => {
			CharacterHP.TakeDamage (new Damage (new Dictionary<DamageType, int> () {
				{DamageType.Physical, 5},
				{DamageType.Arcane, 5},
				{DamageType.Ligtning, 5}
			}));});
		iManager.AddCommand (KeyCode.Alpha1, () => {
			Debug.Log ("Equipped Heavy Armor");
			CharacterArmor = HeavyArmor;
		});
		iManager.AddCommand (KeyCode.Alpha2, () => {
			Debug.Log ("Equipped Mage Robes");
			CharacterArmor = MageRobes;
		});
		iManager.AddCommand (KeyCode.Alpha3, () => {
			Debug.Log ("Unequipped armor");
			CharacterArmor = new Armor();
		});
	}

	void Update () {
		iManager.HandleInput ();
		health.text = string.Format ("{0}/{1}", CharacterHP.HP, CharacterHP.MaxHP);
	}

	public void Die () {
		Debug.Log ("Character died");
		CharacterHP.Heal (100);
	}
}