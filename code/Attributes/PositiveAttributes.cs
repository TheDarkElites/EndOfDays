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
    public DecreaseFireCooldownAttribute(SceneTree sceneTree) : base(sceneTree, "Faster Firerate", "Increases the rate at which you shoot by 20%.") {}
    public override bool Activate()
    {
        Timer gunTimer = SceneTree.GetRoot().GetNode<Timer>("/root/Game/Player/Gun/Timer");
        gunTimer.WaitTime *= 0.80f;
        return true;
    }
}

public class DecreaseGunSpreadAttribute : Attribute
{
    public DecreaseGunSpreadAttribute(SceneTree sceneTree) : base(sceneTree, "Lower Spread", "Decreases the bullet spread by 10%.") {}
    public override bool Activate()
    {
        Globals.Instance.BulletSpread *= 0.9f;
        SendUpdate();
        return true;
    }
}

public class IncreasePlayerSpeedAttribute : Attribute
{
    public IncreasePlayerSpeedAttribute(SceneTree sceneTree) : base(sceneTree, "Faster Movement", "Increases player movement speed by 5%.") {}
    public override bool Activate()
    {
        Globals.Instance.PlayerSpeed *= 1.05f;
        SendUpdate();
        return true;
    }
}

public class IncreaseHealthKitAttribute : Attribute
{
    public IncreaseHealthKitAttribute(SceneTree sceneTree) : base(sceneTree, "Better Health Packs", "Health packs now give 20% more health.") {}
    public override bool Activate()
    {
        Globals.Instance.HealAmount *= 1.2f;
        SendUpdate();
        return true;
    }
}

public class RegenerateAttribute : Attribute
{
    public RegenerateAttribute (SceneTree sceneTree) : base(sceneTree, "Regeneration", "The player now naturally regenerates a small portion of their health.", true) {}
    public override bool Activate()
    {
        Globals.Instance.RegenerationAmount = 0.05 * Globals.Instance.PlayerHealth;
        SendUpdate();
        return true;
    }
}

public class IncreasedDamageAttribute : Attribute
{
    public IncreasedDamageAttribute (SceneTree sceneTree) : base(sceneTree, "More Damage", "The player does one additional point of damage to mobs.") {}
    public override bool Activate()
    {
        Globals.Instance.PlayerDamage++;
        SendUpdate();
        return true;
    }
}



