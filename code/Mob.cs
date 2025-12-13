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
	[Export] 
	private int _playerDamageRate = 1;
	[Export] 
	private AudioStreamPlayer2D _hitSound;
	private PackedScene _smokeScene;
	private Vector2 _faceDirection;
	
	private Dictionary<Vector2, StringName> _animationDictionary = new Dictionary<Vector2,StringName>();

	public override void _Ready()
	{
		_player = GetNode<Player>("/root/Game/Player");
		_smokeScene = ResourceLoader.Load<PackedScene>("res://scenes/smoke.tscn");
		
		Globals.Instance.UpdateSignal += UpdateStats;
		UpdateStats();
		
		_animationDictionary[Vector2.Down] = "down";
		_animationDictionary[Vector2.Up] = "up";
		_animationDictionary[Vector2.Left] = "left";
		_animationDictionary[Vector2.Right] = "right";
		_animationDictionary[Vector2.Zero] = "down";
	}

	private void UpdateStats()
	{
		_health = Globals.Instance.MobHealth;
		_speed = Globals.Instance.MobSpeed;
		_playerDamageRate = Globals.Instance.PlayerDamage;
	}
	//GetVelocity().Dot(moveDirection) < 0.01f && GetLastSlideCollision()?.GetCollider() is StaticBody2D
	public override void _PhysicsProcess(double delta)
	{
		Vector2 moveDirection = GlobalPosition.DirectionTo(_player.GlobalPosition);
		bool doNotUpdateAnimation = IsOnWall() && GetLastSlideCollision()?.GetCollider() is StaticBody2D; //janky but avoid weird figiting
		if (doNotUpdateAnimation) //avoid getting stuck
		{
			moveDirection = moveDirection.Slide(GetWallNormal()).Normalized();
		}
		
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
		
		//terrible
		if (!animName.Contains("damage") || !_animation.IsPlaying())
		{
			if (!doNotUpdateAnimation || (!_animation.IsPlaying() && animName.Contains("damage")))
			{
				_animation.Play(animationName);
			}	
			MoveAndSlide();
		}
		
		//Base
		
		base._PhysicsProcess(delta);
	}

	public void TakeDamage()
	{
		_animation.Play("damage"+_animationDictionary[_faceDirection]);
		_health -= _playerDamageRate;
		_hitSound.Play();
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
		Globals.Instance.UpdateSignal -= UpdateStats;
		base._ExitTree();
	}
}
