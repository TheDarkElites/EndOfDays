using Godot;
using System;

public partial class Slime : Node2D
{
	AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		base._Ready();
	}

	public void Play_Hurt_Animation()
	{
		animationPlayer.Play("hurt");
	}

	public void Play_Walk_Animation()
	{
		if (animationPlayer.IsPlaying())
		{
			return;
		}
		animationPlayer.Play("walk");
	}
}
