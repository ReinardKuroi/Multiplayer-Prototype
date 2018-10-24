using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterMotor {
	public class Motor {
		protected PlayerController PController { get; set; }
		protected CharacterController Controller { get; set; }
		protected Transform Facing { get; set; }
		protected float Speed { get { return 5f; } }
		protected float JumpHeight { get { return 3f; } }

		protected float Gravity { get { return Physics.gravity.y; } }
		protected float VelocityY { get; set; }

		public Motor (PlayerController p) {
			PController = p;
			Controller = p.GetComponent<CharacterController> ();
			Facing = p.GetComponent<Transform> ();
			VelocityY = 0f;
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
			
			ExitState ();
			PController.LastPosition = this;
			PController.CharacterPosition = state;
			PController.CharacterPosition.EnterState ();
		}

		public virtual void Jump () {}
		public virtual void Blink () {}

		public virtual void ExitState () {
			Debug.LogFormat ("Exited state {0}", this);
		}

		public virtual void EnterState () {
			Debug.LogFormat ("Entered state {0}", this);
		}

		public static implicit operator bool (Motor me) {
			return me != null;
		}
	}
	/// <summary>
	/// Falling state. Transitions to a DoubleJump, Dash, or Ground.
	/// </summary>
	public class Falling : Motor {
		public Falling (PlayerController p) : base (p) {}

		public override void Fall () {
			VelocityY += Gravity * Time.deltaTime;
			base.Fall ();
		}
			
		public override void Jump ()
		{
			SetState (new DoubleJumping (PController));
		}
			
		public override void Blink ()
		{
			SetState (new Blinking (PController));
		}

		public override void Transition (CheckFunction f) {
			if (f ())
				SetState (new Walking (PController));
		}
	}
	/// <summary>
	/// Jumping state. Transitions to Falling, Double Jump, Dash, or Ground.
	/// </summary>
	public class Jumping : Falling {
		public Jumping (PlayerController p) : base (p) {}

		public override void EnterState ()
		{
			VelocityY = Mathf.Sqrt (JumpHeight * -2f * Gravity);
			base.EnterState ();
		}

		public override void Transition (CheckFunction f)
		{	
			if (VelocityY <= 0)
				SetState (new Falling(PController));
			base.Transition (f);
		}
	}
	/// <summary>
	/// Falling after a Double Jump. Transitions to Ground.
	/// </summary>
	public class FallingAfterDoubleJump : Falling {
		public FallingAfterDoubleJump (PlayerController p) : base(p) {}

		public override void Jump () {}
	}
	/// <summary>
	/// Double Jump. Transitions to Falling after a Double Jump, or Ground.
	/// </summary>
	public class DoubleJumping : Jumping {
		public DoubleJumping (PlayerController p) : base (p) {}

		public override void Jump () {}

		public override void Transition (CheckFunction f)
		{
			if (VelocityY <= 0)
				SetState (new FallingAfterDoubleJump (PController));
			if (f ())
				SetState (new Walking (PController));
		}
	}

	public class Blinking : Motor {
		public Blinking (PlayerController p) : base (p) {}

		float Drag { get { return -4f; } }
		float Impulse { get { return 4f; } }
		float ExtraSpeed { get; set; }

		public override void Walk (Vector2 input)
		{
			Controller.Move (Facing.forward * ExtraSpeed);
			ExtraSpeed += Drag;
			base.Walk (input);
		}

		public override void EnterState ()
		{
			ExtraSpeed = Impulse;
			base.EnterState ();
		}

		public override void Transition (CheckFunction f) {
			if ((ExtraSpeed < 0)){
				if (PController.LastPosition is DoubleJumping || PController.LastPosition is FallingAfterDoubleJump)
					SetState (new FallingAfterDoubleJump(PController));
				else
					SetState (new Falling (PController));
			}
			base.Transition (f);
		}
	}

	public class Walking : Motor {
		public Walking (PlayerController p) : base (p) {}

		public override void Fall ()
		{
			if (VelocityY < 0f)
				VelocityY = 0f;
			base.Fall ();
		}

		public override void Jump ()
		{
			SetState (new Jumping (PController));
		}

		public override void Blink ()
		{
			SetState (new Blinking (PController));
		}

		public override void Transition (CheckFunction f) {
			if (!f ())
				SetState (new Falling (PController));
		}
	}
}