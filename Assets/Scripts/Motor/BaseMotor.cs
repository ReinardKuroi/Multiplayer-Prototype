using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterMotor {
	public class Motor {
		protected MotorPrefs prefs;
		public IMotorUser PController { get; protected set; }
		protected CharacterController Controller { get; set; }
		protected Transform Facing { get; set; }
		protected float Speed { get { return prefs.speed; } }
		protected float JumpHeight { get { return prefs.jumpHeight; } }

		protected float Gravity { get { return Physics.gravity.y; } }
		protected float VelocityY { get; set; }

		public Motor (IMotorUser p) {
			PController = p;
			Controller = p.gameObject.GetComponent<CharacterController> ();
			if (Controller == null)
				Controller = p.gameObject.AddComponent<CharacterController> ();
			Facing = p.gameObject.GetComponent<Transform> ();
			if (Facing == null)
				Facing = p.gameObject.AddComponent<Transform> ();
			VelocityY = 0f;
			prefs = PController.MPrefs;
		}

		public virtual void Walk (Vector2 input) {
			Vector3 right = (new Vector3 (Facing.right.x, 0, Facing.right.z)).normalized;
			Vector3 forward = (new Vector3 (Facing.forward.x, 0, Facing.forward.z)).normalized;

			Vector3 direction = right * input.x + forward * input.y;
			Vector3 movement = (direction * Time.deltaTime * Speed);
			Controller.Move (movement);
		}

		public virtual void Fall () {
			Controller.Move (new Vector3 (0f, VelocityY, 0f) * Time.deltaTime);
		}

		public delegate bool CheckFunction ();
		public virtual void Transition (CheckFunction f) {}

		public virtual void SetState (Motor state) {
			if (state.GetType () == this.GetType () || state == null)
				return;

			OnExit ();
			PController.LastPosition = this;
			PController.CharacterPosition = state;
			PController.CharacterPosition.OnEnter ();
		}

		public virtual void Jump () {}
		public virtual void Blink () {}

		public virtual void OnExit () {
			Debug.LogFormat ("Exited state {0}", this);
		}

		public virtual void OnEnter () {
			Debug.LogFormat ("Entered state {0}", this);
		}

		public static implicit operator bool (Motor me) {
			return me != null;
		}
	}
}