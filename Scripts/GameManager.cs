using Godot;
using System;

[GlobalClass]
public partial class GameManager : Node
{
	[Signal] public delegate void ThirtyMetresTravelledEventHandler();
	[Signal] public delegate void SixtyMetresTravelledEventHandler();
	[Signal] public delegate void WonEventHandler();

	public static GameManager instance;

	[Export] private Timer spawnTimer;

	[Export] public Balloon[] balloons;
	[Export] public ColorRect[] bars;

	[Export] private PackedScene bird;
	[Export] private Rect2 birdSpawnArea;

	[Export] private Control endGameScreen;

	[Export] private float spawnTimeDecrement;

	[Export] private PackedScene rockSpawner;
	[Export] private Rect2 rockSpawnArea;
	[Export] private float rockSpawnPlayerRange;
	[Export] private Timer rockTimer;
	[Export] private float rockSpawnTimeDecrement;

	[Export] private Node2D player;

	private RandomNumberGenerator rng;

	private int currentBar = 0;

	private int metresTravelled = 0;

	private int birdSpawnChance = 5;

	private bool canEndGame = false;

	public override void _ExitTree()
	{
		if (GameManager.instance == this) GameManager.instance = null;
	}

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

	private void SetRock() {
		Node2D newRock = (Node2D)rockSpawner.Instantiate();

		float posX, posY;
		int r = Random.Shared.Next(0, 3);
		if (r == 0) { // either anywhere on screen
			posX = rng.RandfRange(rockSpawnArea.Position.X, rockSpawnArea.Position.X + rockSpawnArea.Size.X);
			posY = rng.RandfRange(rockSpawnArea.Position.Y, rockSpawnArea.Position.Y + rockSpawnArea.Size.Y);
		} else { // or near the player
			posX = player.GlobalPosition.X + (Random.Shared.NextSingle() - 0.5f) * rockSpawnPlayerRange;
			posY = rng.RandfRange(rockSpawnArea.Position.Y, rockSpawnArea.Position.Y + rockSpawnArea.Size.Y);
		}

		newRock.GlobalPosition = new Vector2(posX, posY);

		GetTree().CurrentScene.AddChild(newRock);
	}

	public void SpawnRock() {
		SetRock();
		if (Random.Shared.Next(0, birdSpawnChance) == birdSpawnChance - 1) { // reusing birdSpawnChance cuz i cant be asked
			SetRock();
		}
	}

	private void WinPause() {
		GetTree().Paused = true;
	}

	private void OnMetreTravelled() {
		metresTravelled += 1;

		if (metresTravelled % 30 == 0) {
			EmitSignal(SignalName.ThirtyMetresTravelled);
			spawnTimer.WaitTime -= spawnTimeDecrement;
			rockTimer.WaitTime -= rockSpawnTimeDecrement;
		}

		if (metresTravelled % 60 == 0) {
			EmitSignal(SignalName.SixtyMetresTravelled);

			if (currentBar > bars.Length - 1) return;

			if (currentBar == bars.Length - 1) {
				if (!canEndGame) {
					EmitSignal(SignalName.Won);
					CallDeferred("WinPause");
				}

				canEndGame = true;
				endGameScreen.Visible = true;
			}

			bars[currentBar].Color = new Color("34f35f");
			currentBar += 1;

			// Cap spawn chance at 1/2
			if (birdSpawnChance <= 2) return;
			birdSpawnChance -= 1;
		}
	}
}
