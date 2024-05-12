using Godot;
using System;

public partial class Settings : Node2D
{
	[Export] private CheckBox screenShake;

	public override void _Ready()
	{
		screenShake.ButtonPressed = Saves.WantsScreenShake;
	}

	public void ToggleScreenShake() {
		Saves.WantsScreenShake = !Saves.WantsScreenShake;
		Saves.SaveGame();
	}
}
