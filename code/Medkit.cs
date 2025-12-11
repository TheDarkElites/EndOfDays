using Godot;
using System;
using EndOfDays;

public partial class Medkit : StaticBody2D
{
	[Export] private Area2D _pickupArea;
	private float _healAmount = 20;
	public override void _Ready()
	{
		_pickupArea.BodyEntered += Heal;
		Globals.Instance.UpdateSignal += UpdateState;
		base._Ready();
	}

	private void UpdateState()
	{
		_healAmount = Globals.Instance.HealAmount;
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
	
	public override void _ExitTree()
	{
		Globals.Instance.UpdateSignal -= UpdateState;
		base._ExitTree();
	}
}
