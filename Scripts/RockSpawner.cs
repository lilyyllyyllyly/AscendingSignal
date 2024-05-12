using Godot;
using System;

public partial class RockSpawner : Node2D
{
	[Export] private PackedScene[] rocks;
	[Export] private float distMin = 100f;
	[Export] private float distMax = 120f;

	public override void _Ready()
	{
		Node2D newRock = (Node2D)rocks[Random.Shared.Next(0, rocks.Length)].Instantiate();
		newRock.GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y - (Random.Shared.NextSingle() * (distMax - distMin) + distMin));
		GetTree().CurrentScene.AddChild(newRock);
	}
}
