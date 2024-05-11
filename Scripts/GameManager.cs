using Godot;
using System;

[GlobalClass]
public partial class GameManager : Node
{
	public static GameManager instance;

	[Export] public Balloon[] balloons;

	[Export] private PackedScene bird;
	[Export] private Rect2 birdSpawnArea;
	[Export] private float birdSpawnDelay;
	private float lastBirdSpawn;

	private RandomNumberGenerator rng;

	public override void _Ready()
	{
		if (GameManager.instance == null) {
			GameManager.instance = this;
		} else {
			QueueFree();
		}

		rng = new RandomNumberGenerator();
	}

	public override void _Process(double delta)
	{
		float now = Time.GetTicksMsec();
		if (now - lastBirdSpawn >= birdSpawnDelay) {
			Node2D newBird = bird.Instantiate().GetNode<Node2D>(".");
			GetTree().CurrentScene.AddChild(newBird);

			float posX = rng.RandfRange(birdSpawnArea.Position.X, birdSpawnArea.Position.X + birdSpawnArea.Size.X);
			float posY = rng.RandfRange(birdSpawnArea.Position.Y, birdSpawnArea.Position.Y + birdSpawnArea.Size.Y);
			newBird.GlobalPosition = new Vector2(posX, posY);

			lastBirdSpawn = now;
		}
	}
}
