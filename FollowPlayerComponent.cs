using Godot;
using System;

public partial class FollowPlayerComponent : Node
{
	private EnemyBase enemy;
	
	public override void _Ready()
	{
	GD.Print("Padre real de FollowPlayerComponent: " + GetParent().Name);
	GD.Print("Tipo del padre: " + GetParent().GetType());
		enemy = GetParent<EnemyBase>();
		if (enemy == null)
		{
			GD.PrintErr("FollowPlayerComponent debe ser hijo directo de un EnemyBase.");
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if(enemy == null || enemy.Player == null) return;
		
		Vector2 direction = enemy.GlobalPosition.DirectionTo(enemy.Player.GlobalPosition); 
		enemy.Velocity = direction * enemy.speed;
		enemy.MoveAndSlide();
	}
}
