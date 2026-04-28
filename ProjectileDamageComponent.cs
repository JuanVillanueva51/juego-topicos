using Godot;
using System;

public partial class ProjectileDamageComponent : Area2D
{
	/*
	Exportamos los valores de daño y lifeTime, lifeTime se usa para validar que
	tanto tiempo de vida tiene el proyectil antes de desaparecer
	Nota: Creo que no esta funcionando correctamente*/
	[Export] public int dmg = 1;
	[Export] public float lifeTime = 3f;

	private double timer;

	/*
	Al iniciar establecemos el temporizador con el valor de lifeTime y
	asignamos BodyEntered para ejecutar el metodo de OnBodyEntered*/
	public override void _Ready()
	{
		timer = lifeTime;
		BodyEntered += OnBodyEntered;
	}
	/*
	Reducimos el temporizador constantemente y en caso de que 
	sea menor o igual a 0, quitamos el proyectil de la queue*/
	public override void _Process(double delta)
	{
		timer -= delta;

		if (timer <= 0)
		{
			QueueFree();
		}
	}
	/*
	Se ejecuta en cuanto se encuentra bodyEntered
	Comprobamos si el body es parte del grupo de enemy o player
	Agarramos la vida del body que entro para ejecutar el metodo de takeDamage
	y haremos lo mismo para knockback en caso de que tenga el componente de knockback
	y le mandamos la direccion para aplicar el knocback y lo quitamos de la queue*/
	private void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player") && !body.IsInGroup("enemy"))
			return;

		var health = body.GetNodeOrNull<HealthComponent>("HealthComponent");

		if (health == null)
		{
			GD.PrintErr("No se encontró HealthComponent dentro de: " + body.Name);
			return;
		}

		health.takeDamage(dmg);
		var knockback = body.GetNodeOrNull<KnockBackComponent>("KnockBackComponent");
		if (knockback != null)
		{
			Vector2 direction = GlobalPosition.DirectionTo(((Node2D)body).GlobalPosition);
			knockback.knockBack(direction);
		}
		QueueFree();
	}
}
