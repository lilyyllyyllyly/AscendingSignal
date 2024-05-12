using Godot;
using System;

public partial class WinAnimation : Node
{
	[Export] private AnimationPlayer anim;

	public override void _Ready()
	{
		anim.Play("yes");
	}
}
