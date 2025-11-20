using Godot;
using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using Godot.Collections;

public partial class SurvivorsGame : Node2D
{
	[Export] 
	private PathFollow2D _genPath;
	[Export] 
	private Timer _spawnTimer;
	[Export]
	private CanvasLayer _gameOverLayer;
	[Export] 
	private CharacterBody2D _player;
	[Export] 
	private Node2D _mobsNode;
	
	private int _mobCount = 0;
	[Export] private int _maxMobs = 50;
	
	private PackedScene _mobScene;
	public override void _Ready()
	{
		_mobScene = ResourceLoader.Load<PackedScene>("res://scenes/mob.tscn");
		_spawnTimer.Timeout += () => SpawnMob();
		Player player = (Player)_player;
		player.GameOverSignal += () => GameOver();
		base._Ready();
	}

	public void SpawnMob()
	{
		_mobCount++;
		Node2D mob = null;
		if (_mobCount >= _maxMobs)
		{
			Array<Node> mobs = _mobsNode.GetChildren();
			Mob farthestMob = null;
			float distance = -1;
			int count = 0;
			
			foreach(Node node in mobs)
			{
				if (node is not Mob)
				{
					continue;
				}

				Mob curMob = node as Mob;
				
				if (curMob.GetGlobalPosition().DistanceTo(_player.GetGlobalPosition()) > distance)
				{
					farthestMob = curMob;
					distance = curMob.GetGlobalPosition().DistanceTo(_player.GetGlobalPosition());
				}

				count++;
			}
			
			if (farthestMob != null)
			{
				if (distance < 1000)
				{
					return; //if all of our mobs are on screen we do not spawn any more.
				}
				mob = farthestMob;
			}
			_mobCount = count;
		}
		else
		{
			mob = _mobScene.Instantiate<Node2D>();
			_mobsNode.AddChild(mob);
		}
		//Move selected mob
		Random rand = new Random();
		_genPath.SetProgressRatio((float)rand.NextDouble());
		mob.SetGlobalPosition(_genPath.GetGlobalPosition());
	}

	private void GameOver()
	{
		_gameOverLayer.SetVisible(true);
		GetTree().SetPause(true);
	}
}
