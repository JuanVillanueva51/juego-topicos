using Godot;
using System;

public partial class Player : CharacterBody2D
{
	/*
	Establecemos la velocidad inicial del jugador junto con una variable de
	tipo StatsForPlayer para sus estadisticas*/
	public const float speed = 300.0f;
	private StatsForPlayer stats;
	/*
	Añadimos al jugador al grupo de player y obtenemos la variable de estadisticas
	del componente de StatsForPlayerComponent*/
	public override void _Ready()
	{
		AddToGroup("player");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		stats = GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		if(stats == null){
			GD.Print("No se encontro component");
		}
	}
	public const float Speed = 300.0f;
	private AnimatedSprite2D sprite;

	/*
	Revisamos constantemente el movimiento del jugador, en este caso utilizamos
	La variable de stats para modificar la velocidad final del jugador en base
	a su multiplicador de estadisticas.
	Necesario cambiar nombre de speedExtra a multiplier para evitar confusiones 
	y verificar que funcione bien el multiplicador*/
	public override void _PhysicsProcess(double delta)
	{
	
		float speedFinal = speed;
		if(stats != null){
			speedFinal += stats.speedExtra;
		}
		Vector2 velocity = Velocity;
		// Obtener dirección en X y Y
		
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speedFinal;
			velocity.Y = direction.Y * speedFinal;
		}
		else
		{
			// Desaceleración suave en ambos ejes
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
