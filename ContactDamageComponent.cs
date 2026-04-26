using Godot;
using System;

public partial class ContactDamageComponent : Area2D
{
	//Inicializamos valores exportables 
	//Estos nos permitiran asignarles valores desde el inespector
	//Inicializamos daño de contacto y cooldown de daño como exportables
	[Export] public int contactDamage = 1;
	[Export] public float dmgCooldown = 0.5f;
	
	private double cooldown = 0;
	//Aseguramos de ejecutar el metodo OnBodyEntered cuando se detecte un body
	public override void _Ready()
	{
		GD.Print("ENTERED READY OF CONTANCT DAMAGE");
		BodyEntered += OnBodyEntered;
	}
	//Vamos reduciendo el cooldown hasta llevarlo a 0 o menor a 0
	public override void _Process(double delta)
	{
		if(cooldown > 0)
		{
			cooldown -= delta;
		}
	}
	//buscamos que el body que entro sea el del jugador
	//En caso de que el body sea del jugador buscamos su componente de HealthComponent
	//Ejecutamos el metodo de takeDamage en health para hacerle daño al jugador e 
	//Regresamos el cooldown al valor inicial
	private void OnBodyEntered(Node body)
	{
		if(cooldown > 0) return;
		if(body.IsInGroup("player"))
		{
			if(body is Player player)
			{
				var health = body.GetNode<HealthComponent>("HealthComponent");
				GD.Print("Daño hecho por contacto: " + contactDamage);
				health.takeDamage(contactDamage);
				cooldown = dmgCooldown;
			}
		}
	}
}
