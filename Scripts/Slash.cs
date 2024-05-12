using Godot;
using System;

public partial class Slash : Node
{
	[Export] private AnimationPlayer anim;

	public override void _Ready()
	{
		anim.Play("slash");
	}
}
