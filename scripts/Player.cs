using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	
	public override void _Ready()
	{
		AddToGroup("player");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Obtener dirección en X y Y
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			// Desaceleración suave en ambos ejes
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
