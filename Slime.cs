using Godot;
using System;

public partial class Slime : EnemyBase
{
	/*
	Clase de los enemigos, los iniciamos dentro del grupo y aplicamos las animaciones en 
	base  asu velocidad dentro de los ejes*/
	private AnimatedSprite2D sprite;
	public override void _Ready()
	{
		AddToGroup("enemy");
		base._Ready();
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		UpdateAnimation();
	}
	private void UpdateAnimation()
	{
		if (Velocity.Y > 0.2f)
		{
			PlayAnim("Walk");
		}
		else if (Velocity.Y < -0.2f)
		{
			PlayAnim("Walk");
		}
		else if (Velocity.X > 0.2f)
		{
			sprite.FlipH = true;
			PlayAnim("Walk");
		}
		else if (Velocity.X < -0.2f)
		{
			sprite.FlipH = false;
			PlayAnim("Walk");
		}
	}
	private void PlayAnim(string anim)
	{
		if (sprite.Animation != anim)
			sprite.Play(anim);
	}
}
