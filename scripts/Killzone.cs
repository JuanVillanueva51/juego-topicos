using Godot;
using System;

public partial class Killzone : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		// Verificamos si el objeto que entró es el jugador
		if (body is Player player)
		{
			GD.Print("-5HP");
			//GetTree().ReloadCurrentScene();
		}
	}
}
