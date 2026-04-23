using Godot;
using System;

public partial class Bee : CharacterBody2D
{
	[Export] public PackedScene proyectilScene;
	private Node2D player;
	private AnimatedSprite2D sprite;
	private float speed = 100f;
	private float shootTimer = 0f;
	private float shootCooldown = 2f;
	
	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Game/player");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print(player);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (player == null) return;
		shootTimer -= (float)delta;
		if (shootTimer <= 0f)
		{
		Shoot();
		shootTimer = shootCooldown;
		}
		Vector2 direccion = (player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = direccion * speed;
		MoveAndSlide();
		UpdateAnimation(direccion);
	}
	void Shoot()
	{
		if(proyectilScene == null) return;
		var proyectil = proyectilScene.Instantiate<Node2D>();
		proyectil.GlobalPosition = GlobalPosition;
		GetTree().CurrentScene.AddChild(proyectil);
	}

	void UpdateAnimation(Vector2 direccion)
	{
		if (direccion.Y > 0.2)
		{
			PlayAnim("walk_DOWN");
		}
		else if (direccion.Y < -0.2)
		{
			PlayAnim("walk_UP");
		}
		else if (direccion.X > 0.2)
		{
			sprite.FlipH = true;
			PlayAnim("walk_LEFT");
		}
		else if (direccion.X < -0.2)
		{
			sprite.FlipH = false;
			PlayAnim("walk_LEFT");
		}
	}

	void PlayAnim(string anim)
	{
		if (sprite.Animation != anim)
			sprite.Play(anim);
	}
}
