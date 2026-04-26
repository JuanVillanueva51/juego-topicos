using Godot;
using System;

public partial class Player : CharacterBody2D
{
<<<<<<< HEAD
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
		stats = GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		if(stats == null){
			GD.Print("No se encontro component");
		}
=======
	public const float Speed = 300.0f;
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		AddToGroup("player");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
>>>>>>> a8365dfa1b527da306c5d74cdce44a7d369954ef
	}
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
<<<<<<< HEAD
		Vector2 direction = Input.GetVector("Move_LEFT", "Move_RIGHT", "Move_UP", "Move_DOWN");
		
=======
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

>>>>>>> a8365dfa1b527da306c5d74cdce44a7d369954ef
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
		
		UpdateAnimation();
	}
	private void UpdateAnimation()
	{
		if (Velocity.Y > 0.2f)
		{
			PlayAnim("walk_Down");
		}
		else if (Velocity.Y < -0.2f)
		{
			PlayAnim("walk_Up");
		}
		else if (Velocity.X > 0.2f)
		{
			sprite.FlipH = true;
			PlayAnim("walk_Left_Down");
		}
		else if (Velocity.X < -0.2f)
		{
			sprite.FlipH = true;
			PlayAnim("walk_Right_Down");
		}
		if (Velocity.X > 0.2f && Velocity.Y > 0.2f){
			sprite.FlipH = false;
			PlayAnim("walk_Right_Down");
		}
		else if (Velocity.X < -0.2f && Velocity.Y > 0.2f){
			sprite.FlipH = false;
			PlayAnim("walk_Left_Down");
		}
		else if (Velocity.X > 0.2f && Velocity.Y <  -0.2f){
			sprite.FlipH = false;
			PlayAnim("walk_Right_Up");
		}
		else if (Velocity.X < -0.2f && Velocity.Y < -0.2f){
			sprite.FlipH = false;
			PlayAnim("walk_Left_Up");
		}
	}
	
	private void PlayAnim(string anim)
	{
		if (sprite.Animation != anim)
			sprite.Play(anim);
	}
}
