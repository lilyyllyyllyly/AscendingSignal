using Godot;
using System;

public partial class Gun : Node2D
{
	[Export] private PackedScene projectile;
	[Export] private Node2D spawnPoint;

	[Export] private float shootDelay;
	private float lastShoot = 0;

	public override void _Process(double delta)
	{
		float now = Time.GetTicksMsec();
		if (now - lastShoot >= shootDelay && Input.IsActionPressed("fire")) {
			lastShoot = now;

			Node2D newProj = projectile.Instantiate().GetNode<Node2D>(".");
			GetTree().CurrentScene.AddChild(newProj);
			newProj.GlobalPosition = spawnPoint.GlobalPosition;
			newProj.Rotation = Rotation;
		}

		Vector2 diff = GetGlobalMousePosition() - GlobalPosition;
		float rotZ = Mathf.Atan2(diff.Y, diff.X) + Mathf.Pi/2;
		Rotation = rotZ;
	}
}
