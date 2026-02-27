namespace Spasteroids.Scripts;

using Godot;

public partial class Shots : Area2D
{
	[Export] public int Speed { get; set; } = 300;

	[Signal] public delegate void HitEventHandler();
	
	private Vector2 Velocity => Vector2.Up * Speed;
	
	private int _damage = 1;
	public int Damage => _damage;
	[Export] public int InitDamage { get; set; } = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_damage = InitDamage;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Velocity * (float)delta;

		if(Position.Y < 0)
			QueueFree();
	}

	public void OnAreaEntered(Area2D other)
	{
		QueueFree();
		//EmitSignal(SignalName.Hit);
		//GD.Print(other.Name);
	}
}