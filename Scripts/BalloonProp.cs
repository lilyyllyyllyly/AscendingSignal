using Godot;
using System;

public partial class BalloonProp : Node2D
{
	private Vector2 gameSize = new Vector2(400, 225);

	private float speed = 0;

	[Export] private Sprite2D sprite;

	public override void _Ready()
	{
		// So to look it nicer we choose random scales.

		// We can either use HIGH precision, like below.
		//float scaleFactor = Random.Shared.NextSingle() * (0.7f - 0.4f) + 0.4f;
		
		// Or low precision, like we do now. This will limit sizes.
		// But the pixel grid wont be actual piss.
		float scaleFactor = Random.Shared.Next(4, 9) / 10f;
		sprite.Scale = new Vector2(scaleFactor, scaleFactor);

		// Modulate based on scale.
		// Fucking c# 
		var modulate = sprite.Modulate;
		modulate.A = scaleFactor;
		sprite.Modulate = modulate;

		// Pick random speed
		speed = Random.Shared.NextSingle() * (4f - 2f) + 2f;

		// Uhhh maybe we could make balloons move at an angle
		// But,, I'm not math girl :c

		Position = new Vector2(
			Random.Shared.Next(0, (int)gameSize.X),
			gameSize.Y + sprite.Texture.GetHeight() + 10
		);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 pos = Position;
		
		pos.Y -= speed;
		if (pos.Y < -sprite.Texture.GetHeight() - 10) {
			QueueFree();
		}
		
		Position = pos;
	}
}
