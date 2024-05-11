using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private readonly float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	[Export] private float speed =  500;
	[Export] private float accel = 1500;

	[Export] private float jumpForce;
	[Export] private float jumpHold;
	[Export] private int jumpsMax = 2;
	private int jumpsLeft;
	private float jumpStart = 0;
	private bool jumping = false;

	public override void _PhysicsProcess(double delta)
	{
		float now = Time.GetTicksMsec();

		// Horizontal Movement
		float direction = Input.GetAxis("left", "right");
		float goalVel = direction * speed;

		float maxMove = accel;
		if (Mathf.Sign(direction) != 0 && Mathf.Sign(direction) != Mathf.Sign(Velocity.X)) {
			maxMove = accel * 2; // "braking" faster if moving against current velocity
		}

		float xMove = Mathf.MoveToward(Velocity.X, goalVel, maxMove * (float)delta);
		// ---

		// Vertical Movement
		float yMove = 0;
		if (!IsOnFloor()) {
			yMove = Velocity.Y + gravity * (float)delta;
		} else {
			jumpsLeft = jumpsMax;
		}

		if (jumping && !Input.IsActionPressed("jump")) jumping = false;

		if (jumpsLeft > 0 && Input.IsActionJustPressed("jump")) {
			jumping = true;
			jumpStart = now;
			--jumpsLeft;
		}

		if (Input.IsActionPressed("jump") && jumping) {
			yMove = jumpForce;

			if (!jumping) {
				jumping = true;
				jumpStart = now;
			} else if (now - jumpStart > jumpHold) {
				jumping = false;
			}
		}
		// ---

		Velocity = new Vector2(xMove, yMove);
		MoveAndSlide();
	}
}
