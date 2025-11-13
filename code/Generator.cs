using Godot;
using System;
using Godot.Collections;
using GettingstartedwithGodot4;

public partial class Generator : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] 
	private CharacterBody2D _player;

	private Dictionary<Vector2, float> _bounds = new Dictionary<Vector2, float>();
	
	private Vector2[] DIRECTIONS = [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];
	
	private PackedScene _markerScene;
	private GeneratableObject[] _objects;

	[Export] 
	private int _spacing = 150;

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
		PackedScene buildingScene = ResourceLoader.Load<PackedScene>("res://scenes/building.tscn"); 
		PackedScene medkitScene = ResourceLoader.Load<PackedScene>("res://scenes/medkit.tscn");
		_objects = new []{new GeneratableObject(treeScene, (float)0.5), new GeneratableObject(buildingScene, (float)0.05), new  GeneratableObject(medkitScene, (float)0.01)};

		for (int i = 0; i < _objects.Length; i++)
		{
			GeneratableObject obj = _objects[i];
			StaticBody2D tempInstance = obj.Scene.Instantiate<StaticBody2D>();
			CollisionShape2D collisionShapeNode = tempInstance.GetNode<Node2D>("Collision") as CollisionShape2D;

			if (collisionShapeNode == null)
			{
				GD.PrintErr(String.Format("No CollisionShape2D found in the PackedScene! {0} {1}", obj.Scene.ResourcePath, tempInstance.GetName()));
				continue;
			}

			if (collisionShapeNode.Shape == null)
			{
				GD.PrintErr("No CollisionShape2D with a valid Shape found in the PackedScene!");
				continue;
			}

			obj.Shape = new PhysicsShapeQueryParameters2D
			{
				Shape = collisionShapeNode.Shape
			};
			
			_objects[i] = obj;
			tempInstance.QueueFree();
		}
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
		PackedScene sceneToGenerate = null;
		Random random = new Random();

		foreach (GeneratableObject obj in _objects)
		{
			float diceroll = random.NextSingle();
			//PhysicsShapeQueryParameters2D shape = 
			obj.Shape.Transform = new Transform2D(0, position);
			if (GetWorld2D().DirectSpaceState.CollideShape(obj.Shape).Count != 0)
			{
				GD.PrintErr("Attempted to spawn, detected collision and avoided");
			}
			if (diceroll < obj.Probability && GetWorld2D().DirectSpaceState.CollideShape(obj.Shape).Count == 0)
			{
				sceneToGenerate = obj.Scene;
				break;
			}
		}

		if (sceneToGenerate == null)
		{
			return;
		}

		Node2D newMapObject = sceneToGenerate.Instantiate<Node2D>();
		newMapObject.Position = position + new Vector2(random.NextSingle() * 100 - 50, random.NextSingle() * 100 - 50);
		GetNode<Node2D>("/root/Game/Trees").AddChild(newMapObject);
	}
	
	//private bool CheckCollision(Vector2 position, )
}
