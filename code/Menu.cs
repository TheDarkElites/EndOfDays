using Godot;
using System;

public partial class Menu : CanvasLayer
{
    [Export] private Player _player;
    [Export] private Label _label1;
    [Export] private Label _label2;

    private static readonly double VanishTime = 2;
    
    public override void _Ready()
    {
        _player.MovementSignal += MenuFade;
        base._Ready();
    }

    private void MenuFade()
    {
        _player.MovementSignal -= MenuFade;

        _label1.CreateTween().TweenProperty(_label1, "modulate:a", 0, VanishTime);
        _label2.CreateTween().TweenProperty(_label2, "modulate:a", 0, VanishTime);
    }
}
