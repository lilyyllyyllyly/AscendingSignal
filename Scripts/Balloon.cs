using Godot;
using System;

public partial class Balloon : Node2D
{
	[Export] private float maxHealth = 10;
	public float health;

	[Export] private AnimationPlayer anim;
	[Export] private AnimationPlayer fireAnim;

	public override void _Ready()
	{
		health = maxHealth;

		anim.Play("wobble");
		anim.Seek((GD.Randi() % 10)/10.0);

		fireAnim.Play("fire");
		fireAnim.Seek((GD.Randi() % 3)/10.0);
	}

	public override void _Process(double delta)
	{
		if (health <= 0) {
			GetNode("..").QueueFree();
		}
	}
}
