using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterMotor {
	/// <summary>
	/// Falling state. Transitions to a DoubleJump, Dash, or Ground.
	/// </summary>
	public class Falling : Motor {
		public Falling (IMotorUser p) : base (p) {}

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
		public Jumping (IMotorUser p) : base (p) {}

		public override void OnEnter ()
		{
			VelocityY = Mathf.Sqrt (JumpHeight * -2f * Gravity);
			base.OnEnter ();
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
		public FallingAfterDoubleJump (IMotorUser p) : base(p) {}

		public override void Jump () {}
	}
	/// <summary>
	/// Double Jump. Transitions to Falling after a Double Jump, or Ground.
	/// </summary>
	public class DoubleJumping : Jumping {
		public DoubleJumping (IMotorUser p) : base (p) {}

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
		public Blinking (IMotorUser p) : base (p) {}

		float Drag { get { return -4f; } }
		float Impulse { get { return 4f; } }
		float ExtraSpeed { get; set; }

		public override void Walk (Vector2 input)
		{
			Controller.Move (Facing.forward * ExtraSpeed);
			ExtraSpeed += Drag;
			base.Walk (input);
		}

		public override void OnEnter ()
		{
			ExtraSpeed = Impulse;
			base.OnEnter ();
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
		public Walking (IMotorUser p) : base (p) {}

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