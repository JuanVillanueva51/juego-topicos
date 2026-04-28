using Godot;
using System;
using System.Collections.Generic;

public partial class ProjectileDamageComponent : Area2D
{
	/*Estos valores se reemplazan con el weaponData*/
	/*Daño base del projectil*/
	[Export] public int dmg = 1;

	/*
	Tiempo de vida*/
	[Export] public float lifeTime = 3f;

	/*
	Define si el projectil puede atravesar al enemigo*/
	[Export] public bool Pierce = false;

	/*
	Define cuantos enemigos puede atravesar
	*/
	[Export] public int pierceCount = 0;

	/*
	Aqui es como destruimos al proyectil*/
	[Export] public bool freeParentOnDestroy = true;

	private double timer;
	private int piercedEnemies = 0;
	private bool destroyed = false;

	/*
	Guardamos los enemigos golpeados para que no se golpee a un mismo enemigo
	varias veces*/
	private HashSet<Node> hitBodies = new HashSet<Node>();
	/*
	Inicializamos el lifeTime y le asignamos el metodo al
	BodyEntered*/
	public override void _Ready()
	{
		timer = lifeTime;
		BodyEntered += OnBodyEntered;
	}
	
	/*
	Metodo de proceso
	Si el projectil esta destruido no entramos
	reducimos el temporizador sontantemente
	y destruimos el projectil cuando acabe su tiempo*/
	public override void _Process(double delta)
	{
		if (destroyed)
			return;

		timer -= delta;

		if (timer <= 0)
		{
			DestroyProjectile();
		}
	}

	/*
	Aqui guardamos la informacion que se mandara desde un weaponData.
	La informacion se manda desde el shopUI
	*/
	public void Configure(int newDmg, float newLifeTime, bool newPierce, int newPierceCount)
	{
		dmg = newDmg;
		lifeTime = newLifeTime;
		Pierce = newPierce;
		pierceCount = newPierceCount;

		timer = lifeTime;
		piercedEnemies = 0;
		hitBodies.Clear();
		destroyed = false;
	}
	/*
	Cuando entra el cuerpo
	Si el cuerpo esta destruido, nos regresamos*/
	private void OnBodyEntered(Node2D body)
	{
		if (destroyed)
			return;
		/*
		Buscamos si el cuerpo que entro es parte del grupo de enemigos o jugadores
		Obtenemos el healthComponent y KnockBackComponent si se encuentran*/
		bool isPlayer = body.IsInGroup("player");
		bool isEnemy = body.IsInGroup("enemy");

		if (!isPlayer && !isEnemy)
			return;

		if (hitBodies.Contains(body))
			return;

		var health = body.GetNodeOrNull<HealthComponent>("HealthComponent");

		if (health == null)
		{
			GD.PrintErr("No se encontró HealthComponent dentro de: " + body.Name);
			return;
		}
		/*
		Una vez el cuerpo es golpeado, lo añadimos a la lista de cuerpos golpeados*/
		hitBodies.Add(body);

		health.takeDamage(dmg);

		var knockback = body.GetNodeOrNull<KnockBackComponent>("KnockBackComponent");

		if (knockback != null)
		{
			Vector2 direction = GlobalPosition.DirectionTo(body.GlobalPosition);
			knockback.knockBack(direction);
		}

		/*
		Si el projectil golpea al jugador, el projectil desaparece*/
		if (isPlayer)
		{
			DestroyProjectile();
			return;
		}

		/*
		Similar al anterior, si el projectil golpea a un enemigo, pero en este caso
		si no tiene la propiedad de pierce, el projectil desaparece */
		if (isEnemy && !Pierce)
		{
			DestroyProjectile();
			return;
		}

		/*
		Si atraviesa a un enemigo y tiene la propiedad de pierce, lo guardamos dentro
		de piercedEnemies.
		Luego verificamos que pierceCount osea la cantidad de enemigos que puede atravesar
		sea menor o igual a la propiedad de piercedEnemies, esto significa que ya
		no puede atravesar mas enemigos y se destruye*/
		if (isEnemy && Pierce)
		{
			piercedEnemies++;
			if (pierceCount > 0 && piercedEnemies >= pierceCount)
			{
				DestroyProjectile();
			}
		}
	}
	/*
	Si esta destruido, retornamos
	Convertimos destruido a true en caso de que se continue
	Si obtenemos el parent y freeparentONdestroy entonces
	liberamos al parent de la queue 
	y si no destruimos el actual de la queue*/
	private void DestroyProjectile()
	{
		if (destroyed)
			return;

		destroyed = true;

		if (freeParentOnDestroy && GetParent() != null)
		{
			GetParent().QueueFree();
		}
		else
		{
			QueueFree();
		}
	}
}
