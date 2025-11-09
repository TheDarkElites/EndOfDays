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
	
	private PackedScene _markerScene;
	private Dictionary<PackedScene, double>  _generationScenes = new Dictionary<PackedScene, double>();

	[Export] 
	private int _spacing = 200;

	[Export] 
	private int _radius = 10;
	public override void _Ready()
	{
		Player ourPlayer = (Player)_player;
		ourPlayer.MovementSignal += () => OnPlayerMove();
		
		_markerScene = ResourceLoader.Load<PackedScene>("res://scenes/marker.tscn");

		foreach (Vector2 direction in DIRECTIONS)
		{
			_bounds[direction] = 0;
		}
		
		PackedScene treeScene = ResourceLoader.Load<PackedScene>("res://scenes/tree.tscn");
		_generationScenes[treeScene] = 0.3;
		PackedScene buildingScene = ResourceLoader.Load<PackedScene>("res://scenes/building.tscn");
		_generationScenes[buildingScene] = 0.01;
	}

	private void OnPlayerMove()
	{
		foreach (Vector2 direction in DIRECTIONS)
		{
			float boundary = _bounds[direction];
			Vector2 traveledVector = _player.GetGlobalPosition() * direction;
			float distance = Mathf.CeilToInt((traveledVector.Length() * MathF.Sign(traveledVector.X + traveledVector.Y)) / _spacing) + _radius;
			if (distance > boundary)
			{
				_bounds[direction] = distance;
				Generate(direction);
			}
		}
	}

	private void Generate(Vector2 direction)
	{
		GD.Print(String.Format("North: {0}, South: {1}, West: {2}, East: {3}",_bounds[Vector2.Up],_bounds[Vector2.Down],_bounds[Vector2.Left],_bounds[Vector2.Right]));
		Vector2 left = new Vector2(-direction.Y, direction.X), right = new Vector2(direction.Y, -direction.X);
		float distanceLeft = -_bounds[left], distanceRight = _bounds[right];
		for (float i = distanceLeft; i < distanceRight; i++)
		{
			GenerateChunk(right * i + direction * _bounds[direction]);
		}
	}

	private void GenerateChunk(Vector2 position)
	{
		position *= _spacing;
		// Sprite2D newMarker = _markerScene.Instantiate<Sprite2D>();
		// newMarker.Position = position;
		// GetNode<Node2D>("/root/Game").AddChild(newMarker);
		PackedScene sceneToGenerate = null;
		Random random = new Random();

		foreach (PackedScene key in _generationScenes.Keys)
		{
			float diceroll = random.NextSingle();
			if (diceroll < _generationScenes[key])
			{
				sceneToGenerate = key;
				break;
			}
		}

		if (sceneToGenerate == null)
		{
			return;
		}

		Node2D newMapObject = sceneToGenerate.Instantiate<Node2D>();
		newMapObject.Position = position + new Vector2(random.NextSingle() * 100 - 50, random.NextSingle() * 100 - 50);
		GetNode<Node2D>("/root/Game").AddChild(newMapObject);
	}
}
