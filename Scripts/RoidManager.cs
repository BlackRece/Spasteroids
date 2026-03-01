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
				Spawn();
		}
	}

	private void Spawn()
	{
		var roid = RoidScene.Instantiate<Roid>();
		GetParent().AddChild(roid);
		_roids.Add(roid);
		
		_roidCount++;
	}
}
