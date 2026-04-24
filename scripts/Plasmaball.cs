using Godot;
using System;

public partial class Plasmaball : Area2D
{
	public override void _Ready()
	{
		// Conectar la señal body_entered al método OnBodyEntered
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		// Aquí podrías comprobar si el que entra es el jugador
		if (body is Bee)
		{
			GD.Print("-5HP Enemies");
			//QueueFree();
		}
	}
}
