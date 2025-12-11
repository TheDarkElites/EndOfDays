using Godot;
using System;
using Godot.Collections;

public partial class Gun : Area2D
{
	private PackedScene _bulletScene;
	[Export]
	public Marker2D ShootingPoint;

	[Export] private Timer _timer;
	[Export] 
	private AnimatedSprite2D _animation;
	
	private Dictionary<Vector2, StringName> _animationDictionary = new Dictionary<Vector2,StringName>();

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
		newBullet.SetGlobalRotation(ShootingPoint.GetGlobalRotation());
		ShootingPoint.AddChild(newBullet);
		coolingDown = true;
		_timer.Start();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventKey shoot && shoot.IsActionPressed("shoot"))
		{
			Shoot();
		}
	}

	private void CooldownDone()
	{
		coolingDown = false;
	}
}
