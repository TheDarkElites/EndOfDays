using Godot;
using System;
using Godot.Collections;

public partial class Gun : Area2D
{
	private PackedScene _bulletScene;
	[Export]
	public Marker2D ShootingPoint;

	[Export] private Timer _timer;

	private bool coolingDown = false;
	public override void _Ready()
	{
		_bulletScene = ResourceLoader.Load<PackedScene>("res://scenes/bullet.tscn");
		_timer.Timeout += () => CooldownDone();
	}
	public override void _PhysicsProcess(double delta)
	{
		LookAt(GetGlobalMousePosition());
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
