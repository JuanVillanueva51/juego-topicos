using Godot;
using System;

public partial class ContactDamageComponent : Area2D
{
	[Export] public int contactDamage = 1;
	[Export] public float dmgCooldown = 0.5f;
	
	private double cooldown = 0;
	
	public override void _Ready()
	{
		GD.Print("ENTERED READY OF CONTANCT DAMAGE");
		BodyEntered += OnBodyEntered;
	}
	public override void _Process(double delta)
	{
		if(cooldown > 0)
		{
			cooldown -= delta;
		}
	}
	private void OnBodyEntered(Node body)
	{
		if(cooldown > 0) return;
		if(body.IsInGroup("player"))
		{
			if(body is Player player)
			{
				var health = body.GetNode<HealthComponent>("HealthComponent");
				GD.Print("Daño hecho: " + contactDamage);
				health.takeDamage(contactDamage);
				cooldown = dmgCooldown;
			}
		}
	}
}
