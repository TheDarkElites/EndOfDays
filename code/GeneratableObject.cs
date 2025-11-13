namespace GettingstartedwithGodot4;
using Godot;

public struct GeneratableObject
{
    public PackedScene Scene;
    public float Probability;
    public PhysicsShapeQueryParameters2D Shape;

    public GeneratableObject(PackedScene scene, float probability)
    {
        Scene = scene;
        Probability = probability;
        Shape = null;
    }
}