using Godot;
using System;

public partial class Projectile : Node2D
{
	[Export] private float speed = 1000;
	[Export] private float lifetime = 3;

	public override void _Process(double delta)
	{
		lifetime -= (float)delta;
		if (lifetime <= 0) {
			QueueFree();
			return;
		}

		Vector2 localUp = new Vector2(Mathf.Cos(Rotation + 3*Mathf.Tau/4), Mathf.Sin(Rotation + 3*Mathf.Tau/4));
		Position += localUp * speed * (float)delta;
	}
}
