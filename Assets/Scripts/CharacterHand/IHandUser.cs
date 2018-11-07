using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHand {
	public interface IHandUser {
		Hand CharacterHolding { get; set; }

		GameObject gameObject { get; }
	}
}