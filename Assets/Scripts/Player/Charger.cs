using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Charger : MonoBehaviour {
		public int Charges { get; private set; }
		int MaxCharges { get { return 3; } }
		float RechargeTime { get { return 1.83f; } }
		bool Recharging {get { return Charges < MaxCharges; }}
		public float Progress { get { return (Time.time - lastTime)/RechargeTime; } }

		float lastTime;
		Slider DSlider;
		Text DText;

	void Awake () {
		Charges = MaxCharges;
		lastTime = Time.time - RechargeTime;
		GameObject DashCooldown = GameObject.FindGameObjectWithTag ("DashCooldownUI");
		DSlider = DashCooldown.transform.Find ("Slider").GetComponent<Slider> ();
		DText = DashCooldown.transform.Find ("Charges").GetComponent<Text> ();
	}

	public void Update () {
		if (Recharging && Progress >= 1f) {
			Charges++;
			if (Recharging)
				lastTime = Time.time;
		}
		DSlider.value = Progress;
		DText.text = Charges.ToString ();
	}

	public void Use () {
		if (!Recharging)
			lastTime = Time.time;
		Charges--;
	}
}