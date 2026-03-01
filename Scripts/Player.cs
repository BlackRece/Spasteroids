using Godot;

namespace Spasteroids.Scripts;

public partial class Player : Entity
{
	[Export] public float BottomOffset { get; set; } = 50;

	[Export] public double ShotDelay { get; set; } = 0.1f;
	[Export] public float SpawnRadius { get; set; } = 1f;
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
		_bulletTimer.SetOneShot(true);
		_bulletTimer.Stop();
		
		StartingPos = new(
			Area.X / 2,
			Area.Y - BottomOffset);
		
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("kb_fire"))
			FireWeapon();
		
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveShip(delta); 
		
		base._PhysicsProcess(delta);
	}

	private void MoveShip(double delta)
	{
		// Apply velocity
		//InputDirection = InputAxis;
		
		// Left/right rotates the ship
		Rotation += TurnInput * RotateSpeed * (float)delta;

		// Up/down thrust along current facing direction
		InputDirection = Vector2.Up.Rotated(Rotation) * ThrustInput;
	}
	
	private void FireWeapon()
	{
		if (_bulletTimer.IsStopped())
		{
			// Reset ShotClock
			_bulletTimer.Start(ShotDelay);

			SpawnBullet();
		}
		else
		{
			//GD.PushWarning($"Timer: {_bulletTimer.GetTimeLeft()}");
		}
	}
	
	// TODO: move to weapon class
	private void SpawnBullet()
	{
		//GD.Print("Fired!");
		
		// TODO: use object pooling
		var shot = BulletScene.Instantiate<Shots>();
		shot.Init(new Shots.InitVars()
		{
			Position = Position,
			Rotation = Rotation,
			Range = SpawnRadius
		});
		GetParent().AddChild(shot);
	}
}
