using Godot;
using System;

public partial class HealthComponent : Node
{
	[Export] public int maxHealth = 3;
	
	private int currentHealth;
	
	public override void _Ready(){
		currentHealth = maxHealth;
	}
	public void takeDamage(int dmg){
		currentHealth -= dmg;
		GD.Print($"{GetParent().Name} recibio daño: {dmg}  Vida restante {currentHealth}");
		if(currentHealth <=0){
			die();
		}
		
	}
	public void die(){
	GD.Print($"{GetParent().Name} murio");
	GetParent().QueueFree();
	GetTree().ChangeSceneToFile("res://scenes/Game Over/Game Over.tscn");
	}
}
