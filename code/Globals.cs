namespace EndOfDays;
using Godot;

public partial class Globals : Node
{
	public static Globals Instance { get; private set; } 
	
	[Export]
	public int MobHealth = 3;

	private int _initialMobHealth = 3;
	[Export]
	public double PlayerHealth = 100;
	
	private double _initialPlayerHealth = 100;
	[Export]
	public double MobDamageRate = 3;
	
	private double _initialMobDamageRate = 3;
	[Export]
	public float MobSpeed = 300;
	
	private float _initialMobSpeed = 300;
	[Export]
	public float BulletSpeed = 1000;
	
	private float _initialBulletSpeed = 1000;
	[Export]
	public float Distance = 12000;
	
	private float _initialDistance = 12000;
	[Export]
	public int BulletPen = 0;
	
	private int _initialBulletPen = 0;
	[Export] 
	public float BulletSpread = 0.3f;
	
	private float _initialBulletSpread = 0.3f;
	[Signal]
	public delegate void UpdateSignalEventHandler();

	public void Reset()
	{
		MobHealth = _initialMobHealth;
		PlayerHealth = _initialPlayerHealth;
		MobDamageRate = _initialMobDamageRate;
		MobSpeed = _initialMobSpeed;
		BulletSpeed = _initialBulletSpeed;
		Distance = _initialDistance;
		BulletPen = _initialBulletPen;
		BulletSpread = _initialBulletSpread;
	}

	public override void _Ready()
	{
		Instance = this;
	}
}
