namespace EndOfDays.Attributes;
using Godot;

public class IncreasedHealthAttribute : Attribute
{
    public IncreasedHealthAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Health", "Increases player health by 25%") {}
    public override bool Activate()
    {
        Globals.Instance.PlayerHealth *= 1.25;
        SendUpdate();
        return true;
    }
}

public class IncreasedBulletSpeedAttribute : Attribute
{
    public IncreasedBulletSpeedAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Bullet Speed", "Increases player bullet speed by 25%") {}
    public override bool Activate()
    {
        Globals.Instance.BulletSpeed *= (float)1.25;
        SendUpdate();
        return true;
    }
}

public class IncreasedBulletDistanceAttribute : Attribute
{
    public IncreasedBulletDistanceAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Bullet Distance", "Increases player bullet range by 25%") {}
    public override bool Activate()
    {
        Globals.Instance.Distance *= (float)1.25;
        SendUpdate();
        return true;
    }
}

public class AllowBulletPenetrationAttribute : Attribute
{
    public AllowBulletPenetrationAttribute(SceneTree sceneTree) : base(sceneTree, "Bullet Penetration", "Allows for bullets to penetrate through one enemy before stopping.", true) {}
    public override bool Activate()
    {
        Globals.Instance.BulletPen = 1;
        SendUpdate();
        return true;
    }
}

public class DecreaseFireCooldownAttribute : Attribute
{
    public DecreaseFireCooldownAttribute(SceneTree sceneTree) : base(sceneTree, "Faster Firerate", "Increases the rate at which you shoot.") {}
    public override bool Activate()
    {
        Timer gunTimer = SceneTree.GetRoot().GetNode<Timer>("/root/Game/Player/Gun/Timer");
        gunTimer.WaitTime *= 0.80f;
        return true;
    }
}

