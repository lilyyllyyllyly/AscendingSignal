using Godot;
using System;

public partial class Flash : Sprite2D
{
	[Export] private float flashTime = 100;
	private float lastFlash;

	[Export] private Material flashMaterial;

	public void StartFlash()
	{
		Material = flashMaterial;
		lastFlash = Time.GetTicksMsec();
	}

	public override void _Process(double delta)
	{
		if (Material == flashMaterial && Time.GetTicksMsec() - lastFlash > flashTime) {
			Material = null;
		}
	}
}
