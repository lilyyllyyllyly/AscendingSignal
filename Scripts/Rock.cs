using Godot;
using System;

public partial class Rock : Node2D
{
	[Export] private float speed;
	[Export] private float lifetime;

	public override void _Process(double delta)
	{
		lifetime -= (float)delta;
		if (lifetime <= 0) {
			QueueFree();
			return;
		}

		Position = new Vector2(Position.X, Position.Y + speed * (float)delta);
	}
}
