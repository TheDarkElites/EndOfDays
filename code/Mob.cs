using Godot;
using System;
using EndOfDays;
using Godot.Collections;

public partial class Mob : CharacterBody2D, IDamageable
{
	private Player _player;
	[Export]
	private int _health = 3;
	private float _speed = 300;
	[Export]
	private AnimatedSprite2D _animation;
	private PackedScene _smokeScene;
	private Vector2 _faceDirection;
	
	private Dictionary<Vector2, StringName> _animationDictionary = new Dictionary<Vector2,StringName>();

	public override void _Ready()
	{
		_player = GetNode<Player>("/root/Game/Player");
		_smokeScene = ResourceLoader.Load<PackedScene>("res://smoke_explosion/smoke_explosion.tscn");
		
		Globals GB = GetNode<Globals>("/root/Globals");
		GB.UpdateSignal += UpdateStats;
		
		_animationDictionary[Vector2.Down] = "down";
		_animationDictionary[Vector2.Up] = "up";
		_animationDictionary[Vector2.Left] = "left";
		_animationDictionary[Vector2.Right] = "right";
		_animationDictionary[Vector2.Zero] = "down";
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
		
		//Animation Handling
		_faceDirection = Vector2.Right
			.Rotated(Mathf.Round(moveDirection.Angle() / (float)Math.Tau * 4) *
				(float)Math.Tau / 4).Snapped(Vector2.One);

		String animationName = _animationDictionary[_faceDirection];
		
		if (moveDirection.Length() != 0)
		{
			animationName += "walk";
		}
		else
		{
			animationName += "idle";
		}

		String animName = _animation.GetAnimation();
		if (!animName.Contains("damage") || !_animation.IsPlaying())
		{
			_animation.Play(animationName);
			MoveAndSlide();
		}
		
		//Base
		
		base._PhysicsProcess(delta);
	}

	public void TakeDamage()
	{
		_animation.Play("damage"+_animationDictionary[_faceDirection]);
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
