using Godot;
using System;
using EndOfDays;

public partial class Mob : CharacterBody2D, IDamageable
{
	private Player _player;
	[Export]
	private int _health = 3;
	private float _speed = 300;
	private Slime _slime;
	private PackedScene _smokeScene;

	public override void _Ready()
	{
		_player = GetNode<Player>("/root/Game/Player");
		_slime = GetNode<Slime>("Slime");
		_smokeScene = ResourceLoader.Load<PackedScene>("res://smoke_explosion/smoke_explosion.tscn");
		
		Globals GB = GetNode<Globals>("/root/Globals");
		
	}

	private void UpdateStats()
	{
		Globals GB = GetNode<Globals>("/root/Globals");
		_health = GB.MobHealth;
		_speed = GB.MobSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 moveDirection = GlobalPosition.DirectionTo(_player.GlobalPosition);
		SetVelocity(moveDirection * _speed);
		MoveAndSlide();
		
		_slime.Play_Walk_Animation();
		base._PhysicsProcess(delta);
	}

	public void TakeDamage()
	{
		_slime.Play_Hurt_Animation();
		_health--;
		if (_health <= 0)
		{
			Die();
			_player.IncrementLevel();
		}
	}

	public void Die()
	{
		QueueFree();
		Node2D smoke = _smokeScene.Instantiate<Node2D>();
		smoke.SetGlobalPosition(GetGlobalPosition());
		GetParent().AddChild(smoke);
	}
	
	public override void _ExitTree()
	{
		Globals GB = GetNode<Globals>("/root/Globals");
		GB.UpdateSignal -= UpdateStats;
		base._ExitTree();
	}
}
