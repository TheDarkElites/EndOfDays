using Godot;
using System;
using System.Collections;
using System.Dynamic;
using System.Linq;

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
	private int _mobCount = 0;
	[Export] private int _maxMobs = 100;
	
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
		if (_mobCount >= _maxMobs)
		{
			IEnumerable mobs = GetChildren().Where(x => x is Mob);
			Mob farthestMob = null;
			float distance = -1;
			int count = 0;
			
			foreach(Mob curMob in mobs)
			{
				if (curMob.GetGlobalPosition().DistanceTo(_player.GetGlobalPosition()) > distance)
				{
					farthestMob = curMob;
					distance = curMob.GetGlobalPosition().DistanceTo(_player.GetGlobalPosition());
				}

				count++;
			}
			
			GD.Print(String.Format("Mob Count: {0}", count));
			if (farthestMob != null)
			{
				farthestMob.Die();
			}
			_mobCount = count;
		}
		Node2D mob = _mobScene.Instantiate<Node2D>();
		Random rand = new Random();
		_genPath.SetProgressRatio((float)rand.NextDouble());
		mob.SetGlobalPosition(_genPath.GetGlobalPosition());
		AddChild(mob);
	}

	private void GameOver()
	{
		_gameOverLayer.SetVisible(true);
		GetTree().SetPause(true);
	}
}
