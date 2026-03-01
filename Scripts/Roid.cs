using Godot;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

namespace Spasteroids.Scripts;
public partial class Roid : Entity
{
	[Export] public float SpinSpeed { get; set; } = 0.125f;
	private float _spinDirection = 0f;
	private Sprite2D _sprite = null;

	private RoidType Type { get; set; } = RoidType.Large;
	public enum RoidType { Small = 0, Medium, Large }

	private Dictionary<RoidType, RoidData> _roidType = [];
	public record RoidData
	{
		public float Speed { get; init; } = 0f;
		public float Scale { get; init; } = 1f;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_roidType = new Dictionary<RoidType, RoidData>()
		{
			[RoidType.Small] = new(){ Scale = 1f, Speed = 30f},
			[RoidType.Medium] = new(){ Scale = 3f, Speed = 20f},
			[RoidType.Large] = new(){ Scale = 5f, Speed = 10f}
		};

		MaxSpeed = _roidType[Type].Speed;

		GlobalPosition = SetStartPosition();
		
		Rotation = (float)GD.RandRange(0f, Mathf.Tau);

		_sprite = GetNode<Sprite2D>("Sprite2D");
		_sprite.Rotation = (float)GD.RandRange(0f, Mathf.Tau);
		_spinDirection = GD.Randf() < 0.5f ? SpinSpeed * 1 : SpinSpeed * -1;

		Scale = Scale * _roidType[Type].Scale;
		
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_sprite.Rotation += _spinDirection;

		InputDirection = Vector2.Up.Rotated(Rotation) * 1f;// * _roidType[Type].Speed;
		
		base._Process(delta);
	}

	private Vector2 SetStartPosition() =>
		new(
			(GD.Randf() < 0.5f) ? 0f : Area.X,
			(GD.Randf() < 0.5f) ? 0f : Area.Y
		);
}
