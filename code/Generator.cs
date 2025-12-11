using Godot;
using System;
using System.Collections.Generic;
using EndOfDays;

public partial class Generator : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] 
	private CharacterBody2D _player;
	
	private PackedScene _markerScene;
	private GeneratableObject[] _objects;

	private HashSet<Vector2I> _generatedChunks = new HashSet<Vector2I>();

	[Export] 
	private int _spacing = 150;

	[Export] 
	private int _radius = 8;
	public override void _Ready()
	{
		Player ourPlayer = (Player)_player;
		ourPlayer.MovementSignal += () => OnPlayerMove();
		
		_markerScene = ResourceLoader.Load<PackedScene>("res://scenes/marker.tscn");
		
		PackedScene treeScene = ResourceLoader.Load<PackedScene>("res://scenes/tree.tscn");
		PackedScene buildingScene = ResourceLoader.Load<PackedScene>("res://scenes/building.tscn"); 
		PackedScene medkitScene = ResourceLoader.Load<PackedScene>("res://scenes/medkit.tscn");
		_objects = new []{new GeneratableObject(treeScene, (float)0.8), new GeneratableObject(buildingScene, (float)0.05), new  GeneratableObject(medkitScene, (float)0.03)};

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
		
		OnPlayerMove();
	}

	private void OnPlayerMove()
	{
		Generate(new Vector2I(Mathf.CeilToInt(_player.GetGlobalPosition().X / _spacing), Mathf.CeilToInt(_player.GetGlobalPosition().Y / _spacing)));
	}

	private void Generate(Vector2I position)
	{
		Vector2I currentGen = position;

		for (currentGen.X = position.X - _radius; currentGen.X < position.X + _radius; currentGen += new Vector2I(1,0))
		{
			for (currentGen.Y = position.Y - _radius; currentGen.Y < position.Y + _radius; currentGen += new Vector2I(0,1))
			{
				if (_generatedChunks.Contains(currentGen))
				{
					continue;
				}
				GenerateChunk(currentGen);
			}
		}
	}

	private void GenerateChunk(Vector2 position)
	{
		_generatedChunks.Add(new Vector2I((int)position.X, (int)position.Y));
		position *= _spacing;
		Random random = new Random();
		PackedScene sceneToGenerate = null;
		position += new Vector2(random.NextSingle() * 100 - 50, random.NextSingle() * 100 - 50);

		foreach (GeneratableObject obj in _objects)
		{
			float diceroll = random.NextSingle();
			obj.Shape.Transform = new Transform2D(0, position);
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
		newMapObject.Position = position;
		GetNode<Node2D>("/root/Game/Trees").AddChild(newMapObject);
	}
}
