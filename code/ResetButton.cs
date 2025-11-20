using Godot;
using System;

public partial class ResetButton : Button
{
	public override void _Ready()
	{
		this.Pressed += () => Restart();
	}
	private void Restart()
	{
		GD.Print("Restart");
		GetTree().SetPause(false);
		GetTree().ReloadCurrentScene();
	}
}
