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

	[Export] private Label endGameText;

	[Export] private float spawnTimeDecrement;

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

	public override void _PhysicsProcess(double delta) {
		if (Input.IsActionJustPressed("leave") && canEndGame) {
			GetTree().ChangeSceneToFile("res://Scenes/States/Win.tscn");
		}
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
			spawnTimer.WaitTime -= spawnTimeDecrement;
		}

		if (metresTravelled % 60 == 0) {
			EmitSignal(SignalName.SixtyMetresTravelled);

			if (currentBar > bars.Length - 1) return;

			if (currentBar == bars.Length - 1) {
				if (!canEndGame) EmitSignal(SignalName.Won);

				canEndGame = true;
				endGameText.Visible = true;
			}

			bars[currentBar].Color = new Color("34f35f");
			currentBar += 1;

			// Cap spawn chance at 1/2
			if (birdSpawnChance <= 2) return;
			birdSpawnChance -= 1;
		}
	}
}
