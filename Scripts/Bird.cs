using Godot;
using System;

public partial class Bird : CharacterBody2D
{
	[Export] private Balloon target;
	[Export] private float speed = 100;
	[Export] private float accel = 200;

	[Export] private float stopDist = 100;
	private bool inAttackArea = false;
	[Export] private float attackTime = 1500;
	private float lastAttack = 0;

	public override void _PhysicsProcess(double delta)
	{
		if (!IsInstanceValid(target)) {
			long r = GD.Randi() % GameManager.instance.balloons.Length;
			target = GameManager.instance.balloons[r];
			return;
		}

		float now = Time.GetTicksMsec();
		inAttackArea = GlobalPosition.DistanceTo(target.GlobalPosition) <= stopDist;

		// Moving
		Vector2 direction;
		if (!inAttackArea) {
			direction = (target.GlobalPosition - GlobalPosition).Normalized();
			lastAttack = now; // so it cant attack instantly when it enters area
		} else {
			direction = Vector2.Zero;
		}

		Vector2 goalVel = direction * speed;

		Velocity = Velocity.MoveToward(goalVel, accel * (float)delta);
		MoveAndSlide();

		// Attacking
		if (!inAttackArea) return;

		if (now - lastAttack >= attackTime) {
			--target.health;
			lastAttack = now;
		}
	}
}
