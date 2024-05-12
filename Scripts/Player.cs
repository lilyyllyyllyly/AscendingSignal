using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private readonly float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	[Export] private float speed =  500;
	[Export] private float accel = 1500;

	[Export] private float jumpForce = -400;
	[Export] private float jumpHold  =  100;
	[Export] private int jumpsMax = 2;
	private int jumpsLeft;
	private float jumpStart = 0;
	private bool jumping = false;

	private int gameHeight = 225;
	private int playerSize = 8;

	[Export] private AnimationPlayer anim;

	public override void _PhysicsProcess(double delta)
	{
		float now = Time.GetTicksMsec();

		// Horizontal Movement
		float direction = Input.GetAxis("left", "right");
		if (direction != 0) anim.Play("walk");
		else anim.Play("idle");

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
			anim.Play("jump");
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

		if (Position.Y > gameHeight + playerSize) {
			GetTree().ChangeSceneToFile("res://Scenes/States/Lose.tscn");
		}

		Velocity = new Vector2(xMove, yMove);
		MoveAndSlide();
	}
}
