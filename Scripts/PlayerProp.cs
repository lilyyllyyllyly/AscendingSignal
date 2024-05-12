using Godot;
using System;

public partial class PlayerProp : Sprite2D
{
	public override void _Ready()
		=> GetNode<AnimationPlayer>("AnimationPlayer").Play("Fall");
}
