using System.Collections.Generic;
using Godot;

namespace Spasteroids.Scripts;

public partial class RoidManager : Node2D
{
	[Export] public PackedScene RoidScene;

	private int _roidCount = 1;
	private List<Roid> _roids = [];
	
	private int _levelCount = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_roids.Count < 1)
		{
			_levelCount++;

			for (var i = 0; i < _levelCount; i++)
				Spawn(Roid.RoidData.Default());
		}
	}

	private void Spawn(Roid.RoidData data)
	{
		var roid = RoidScene.Instantiate<Roid>();
		roid.Init(data);
		GetParent().AddChild(roid);

		roid.Destroyed += OnRoidDestroyed;
		_roids.Add(roid);
		
		_roidCount++;
	}
	
	private void OnRoidDestroyed(Roid roid)
	{
		var data = new Roid.RoidData()
		{
			Type = roid.Type switch
			{
				Roid.RoidType.Large => Roid.RoidType.Medium,
				Roid.RoidType.Medium => Roid.RoidType.Small,
				Roid.RoidType.Small => Roid.RoidType.None
			},
			Pos = roid.Position
		};

		if (data.Type != Roid.RoidType.None)
		{
			Spawn(data);
			Spawn(data);
		}
		
		roid.Destroyed -= OnRoidDestroyed;
		_roids.Remove(roid);
	}
}
