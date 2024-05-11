using Godot;
using System;

[GlobalClass]
public partial class GameManager : Node
{
	public static GameManager instance;

	[Export] public Balloon[] balloons;

	public override void _Ready()
	{
		if (GameManager.instance == null) {
			GameManager.instance = this;
		} else {
			QueueFree();
		}
	}
}
