using System;

namespace EndOfDays.Attributes;
using Godot;

public abstract class Attribute
{
    public string Name;
    public string Description;
    public SceneTree SceneTree;
    public bool AvailableOnce;
    
    protected Attribute(SceneTree sceneTree, String name, String description, bool availableOnce = false)
    {
        SceneTree = sceneTree;
        Name = name;
        Description = description;
        AvailableOnce = availableOnce;
    }
    
    // This is called when an attribute is activated, so that we can have arbitrary code execution.
    public abstract bool Activate();

    public void SendUpdate()
    {
        Globals.Instance.EmitSignal("UpdateSignal");
    }
}