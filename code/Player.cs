using Godot;
using System;
using EndOfDays;
using Godot.Collections;

public partial class Player : CharacterBody2D, IHealable
{
	[Export]
	private double _health = 100.0;
	[Export] 
	public Area2D HurtBox;
	[Export] 
	private double _damageRate = 3.0;
	[Export] 
	private ProgressBar _healthBar;
	[Export] 
	private ProgressBar _levelBar;
	[Export] 
	private Label _levelLabel;
	[Signal]
	public delegate void GameOverSignalEventHandler();
	[Signal]
	public delegate void MovementSignalEventHandler();
	[Signal]
	public delegate void LevelUpSignalEventHandler();
	[Export]
	private int _maxLevel = 2;
	[Export]
	private int _currentLevel = 0;
	[Export]
	private int _level = 0;

	public override void _Ready()
	{
		base._Ready();
		UpdateStats();
		Globals GB = GetNode<Globals>("/root/Globals");
		GB.UpdateSignal += UpdateStats;
	}
	private void UpdateStats()
	{
		Globals GB = GetNode<Globals>("/root/Globals");
		_health = GB.PlayerHealth;
		_damageRate = GB.MobDamageRate;
	}
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

	public void IncrementLevel()
	{
		_currentLevel++;
		if (_currentLevel == _maxLevel)
		{
			_maxLevel *= 2;
			_level++;
			EmitSignalLevelUpSignal();
			_currentLevel = 0;
			_levelBar.SetMax(_maxLevel);
			_levelLabel.SetText(_level.ToString());
		}
		_levelBar.SetValue(_currentLevel);
	}

	public bool Heal(int amount)
	{
		if (_health == _healthBar.GetMax())
		{
			return false;
		}
		_health += amount;
		_health = Math.Min(_health, _healthBar.GetMax());
		_healthBar.SetValue(_health);
		return true;
	}
	
}
