using Godot;
using System;

public partial class Bee : EnemyBase
{
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
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
			PlayAnim("walk_DOWN");
		}
		else if (Velocity.Y < -0.2f)
		{
			PlayAnim("walk_UP");
		}
		else if (Velocity.X > 0.2f)
		{
			sprite.FlipH = true;
			PlayAnim("walk_LEFT");
		}
		else if (Velocity.X < -0.2f)
		{
			sprite.FlipH = false;
			PlayAnim("walk_LEFT");
		}
	}

	private void PlayAnim(string anim)
	{
		if (sprite.Animation != anim)
			sprite.Play(anim);
	}
}
