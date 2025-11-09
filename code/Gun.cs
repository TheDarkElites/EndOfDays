using Godot;
using System;
using Godot.Collections;

public partial class Gun : Area2D
{
	private PackedScene _bulletScene;
	[Export]
	public Marker2D ShootingPoint;
	public override void _Ready()
	{
		_bulletScene = ResourceLoader.Load<PackedScene>("res://scenes/bullet.tscn");
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += () => Shoot();
	}
	public override void _PhysicsProcess(double delta)
	{
		Array<Node2D> targets_in_range = GetOverlappingBodies();
		if (targets_in_range.Count > 0)
		{
			Node2D target = targets_in_range[0];
			LookAt(target.GetGlobalPosition());
		}
		base._PhysicsProcess(delta);
	}

	public void Shoot()
	{
		Area2D newBullet = _bulletScene.Instantiate<Area2D>();
		newBullet.SetGlobalPosition(ShootingPoint.GetGlobalPosition());
		newBullet.SetGlobalRotation(ShootingPoint.GetGlobalRotation());
		ShootingPoint.AddChild(newBullet);
	}
}
