using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		AddToGroup("player");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
