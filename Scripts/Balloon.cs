using Godot;
using System;

public partial class Balloon : Node2D
{
	[Signal] public delegate void TakeHitEventHandler();

	[Export] private float maxHealth = 5;
	private float size;
	[Export] private float accel = 22.4f;

	[Export] private Flash sprite;

	[Export] private Vector2 velocity;

	[Export] private Node2D parent;

	private float _health;

	public float health {
		get {
			return _health;
		}

		set {
			if (value < _health) EmitSignal(SignalName.TakeHit);
			_health = value;
			if (IsInstanceValid(sprite)) {
				sprite.Frame = Mathf.Clamp((int)(maxHealth-value), 0, sprite.Hframes);
			}
		}
	}

	[Export] private AnimationPlayer anim;
	[Export] private AnimationPlayer fireAnim;

	[Export] private Label textHP;

	public override void _Ready()
	{
		health = maxHealth;

		anim.Play("wobble");
		anim.Seek((GD.Randi() % 10)/10.0);

		fireAnim.Play("fire");
		fireAnim.Seek((GD.Randi() % 3)/10.0);

		size = sprite.Texture.GetHeight();
	}

	public override void _Process(double delta)
	{
		textHP.Text = $"{health} HP";
		if (_health <= 0) {
			textHP.Visible = false;
			// Begin kill sequence.
			if (parent.Position.Y > 225 + size) {
				parent.QueueFree();
			}

			velocity.Y += accel * (float)delta;
			parent.Position += velocity * (float)delta;
		}
	}
}
