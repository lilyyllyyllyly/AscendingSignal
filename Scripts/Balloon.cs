using Godot;
using System;

public partial class Balloon : Node2D
{
	[Export] private float maxHealth = 10;
	public float health;

	public override void _Ready()
	{
		health = maxHealth;
	}

	public override void _Process(double delta)
	{
		if (health <= 0) {
			GetNode("..").QueueFree();
		}
	}
}
