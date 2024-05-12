using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] private float intensityMultiplier = 5;
	private float currentIntensity;

	public void Shake(float intensity)
	{
		if (Saves.WantsScreenShake)
			currentIntensity = intensity;
	}

	public override void _Process(double delta)
	{
		currentIntensity -= (float)delta;
		if (currentIntensity <= 0) {
			Position = Vector2.Zero;
			return;
		}

		float rot = Random.Shared.NextSingle() * Mathf.Tau;
		Position = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot)) * currentIntensity * intensityMultiplier;
	}
}
