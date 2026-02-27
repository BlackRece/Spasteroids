using Godot;
using Spasteroids.Scripts;

public partial class Player : Entity
{
	[Export] public float BottomOffset { get; set; } = 50;

	[Export] public double ShotDelay { get; set; } = 0.1f;
	[Export] public PackedScene BulletScene { get; set; }
	private Timer _bulletTimer;
	
	private Vector2 InputAxis => Input.GetVector(
		negativeX: "kb_left",
		positiveX: "kb_right",
		negativeY: "kb_up",
		positiveY: "kb_down");
	
	private float TurnInput => Input.GetAxis(
		negativeAction: "kb_left",
		positiveAction: "kb_right");

	private float ThrustInput => Input.GetAxis(
		negativeAction: "kb_down",
		positiveAction: "kb_up");
	
	public override void _Ready()
	{
		_bulletTimer = GetNode<Timer>("ShotClock");
		_bulletTimer.Stop();
		
		StartingPos = new(
			Area.X / 2,
			Area.Y - BottomOffset);
		
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if (_bulletTimer.IsStopped() &&
		    Input.IsActionPressed("kb_fire"))
		{
			// Reset ShotClock
			_bulletTimer.Start(ShotDelay);

			SpawnBullet();
		}
		
		// Apply velocity
		//InputDirection = InputAxis;
		
		// Left/right rotates the ship
		Rotation += TurnInput * RotateSpeed * (float)delta;

		// Up/down thrust along current facing direction
		var forward = Vector2.Up.Rotated(Rotation);
		InputDirection = forward * ThrustInput;
		base._Process(delta);
	}
	
	private void SpawnBullet()
	{
		/*
		var shot = BulletScene.Instantiate<Shots>();
		shot.Position = Position;
		GetParent().AddChild(shot);
		*/
	}
}
