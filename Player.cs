using Godot;
using System;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	[Export]
	private double _health = 100.0;
	[Export] 
	public Area2D HurtBox;
	[Export] 
	private double _damageRate = 3.0;
	[Export] 
	private ProgressBar _healthBar;
	[Signal]
	public delegate void GameOverSignalEventHandler();
	[Signal]
	public delegate void MovementSignalEventHandler();
	public override void _PhysicsProcess(double delta)
	{
		Vector2 moveDirection = Input.GetVector("move_left","move_right","move_up","move_down");
		SetVelocity(moveDirection * 600);
		MoveAndSlide();
		
		HappyBoo body = GetNode<HappyBoo>("HappyBoo");

		if (moveDirection.Length() != 0)
		{
			body.Play_Walk_Animation();
			EmitSignalMovementSignal();
		}
		else
		{
			body.Play_Idle_Animation();
		}

		Array<Node2D> overlappingMobs = HurtBox.GetOverlappingBodies();
		_health -= overlappingMobs.Count * _damageRate * delta;
		_healthBar.SetValue(_health);

		if (_health <= 0)
		{
			EmitSignalGameOverSignal();
		}
		
		base._PhysicsProcess(delta);
	}
	
}
