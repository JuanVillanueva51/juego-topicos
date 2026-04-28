using Godot;
using System;

public partial class BasePick : Area2D
{
	//Inicializamos la llamada de bodyentered
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
	//Metodo base para ejecutar el PickUp en las clases heredades
	private void OnBodyEntered(Node body)
	{
		if(!body.IsInGroup("player"))
		{
			return;
		}
		PickUp(body);
		QueueFree();
	}
	//Metodo heredado que se reemplaza en las clases hijas
	//Virtual nos permite alterar los contenidos del metodo en clases que exientda
	protected virtual void PickUp(Node player){
		GD.Print($"{GetParent().Name} recogido");
	}
}
