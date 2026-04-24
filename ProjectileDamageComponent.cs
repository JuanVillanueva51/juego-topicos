using Godot;
using System;

public partial class ProjectileDamageComponent : Area2D
{
	[Export] public int dmg = 1;
	[Export] public float lifeTime = 3f;
	
	private double timer;
	
	public override void _Ready()
	{
		timer = lifeTime;
		BodyEntered += OnBodyEntered;
	}
	
	public override void _Process(double delta)
	{
		timer -= delta;
		if(timer <= 0)
		{
			QueueFree();
		}
	}
	private void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player"))
			return;
 		if (body is Player player)
 		{
			
			var health = body.GetNode<HealthComponent>("HealthComponent");
				if (health == null)
	{
		GD.PrintErr("No se encontró HealthComponent dentro de: " + body.Name);
		return;
	}

	GD.Print("Health encontrado");
 			 health.takeDamage(dmg);
 		}
 		QueueFree();
	}
}
