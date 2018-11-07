using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterMotor {
	public interface IMotorUser {
		Motor CharacterPosition { get; set; }
		Motor LastPosition { get; set; }

		GameObject gameObject { get; }

		MotorPrefs MPrefs { get; }
	}
}