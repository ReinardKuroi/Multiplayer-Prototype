using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand {
	protected PlayerController PController { get; set; }
	protected Transform InHand { get; set; }
	protected Transform HandTransform { get; set; }
	protected string ItemTag { get { return "PickMeUp"; }}

	public Hand (PlayerController p) {
		PController = p;
		HandTransform = p.gameObject.transform;
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

public class Holding : Hand {
	public Holding (PlayerController p) : base (p) {}

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
	public Attract (PlayerController p) : base (p) {}

	protected override float PickUpDistance { get { return 100f; } }
}

public class EmptyHand : Hand {
	public EmptyHand (PlayerController p) : base (p) {}

	public override void Interact ()
	{
		SetState (new Holding (PController));
	}

	public override void AltInteract ()
	{
		SetState (new Attract (PController));
	}
}