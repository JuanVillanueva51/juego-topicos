using Godot;
using System;

public partial class FollowPlayerComponent : Node
{
	//Tomamos un enemigo utilizando EnemyBase
	private EnemyBase enemy;
	//Obtenemos el enemigo utilizando el GetParent
	public override void _Ready()
	{
		enemy = GetParent<EnemyBase>();
		if (enemy == null)
		{
			GD.PrintErr("FollowPlayerComponent debe ser hijo directo de un EnemyBase.");
		}
	}
	//Creamos un vector de direccion y lo direccionamos a la posicion
	//del jugador asignado al enemigo
	public override void _PhysicsProcess(double delta)
	{
		if(enemy == null || enemy.Player == null) return;
		
		Vector2 direction = enemy.GlobalPosition.DirectionTo(enemy.Player.GlobalPosition); 
		enemy.Velocity = direction * enemy.speed;
		enemy.MoveAndSlide();
	}
}
