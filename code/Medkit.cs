using Godot;
using System;
using EndOfDays;

public partial class Medkit : StaticBody2D
{
	[Export] private Area2D _pickupArea;
	public override void _Ready()
	{
		_pickupArea.BodyEntered += Heal;
		base._Ready();
	}

	private void Heal(Node2D body)
	{
		if (body is IHealable)
		{
			IHealable healable = body as IHealable;
			if (healable.Heal(20))
			{
				PlayerSound.Instance.Play("res://sounds/pickup.wav");
				QueueFree();
			}
		}
	}
}
