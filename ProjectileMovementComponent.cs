using Godot;
using System;

public partial class ProjectileMovementComponent : Node
{
	/*
	Asignamos la velocidad del projectil como exportable*/
	[Export] public float Speed = 300f;

	private CharacterBody2D projectile;
	private Vector2 direction = Vector2.Zero;

/*
Nos aseguramos de recibir un padre de CharacterBody2D para el proyectil*/
	public override void _Ready()
	{
		projectile = GetParent<CharacterBody2D>();
		if (projectile == null)
		{
			GD.PrintErr("ProjectileMovementComponent debe ser hijo de un CharacterBody2D");
		}
	}
	/*
	Revisamos constantemente para aplicar el movimiento del proyectil jutunto con aplicarle la velocidad*/
	public override void _PhysicsProcess(double delta)
	{
		if (projectile == null || direction == Vector2.Zero)
			return;

		projectile.Velocity = direction * Speed;
		projectile.MoveAndSlide();
	}
	/*
	Este metodo se llama dentro de otros,
	le mandamos una direccion al proyectil cuando se genera*/
	public void SetDirection(Vector2 dir)
	{
		direction = dir.Normalized();
	}
}
