using Godot;
using System;

public partial class Killzone : Area2D
{
	public override void _Ready()
	{
		// Conectar la señal body_entered al método OnBodyEntered
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		// Verificamos si el objeto que entró es el jugador
		if (body is Player player)
		{
			GD.Print("Jugador entró en la KillZone");

			// Aquí decides qué hacer:
			// 1. Reiniciar la escena
			GetTree().ReloadCurrentScene();

			// 2. O llamar a un método en el jugador, por ejemplo:
			// player.Die();
		}
	}
}
