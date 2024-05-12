using Godot;
using System;

[GlobalClass]
public partial class SceneChanger : Node
{
	[Export(PropertyHint.File)] private string scenePath;

	public void ChangeScene()
	{
		GetTree().CallDeferred("change_scene_to_file", scenePath);
	}
}
