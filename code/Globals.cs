namespace GettingstartedwithGodot4;
using Godot;

public partial class Globals : Node
{
    [Export]
    public int MobHealth = 3;
    [Export]
    public double PlayerHealth = 100;
    [Export]
    public double MobDamageRate = 3;
    [Export]
    public float BulletSpeed = 1000;
    [Export]
    public float Distance = 12000;
}