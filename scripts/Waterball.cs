using Godot;
using System;

public partial class Waterball : CharacterBody2D
{
	private Node2D player;
	private AnimatedSprite2D sprite;
	private float speed = 300f;
	Vector2 direccion;

	public override void _Ready()
	{
		player = GetNode<Node2D>("/root/Game/player");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		direccion = (player.GlobalPosition - GlobalPosition).Normalized();
	}
	public override void _PhysicsProcess(double delta)
	{
		if (player == null) return;
		Velocity = direccion * speed;
		MoveAndSlide();
	}
}
