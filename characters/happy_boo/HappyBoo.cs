using Godot;
using System;

public partial class HappyBoo : Node2D
{
	AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		base._Ready();
	}

	public void Play_Idle_Animation()
	{
		animationPlayer.Play("idle");
	}

	public void Play_Walk_Animation()
	{
		animationPlayer.Play("walk");
	}
}


//extends Node2D
//
//
//func play_idle_animation():
	//%AnimationPlayer.play("idle")
//
//
//func play_walk_animation():
	//%AnimationPlayer.play("walk")
