namespace EndOfDays;
using Godot;

//Special singleton for when non player objects need to play a sound directly to the player's ear. Useful in certain special cases.

public partial class PlayerSound : Node
{
    public static PlayerSound Instance { get; private set; }

    private AudioStreamPlayer2D _playerSound;

    public override void _Ready()
    {
        Instance = this;
        RegisterPlayer();
        base._Ready();
    }

    public void Play(string sound, bool ignorePlaying = false)
    {
        if (!ignorePlaying && _playerSound.IsPlaying()) { return;}
        _playerSound.Stream = ResourceLoader.Load<AudioStreamWav>(sound);
        _playerSound.Play();
    }

    public void RegisterPlayer()
    {
        _playerSound = GetNode<AudioStreamPlayer2D>("/root/Game/Player/Sound");
    }
}