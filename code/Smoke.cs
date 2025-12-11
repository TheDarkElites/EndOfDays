using Godot;
using System;

public partial class Smoke : AnimatedSprite2D
{
    public override void _Ready()
    {
        Play();
        AnimationFinished += QueueFree;
    }
}
