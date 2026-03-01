namespace Spasteroids.Scripts;

using Godot;

public partial class Shots : Area2D
{
	[Export] public int Speed { get; set; } = 300;

	[Signal] public delegate void HitEventHandler();
	
	private Vector2 Velocity => Vector2.Up.Rotated(Rotation) * Speed;
	
	private int _damage = 1;
	public int Damage => _damage;
	[Export] public int InitDamage { get; set; } = 1;

	public record InitVars
	{
		public Vector2 Position { get; init; } = Vector2.Zero;
		public float Rotation { get; init; } = 0f;
		public float Range { get; init; } = 0f;
	}
	
	public void Init(InitVars vars)
	{
		Position = vars.Position;
		Rotation = vars.Rotation;
		Position += Vector2.Up.Rotated(Rotation) * vars.Range;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_damage = InitDamage;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// TODO: move in facing direction
		//Position += Vector2.Up.Rotated(Rotation) * (float)delta;
		Position += Velocity * (float)delta;

		// TODO: remove when outside of playing area
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