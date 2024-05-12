using Godot;
using System;

public partial class Settings : Node2D
{
	[Export] private CheckBox screenShake;
	[Export] private CheckBox enableFlashing;

	public override void _Ready()
	{
		screenShake.ButtonPressed = Saves.WantsScreenShake;
		enableFlashing.ButtonPressed = Saves.WantsFlashing;
	}

	public void ToggleScreenShake() {
		Saves.WantsScreenShake = !Saves.WantsScreenShake;
		Saves.SaveGame();
	}

	public void ToggleFlashing() {
		Saves.WantsFlashing = !Saves.WantsFlashing;
		Saves.SaveGame();
	}
}
