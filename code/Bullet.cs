using Godot;
using System;
using EndOfDays;

public partial class Bullet : Area2D
{
	private double distance_travelled = 0;
	private float Speed = 1000;
	private float Distance = 12000;
	private int Penetration = 0;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		Globals GB = GetNode<Globals>("/root/Globals");
		Speed = GB.BulletSpeed;
		Distance = GB.Distance;
		Penetration = GB.BulletPen;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Right.Rotated(GetRotation());
		SetPosition(GetPosition() + direction * Speed * (float)delta);

		distance_travelled += delta * Speed;

		if (distance_travelled > Distance)
		{
			QueueFree();
		}
		base._PhysicsProcess(delta);
	}

	private void OnBodyEntered(Node2D body)
	{
		if(Penetration == 0) {QueueFree();}
		Penetration--;
		if (body is IDamageable)
		{
			IDamageable damageable = body as IDamageable;
			damageable.TakeDamage();
		}
	}
}
