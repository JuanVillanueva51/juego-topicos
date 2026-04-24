using Godot;
using System;

public partial class ShooterComponent : Node
{
	[Export] public PackedScene proyectilScene;
	[Export] public float shootCooldown = 2f;
	
	private EnemyBase enemy;
	private float timer = 0f;
	
	public override void _Ready()
	{
		enemy = GetParent() as EnemyBase;
		if(enemy == null)
		{
			GD.Print("Enemigo necesita ser parte de un padre EnemyBase");
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if(enemy == null || proyectilScene == null) return;
		
		timer -= (float)delta;
		
		if(timer <= 0f)
		{
			shoot();
			timer = shootCooldown;
		}
	}
	public void shoot()
	{
			var proyectil = proyectilScene.Instantiate<Node2D>();
	proyectil.GlobalPosition = enemy.GlobalPosition;
	Vector2 direction = enemy.GlobalPosition.DirectionTo(enemy.Player.GlobalPosition);
	var movement = proyectil.GetNode<ProjectileMovementComponent>("ProjectileMovementComponent");
	movement.SetDirection(direction);
	GetTree().CurrentScene.AddChild(proyectil);
	}
}
