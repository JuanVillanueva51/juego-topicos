using Godot;
using System;

public partial class EnemyBase : CharacterBody2D
{
	[Export] public float speed = 300f;
	[Export ]public float maxHealth = 100f;
	
	protected Node2D player;
	protected float currentHealth;
	
	public Node2D Player => player;
	public override void _Ready()
	{
		currentHealth = maxHealth;
		player = GetTree().GetFirstNodeInGroup("player") as Node2D;
				if (player == null)
		{
			GD.PrintErr("No se encontró ningún nodo en el grupo 'player'");
		}
	}
	public virtual void takeDamage(int dmg)
	{
		currentHealth -= dmg;
		if(currentHealth <= 0){
			die();
		}
	}
	
	public void die()
	{
		QueueFree();
	}
}
