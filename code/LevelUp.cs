using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EndOfDays.Attributes;

public partial class LevelUp : CanvasLayer
{
	List<EndOfDays.Attributes.Attribute> PositiveAttributes;
	List<EndOfDays.Attributes.Attribute> NegativeAttributes;

	[Export] private Button Perk1;
	[Export] private Button Perk2;
	[Export] private Button Perk3;
	
	[Export]
	private Player _player;
	
	//Current Lambda Expression Handlers (Janky)
	private Action PH1;
	private Action PH2;
	private Action PH3;

	public override void _Ready()
	{
		PositiveAttributes = new List<EndOfDays.Attributes.Attribute>();
		//Now we add all the positive Attributes (this is bad but im strapped for time)
		PositiveAttributes.Add(new AllowBulletPenetrationAttribute(GetTree()));
		PositiveAttributes.Add(new IncreasedHealthAttribute(GetTree()));
		PositiveAttributes.Add(new IncreasedBulletDistanceAttribute(GetTree()));
		PositiveAttributes.Add(new IncreasedBulletSpeedAttribute(GetTree()));
		PositiveAttributes.Add(new DecreaseFireCooldownAttribute(GetTree()));
		PositiveAttributes.Add(new DecreaseGunSpreadAttribute(GetTree()));
		PositiveAttributes.Add(new IncreasePlayerSpeedAttribute(GetTree()));
		PositiveAttributes.Add(new IncreaseHealthKitAttribute(GetTree()));
		PositiveAttributes.Add(new RegenerateAttribute(GetTree()));
		PositiveAttributes.Add(new IncreasedDamageAttribute(GetTree()));

		NegativeAttributes = new List<EndOfDays.Attributes.Attribute>();
		//Now we add all the negative Attributes
		NegativeAttributes.Add(new IncreasedMobDamageAttribute(GetTree()));
		NegativeAttributes.Add(new IncreasedMobHealthAttribute(GetTree()));
		NegativeAttributes.Add(new IncreasedMobSpeedAttribute(GetTree()));

		_player.LevelUpSignal += InitiateLevelUp;
	}
	public void InitiateLevelUp()
	{
		SetVisible(true);
		GetTree().SetPause(true);

		EndOfDays.Attributes.Attribute AP1 = PositiveAttributes.PickRandom();
		if(AP1.AvailableOnce) {PositiveAttributes.Remove(AP1);}
		EndOfDays.Attributes.Attribute AN1 = NegativeAttributes.PickRandom();
		if(AN1.AvailableOnce) {NegativeAttributes.Remove(AN1);}
		EndOfDays.Attributes.Attribute AP2 = PositiveAttributes.PickRandom();
		if(AP2.AvailableOnce) {PositiveAttributes.Remove(AP2);}
		EndOfDays.Attributes.Attribute AN2 = NegativeAttributes.PickRandom();
		if(AN2.AvailableOnce) {NegativeAttributes.Remove(AN2);}
		EndOfDays.Attributes.Attribute AP3 = PositiveAttributes.PickRandom();
		if(AP3.AvailableOnce) {PositiveAttributes.Remove(AP3);}
		EndOfDays.Attributes.Attribute AN3 = NegativeAttributes.PickRandom();
		if(AN3.AvailableOnce) {NegativeAttributes.Remove(AN3);}

		Perk1.GetChild<RichTextLabel>(0).SetText(String.Format("[color=green]{0}[/color]\n\n[color=#228B22]{1}[/color]\n\n[color=red]{2}[/color]\n\n[color=#B22222]{3}[/color]", AP1.Name, AP1.Description, AN1.Name, AN1.Description));
		Perk2.GetChild<RichTextLabel>(0).SetText(String.Format("[color=green]{0}[/color]\n\n[color=#228B22]{1}[/color]\n\n[color=red]{2}[/color]\n\n[color=#B22222]{3}[/color]", AP2.Name, AP2.Description, AN2.Name, AN2.Description));
		Perk3.GetChild<RichTextLabel>(0).SetText(String.Format("[color=green]{0}[/color]\n\n[color=#228B22]{1}[/color]\n\n[color=red]{2}[/color]\n\n[color=#B22222]{3}[/color]", AP3.Name, AP3.Description, AN3.Name, AN3.Description));
		
		PH1 = () => AttributePicked(AP1, AN1);
		PH2 = () => AttributePicked(AP2, AN2);
		PH3 = () => AttributePicked(AP3, AN3);
		
		Perk1.Pressed += PH1;
		Perk2.Pressed += PH2;
		Perk3.Pressed += PH3;
	}

	public void AttributePicked(EndOfDays.Attributes.Attribute positiveAttribute, EndOfDays.Attributes.Attribute negativeAttribute)
	{
		positiveAttribute.Activate();
		negativeAttribute.Activate();

		Perk1.Pressed -= PH1;
		Perk2.Pressed -= PH2;
		Perk3.Pressed -= PH3;
		
		SetVisible(false);
		GetTree().SetPause(false);
	}
}
