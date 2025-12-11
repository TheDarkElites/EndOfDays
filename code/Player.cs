using Godot;
using System;
using EndOfDays;
using Godot.Collections;

public partial class Player : CharacterBody2D, IHealable
{
	[Export]
	private double _health = 100.0;
	[Export] 
	private AnimatedSprite2D _animation;
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
	
	private Dictionary<Vector2, StringName> _animationDictionary = new Dictionary<Vector2,StringName>();

	public override void _Ready()
	{
		base._Ready();
		UpdateStats();
		Globals.Instance.UpdateSignal += UpdateStats;

		_animationDictionary[Vector2.Down] = "down";
		_animationDictionary[Vector2.Up] = "up";
		_animationDictionary[Vector2.Left] = "left";
		_animationDictionary[Vector2.Right] = "right";
		_animationDictionary[Vector2.Zero] = "down";
	}
	private void UpdateStats()
	{
		_healthBar.SetMax(Globals.Instance.PlayerHealth);
		_damageRate = Globals.Instance.MobDamageRate;
	}
	public override void _PhysicsProcess(double delta)
	{
		//Movement
		Vector2 moveDirection = Input.GetVector("move_left","move_right","move_up","move_down");
		SetVelocity(moveDirection * 600);
		MoveAndSlide();
		
		//Animation Handling
		Vector2 mouseDirection = Vector2.Right
			.Rotated(Mathf.Round(GlobalPosition.DirectionTo(GetGlobalMousePosition()).Angle() / (float)Math.Tau * 4) *
				(float)Math.Tau / 4).Snapped(Vector2.One);

		String animationName = _animationDictionary[mouseDirection];
		
		if (moveDirection.Length() != 0)
		{
			animationName += "walk";
			EmitSignalMovementSignal();
		}
		else
		{
			animationName += "idle";
		}

		if (mouseDirection.Normalized().Dot(moveDirection) < 0)
		{
			_animation.PlayBackwards(animationName);
		}
		else
		{
			_animation.Play(animationName);
		}
		
		//Health
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

	public override void _ExitTree()
	{
		Globals.Instance.UpdateSignal -= UpdateStats;
		base._ExitTree();
	}
}
