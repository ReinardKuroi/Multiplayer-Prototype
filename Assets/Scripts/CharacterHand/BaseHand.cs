using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHand {
	public class Hand {
		protected IHandUser PController { get; set; }
		protected Transform InHand { get; set; }
		protected Transform HandTransform { get; set; }
		protected string ItemTag { get { return "PickMeUp"; }}

		public Hand (IHandUser p) {
			PController = p;
			HandTransform = p.gameObject.GetComponent<Transform> ();
			if (HandTransform == null)
				HandTransform = p.gameObject.AddComponent<Transform> ();
		}

		public virtual void Interact () {
			Debug.LogFormat ("{0} received Interact command, not implemented", this);
		}

		public virtual void AltInteract () {
			Debug.LogFormat ("{0} received AltInteract command, not implemented", this);
		}

		public virtual void ExitState () {
			Debug.LogFormat ("Exited state {0}", this);
		}

		public virtual void EnterState () {
			Debug.LogFormat ("Entered state {0}", this);
		}

		public static implicit operator bool (Hand me) {
			return me != null;
		}

		public virtual void SetState (Hand state) {
			if (state.GetType () == this.GetType () || !state)
				return;

			ExitState ();
			PController.CharacterHolding = state;
			PController.CharacterHolding.EnterState ();
		}
	}
}