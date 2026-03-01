using Godot;
using System.Collections.Generic;

namespace Spasteroids.Scripts;

public partial class Roid : Entity
{
	[Signal]
	public delegate void DestroyedEventHandler(Roid roid); 
	[Export] public float SpinSpeed { get; set; } = 0.125f;
	private float _spinDirection = 0f;
	private Sprite2D _sprite = null;

	public int MaxHitPoints { get; set; } = 10;
	private int _hitPoints = 0;

	public RoidType Type { get; private set; } = RoidType.Large;
	public enum RoidType { None = 0, Small, Medium, Large }

	private Dictionary<RoidType, RoidTypeData> _roidType = [];
	public record RoidTypeData
	{
		public float Speed { get; init; } = 0f;
		public float Scale { get; init; } = 1f;
	}
	
	public record RoidData
	{
		public RoidType Type { get; init; } = RoidType.None;
		public Vector2 Pos { get; init; } = Vector2.Zero;
		
		public static RoidData Default() => new ()
		{
			Type = RoidType.Large,
			Pos = Vector2.Zero
		};
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_roidType = new Dictionary<RoidType, RoidTypeData>()
		{
			[RoidType.Small] = new(){ Scale = 1f, Speed = 20f},
			[RoidType.Medium] = new(){ Scale = 3f, Speed = 10f},
			[RoidType.Large] = new(){ Scale = 5f, Speed = 5f}
		};

		MaxSpeed = _roidType[Type].Speed;

		if(Type == RoidType.Large)
			GlobalPosition = SetStartPosition();
		
		Rotation = (float)GD.RandRange(0f, Mathf.Tau);

		_sprite = GetNode<Sprite2D>("Sprite2D");
		_sprite.Rotation = (float)GD.RandRange(0f, Mathf.Tau);
		_spinDirection = GD.Randf() < 0.5f ? SpinSpeed * 1 : SpinSpeed * -1;

		Scale = Scale * _roidType[Type].Scale;

		// TODO: to be multiplied by level
		_hitPoints = MaxHitPoints;
		
		AreaEntered += OnAreaEntered;
		
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_sprite.Rotation += _spinDirection;
		
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		InputDirection = Vector2.Up.Rotated(Rotation);
		
		base._PhysicsProcess(delta);
	}

	private Vector2 SetStartPosition()
	{
		var pos = new Vector2(
			(float)GD.RandRange(0f, Area.X),
			(float)GD.RandRange(0f, Area.Y)
		);

		return GD.RandRange(1, 4) switch
		{
			1 => new Vector2(pos.X, 0), // top
			2 => new Vector2(Area.X, pos.Y), // right
			3 => new Vector2(pos.X, Area.X), // bottom
			4 => new Vector2(0, pos.Y), // left
			_ => pos
		};
	}

	public void Init(RoidData data)
	{
		Type = data.Type;
		Position = data.Pos;
	}
	public void SetType(RoidType type = RoidType.Large) => Type = type;

	public void OnAreaEntered(Area2D other)
	{
		switch (other)
		{
			case Shots shot:
				// call shot methods
				_hitPoints--;
				break;
			case Player player:
				// call player methods
				break;
			case Entity entity:
				// call entity methods
				break;
		}

		if (_hitPoints <= 0)
		{
			EmitSignal(SignalName.Destroyed, this);
			QueueFree();
		}
	}
}
