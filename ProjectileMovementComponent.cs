using Godot;
using System;

public partial class ProjectileMovementComponent : Node
{
	[Export] public float Speed = 300f;

	private CharacterBody2D projectile;
	private Vector2 direction = Vector2.Zero;

	public override void _Ready()
	{
		projectile = GetParent<CharacterBody2D>();

		if (projectile == null)
		{
			GD.PrintErr("ProjectileMovementComponent debe ser hijo de un CharacterBody2D");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (projectile == null || direction == Vector2.Zero)
			return;

		projectile.Velocity = direction * Speed;
		projectile.MoveAndSlide();
	}

	public void SetDirection(Vector2 dir)
	{
		direction = dir.Normalized();
	}
}
