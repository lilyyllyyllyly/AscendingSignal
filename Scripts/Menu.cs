using Godot;
using System;

public partial class Menu : Node2D
{
	[Export] private PackedScene balloon;

	[Export] private ColorRect[] leftBars;
	[Export] private ColorRect[] rightBars;

	[Export] private ColorRect panel;

	private int activeBarIdx = 0;

	private Color inactive;
	private Color active;

	public override void _Ready() {
		inactive = new Color("9c9c9c");
		active = new Color("00d655");

		Saves.LoadGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("hide")) panel.Visible = !panel.Visible;
	}

	private void ResetBars() {
		for (int i = 0; i < leftBars.Length; i++) {
			leftBars[i].Color = inactive;
			rightBars[i].Color = inactive;
		} 
	}

	public void EndGame()
		=> GetTree().Quit();

	public void SpawnBalloon() {
		AddChild((Node2D)balloon.Instantiate());
	}

	public void FillBar() {
		if (activeBarIdx > leftBars.Length - 1) {
			ResetBars();
			activeBarIdx = 0;

			return;
		}

		leftBars[activeBarIdx].Color = active;
		rightBars[activeBarIdx].Color = active;

		activeBarIdx += 1;	
	}
}
