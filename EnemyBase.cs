using Godot;
using System;

/*
Para añadir a un enemigo, este enemigo debe de heredar EnemyBase
a la hora de crear su script, dado que ahora se manejan spawnData, una vez agregues todos 
los componentes, es necesario que agregues su data dentro de la carpeta. En el scrip de SpawnData viene 
instrucciones.
No hace falta añadir mas scripts a las clases existentes
Puedes simplemente copiar todos los componentes o nodos hijo de un enemigo y aplicarselos
al enemigo nuevo, solo asegurate de cambiar su sprite, su area de colision y
ajustar las estadisticas del enemigo en caso de que sea necesario*/
public partial class EnemyBase : CharacterBody2D
{
	//Atributos base para el padre de los enemigos
	[Export] public float speed = 300f;
	[Export ]public float maxHealth = 100f;
	
	protected Node2D player;
	protected float currentHealth;
	
	public Node2D Player => player;
	//Añadimos los enemigos a el grupo de enemy
	public override void _Ready()
	{
		AddToGroup("enemy");
		currentHealth = maxHealth;
		player = GetTree().GetFirstNodeInGroup("player") as Node2D;
	}
	//Metodo virtual para poder cambiarlo dentro de los enemigos
	public virtual void takeDamage(int dmg)
	{
		currentHealth -= dmg;
		if(currentHealth <= 0){
			die();
		}
	}
	//Metodo simple para remover al enemigo de la queue
	public void die()
	{
		QueueFree();
	}
}
