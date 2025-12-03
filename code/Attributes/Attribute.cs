using System;

namespace GettingstartedwithGodot4.Attributes;
using Godot;

public abstract class Attribute
{
    public string Name;

    // This is called when an attribute is activated, so that we can have arbitrary code execution.
    public abstract bool Activate();
}