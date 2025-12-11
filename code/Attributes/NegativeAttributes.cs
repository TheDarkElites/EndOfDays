namespace EndOfDays.Attributes;
using Godot;

public class IncreasedMobHealthAttribute : Attribute
{
    public IncreasedMobHealthAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Mob Health", "Enemies now take one additional shot to take down.") {}
    public override bool Activate()
    {
        Globals.Instance.MobHealth++;
        SendUpdate();
        return true;
    }
}

public class IncreasedMobSpeedAttribute : Attribute
{
    public IncreasedMobSpeedAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Mob Speed", "Enemies now move 25% faster.") {}
    public override bool Activate()
    {
        Globals.Instance.MobSpeed *= 1.25f;
        SendUpdate();
        return true;
    }
}

public class IncreasedMobDamageAttribute : Attribute
{
    public IncreasedMobDamageAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Mob Damage", "Enemies now do 25% more damage to the player.") {}
    public override bool Activate()
    {
        Globals.Instance.MobDamageRate *= 1.25;
        SendUpdate();
        return true;
    }
}

public class IncreasedBulletSpreadAttribute : Attribute
{
    public IncreasedBulletSpreadAttribute(SceneTree sceneTree) : base(sceneTree, "More Bullet Spread", "Increases bullet spread by 15%.") {}
    public override bool Activate()
    {
        Globals.Instance.BulletSpread *= 1.15f;
        SendUpdate();
        return true;
    }
}

public class IncreaseSpawnRateAttribute : Attribute
{
    public IncreaseSpawnRateAttribute(SceneTree sceneTree) : base(sceneTree, "More Zombies", "Increases zombie spawnrate by 10%.") {}
    public override bool Activate()
    {
        Timer spawnTimer = SceneTree.GetRoot().GetNode<Timer>("/root/Game/Timer");
        spawnTimer.WaitTime *= 0.90f;
        return true;
    }
}

public class IncreaseMobRangeAttribute : Attribute
{
    public IncreaseMobRangeAttribute(SceneTree sceneTree) : base(sceneTree, "Increase Mob Range", "Mobs can now damage the player from farther away.") {}
    public override bool Activate()
    {
        Area2D hurtArea = SceneTree.GetRoot().GetNode<Area2D>("/root/Game/Player/HurtBox");
        hurtArea.Scale *= 1.15f;
        return true;
    }
}