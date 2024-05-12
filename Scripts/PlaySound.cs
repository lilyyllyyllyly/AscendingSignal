using Godot;
using System;

[GlobalClass]
public partial class PlaySound : AudioStreamPlayer
{
	[Export] private float pitchRange;

	public void PlayAudio()
	{
		PitchScale = 1 + (Random.Shared.NextSingle() - 0.5f) * pitchRange;
		Play();
	}
}
