using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterHand {
	public class Holding : Hand {
		public Holding (IHandUser p) : base (p) {}

		protected virtual float PickUpDistance { get { return 5f; } }
		protected virtual float Strength { get { return 500f; } }

		public override void Interact ()
		{
			if (InHand != null) {
				InHand.GetComponent<PickMeUp> ().PutDown (0);
				SetState (new EmptyHand (PController));
			}
		}

		public override void AltInteract ()
		{
			if (InHand != null) {
				InHand.GetComponent<PickMeUp> ().PutDown (Strength);
				SetState (new EmptyHand (PController));
			}
		}

		public override void EnterState () {
			RaycastHit hit;
			Physics.Raycast (HandTransform.position, HandTransform.forward, out hit, PickUpDistance);
			InHand = hit.transform;
			if (InHand != null && InHand.gameObject.CompareTag (ItemTag) && InHand.GetComponent<PickMeUp> ().PickUp (HandTransform))
				Debug.LogFormat ("{0} picked up {1}", PController, InHand);
			else
				SetState (new EmptyHand (PController));
			base.EnterState ();
		}

		public override void ExitState ()
		{
			InHand = null;
			base.ExitState ();
		}
	}

	public class Attract : Holding {
		public Attract (IHandUser p) : base (p) {}

		protected override float PickUpDistance { get { return 100f; } }
	}

	public class EmptyHand : Hand {
		public EmptyHand (IHandUser p) : base (p) {}

		public override void Interact ()
		{
			SetState (new Holding (PController));
		}

		public override void AltInteract ()
		{
			SetState (new Attract (PController));
		}
	}
}