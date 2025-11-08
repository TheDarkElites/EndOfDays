using Godot;
using System;
using System.Dynamic;

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
	
	private PackedScene _mobScene;
	public override void _Ready()
	{
		_mobScene = ResourceLoader.Load<PackedScene>("res://mob.tscn");
		_spawnTimer.Timeout += () => SpawnMob();
		Player player = (Player)_player;
		player.GameOverSignal += () => GameOver();
		base._Ready();
	}

	public void SpawnMob()
	{
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
