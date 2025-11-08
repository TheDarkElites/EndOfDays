using Godot;
using System;
using Godot.Collections;

public partial class Generator : Node
{
	// Called when the node enters the scene tree for the first time.
	[Export] 
	private CharacterBody2D _player;

	private Dictionary<Vector2, float> _bounds = new Dictionary<Vector2, float>();
	
	private Vector2[] DIRECTIONS = [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];
	public override void _Ready()
	{
		Player ourPlayer = (Player)_player;
		ourPlayer.MovementSignal += () => OnPlayerMove();

		foreach (Vector2 direction in DIRECTIONS)
		{
			_bounds[direction] = 0;
		}
	}

	private void OnPlayerMove()
	{
		foreach (Vector2 direction in DIRECTIONS)
		{
			float boundary = _bounds[direction];
			Vector2 traveledVector = _player.GetGlobalPosition() * direction;
			float distance = traveledVector.Length() * MathF.Sign(traveledVector.X + traveledVector.Y);
			if (distance > boundary)
			{
				_bounds[direction] = distance;
				Generate();
			}
		}
	}

	private void Generate()
	{
		GD.Print(String.Format("North: {0}, South: {1}, West: {2}, East: {3}",_bounds[Vector2.Up],_bounds[Vector2.Down],_bounds[Vector2.Left],_bounds[Vector2.Right]));
	}
}
