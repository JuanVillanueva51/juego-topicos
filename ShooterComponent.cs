using Godot;
using System;

public partial class ShooterComponent : Node
{
	/*
	Metodo de disparo principalmente para los enemigos
	Le damos una escena para el proyectil junto con un cooldown*/
	[Export] public PackedScene proyectilScene;
	[Export] public float shootCooldown = 2f;
	
	private EnemyBase enemy;
	private float timer = 0f;
	/*
	Obtenemos el enemigo como enemyBase*/
	public override void _Ready()
	{
		enemy = GetParent() as EnemyBase;
		if(enemy == null)
		{
			GD.Print("Enemigo necesita ser parte de un padre EnemyBase");
		}
	}
	/*
	Reducimos constantemente el temporizador
	una vez el temporizador llega a 0 o menor a 0 ejecutamos el 
	metodo de shoot y reiniciamos el temporizador*/
	public override void _PhysicsProcess(double delta)
	{
		if(enemy == null || proyectilScene == null) return;
		
		timer -= (float)delta;
		
		if(timer <= 0f)
		{
			shoot();
			timer = shootCooldown;
		}
	}
	/*
	Iniciamos la escena del proyectil, establecemos la posicion en la misma posicion del enemigo
	Dirigimos el vector de direccion hacia la direccion del jugador
	Y aplicamos la direccion a la variable de movimiento dentro del proyectilMovementComponent*/
	public void shoot()
	{
	var proyectil = proyectilScene.Instantiate<Node2D>();
	proyectil.GlobalPosition = enemy.GlobalPosition;
	Vector2 direction = enemy.GlobalPosition.DirectionTo(enemy.Player.GlobalPosition);
	var movement = proyectil.GetNode<ProjectileMovementComponent>("ProjectileMovementComponent");
	movement.SetDirection(direction);
	GetTree().CurrentScene.AddChild(proyectil);
	}
}
