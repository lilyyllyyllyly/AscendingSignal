using Godot;
using System;

[GlobalClass]
public partial class GameManager : Node
{
	[Signal] public delegate void ThirtyMetresTravelledEventHandler();
	[Signal] public delegate void SixtyMetresTravelledEventHandler();

	public static GameManager instance;

	[Export] private Timer spawnTimer;

	[Export] public Balloon[] balloons;
	[Export] public ColorRect[] bars;

	[Export] private PackedScene bird;
	[Export] private Rect2 birdSpawnArea;

	private RandomNumberGenerator rng;

	private int currentBar = 0;

	private int metresTravelled = 0;

	private int birdSpawnChance = 7;

	public override void _Ready()
	{
		if (GameManager.instance == null) {
			GameManager.instance = this;
		} else {
			QueueFree();
		}

		rng = new RandomNumberGenerator();
	}

	private void SetBird() {
		Node2D newBird = (Node2D)bird.Instantiate();
		GetTree().CurrentScene.AddChild(newBird);

		float posX = rng.RandfRange(birdSpawnArea.Position.X, birdSpawnArea.Position.X + birdSpawnArea.Size.X);
		float posY = rng.RandfRange(birdSpawnArea.Position.Y, birdSpawnArea.Position.Y + birdSpawnArea.Size.Y);
		newBird.GlobalPosition = new Vector2(posX, posY);
	}

	private void SpawnBird() {
		// Spawn one bird.
		SetBird();
		
		// 1/birdSpawnChance chance for two birds to spawn.
		// This decrements every 60 metres travelled.
		if (Random.Shared.Next(0, birdSpawnChance) == birdSpawnChance - 1) {
			SetBird();
		}
	}

	private void OnMetreTravelled() {
		metresTravelled += 1;

		if (metresTravelled % 30 == 0) {
			EmitSignal(SignalName.ThirtyMetresTravelled);
			spawnTimer.WaitTime -= 0.2;
		}

		if (metresTravelled % 60 == 0) {
			EmitSignal(SignalName.SixtyMetresTravelled);
	
			bars[currentBar].Color = new Color("34f35f");
			currentBar += 1;
			if (currentBar == bars.Length - 1) {
				// TODO: kill the child.
			}

			// Cap spawn chance at 1/4
			if (birdSpawnChance <= 4) return;
			birdSpawnChance -= 1;
		}
	}
}
