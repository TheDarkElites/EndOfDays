using Godot;
using System;
using EndOfDays;
using Godot.Collections;

public partial class Gun : Area2D
{
	private PackedScene _bulletScene;
	[Export]
	public Marker2D ShootingPoint;

	[Export] 
	private Timer _timer;
	
	[Export] 
	private AnimatedSprite2D _animation;

	[Export] private AnimatedSprite2D _shootAnimation;
	
	private Dictionary<Vector2, StringName> _animationDictionary = new Dictionary<Vector2,StringName>();

	[Export] private AudioStreamPlayer2D _shootSound;
	
	private Random _random = new Random();

	private bool coolingDown = false;
	public override void _Ready()
	{
		_bulletScene = ResourceLoader.Load<PackedScene>("res://scenes/bullet.tscn");
		_timer.Timeout += () => CooldownDone();
		
		_animationDictionary[Vector2.Down] = "down";
		_animationDictionary[Vector2.Up] = "up";
		_animationDictionary[Vector2.Left] = "left";
		_animationDictionary[Vector2.Right] = "right";
		_animationDictionary[Vector2.Zero] = "down";
	}
	public override void _PhysicsProcess(double delta)
	{
		LookAt(GetGlobalMousePosition());
		
		Vector2 mouseDirection = Vector2.Right
			.Rotated(Mathf.Round(_animation.GetGlobalRotation() / (float)Math.Tau * 4) *
				(float)Math.Tau / 4).Snapped(Vector2.One);
		_animation.Play(_animationDictionary[mouseDirection]);
		base._PhysicsProcess(delta);
	}

	private void Shoot()
	{
		if (coolingDown)
		{
			return;
		}
		Area2D newBullet = _bulletScene.Instantiate<Area2D>();
		newBullet.SetGlobalPosition(ShootingPoint.GetGlobalPosition());
		newBullet.SetGlobalRotation(ShootingPoint.GetGlobalRotation() + (float)(_random.NextDouble() + (-1 * _random.NextDouble())) * Globals.Instance.BulletSpread);
		ShootingPoint.AddChild(newBullet);
		coolingDown = true;
		_shootAnimation.Play();
		_timer.Start();
		_shootSound.PitchScale = (float)Math.Clamp(_random.NextDouble() + 0.5, 0.8, 1.2);
		_shootSound.Play();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is not null && @event.IsActionPressed("shoot"))
		{
			Shoot();
		}
	}

	private void CooldownDone()
	{
		coolingDown = false;
		if(Input.IsActionPressed("shoot")) {Shoot();}
	}
}
