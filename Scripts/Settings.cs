using Godot;
using System;

public partial class Settings : Node2D
{
	[Export] private CheckBox fullScreen;
	[Export] private CheckBox screenShake;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fullScreen.ButtonPressed = Saves.WantsFullScreen;
		screenShake.ButtonPressed = Saves.WantsScreenShake;
	}

	public void ToggleFullScreen(bool button_pressed) {
		if (button_pressed)
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else 
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		
		Saves.WantsFullScreen = !Saves.WantsFullScreen;
		Saves.SaveGame();
	}

	public void ToggleScreenShake() {
		Saves.WantsScreenShake = !Saves.WantsScreenShake;
		Saves.SaveGame();
	}
}
