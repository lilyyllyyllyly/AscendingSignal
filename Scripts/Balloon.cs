using Godot;
using System;

public partial class Balloon : Node2D
{
	[Signal] public delegate void TakeHitEventHandler();

	[Export] private float maxHealth = 10;
	private float _health;

	public float health {
		get {
			return _health;
		}

		set {
			if (value < _health) EmitSignal(SignalName.TakeHit);
			_health = value;
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
	}

	public override void _Process(double delta)
	{
		textHP.Text = $"{health} HP";
		if (_health <= 0) {
			GetNode("..").QueueFree();
		}
	}
}
