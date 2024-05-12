using Godot;
using System;

public partial class Pause : Control
{
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause")) {
			Visible = !Visible;
			GetTree().Paused = !GetTree().Paused;
		}
	}

	public void GameResume() {
		Visible = !Visible;
		GetTree().Paused = !GetTree().Paused;
	}
}
