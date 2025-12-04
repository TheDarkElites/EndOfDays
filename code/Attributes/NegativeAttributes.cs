namespace EndOfDays.Attributes;
using Godot;

public class IncreasedMobHealthAttribute : Attribute
{
    public IncreasedMobHealthAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Mob Health", "Enemies now take one additional shot to take down.") {}
    public override bool Activate()
    {
        GB.MobHealth++;
        SendUpdate();
        return true;
    }
}

public class IncreasedMobSpeedAttribute : Attribute
{
    public IncreasedMobSpeedAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Mob Speed", "Enemies now move 25% faster.") {}
    public override bool Activate()
    {
        GB.MobSpeed *= (float)1.25;
        SendUpdate();
        return true;
    }
}

public class IncreasedMobDamageAttribute : Attribute
{
    public IncreasedMobDamageAttribute(SceneTree sceneTree) : base(sceneTree, "Increased Mob Damage", "Enemies now do 25% more damage to the player.") {}
    public override bool Activate()
    {
        GB.MobDamageRate *= 1.25;
        SendUpdate();
        return true;
    }
}