namespace EndOfDays.Attributes;
using Godot;

public class IncreasedHealthAttribute : Attribute
{
    public IncreasedHealthAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Health", "Increases player health by 25%") {}
    public override bool Activate()
    {
        GB.PlayerHealth *= 1.25;
        SendUpdate();
        return true;
    }
}

public class IncreasedBulletSpeedAttribute : Attribute
{
    public IncreasedBulletSpeedAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Bullet Speed", "Increases player bullet speed by 25%") {}
    public override bool Activate()
    {
        GB.BulletSpeed *= (float)1.25;
        SendUpdate();
        return true;
    }
}

public class IncreasedBulletDistanceAttribute : Attribute
{
    public IncreasedBulletDistanceAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Bullet Distance", "Increases player bullet range by 25%") {}
    public override bool Activate()
    {
        GB.Distance *= (float)1.25;
        SendUpdate();
        return true;
    }
}

public class AllowBulletPenetrationAttribute : Attribute
{
    public AllowBulletPenetrationAttribute(SceneTree sceneTree) : base(sceneTree, "Bullet Penetration", "Allows for bullets to penetrate through one enemy before stopping.", true) {}
    public override bool Activate()
    {
        GB.BulletPen = 1;
        SendUpdate();
        return true;
    }
}

