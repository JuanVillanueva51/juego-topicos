using Godot;
using System;

public partial class HealthComponent : Node
{
	//Creamos variables tipo Signal para manejar los eventos 
	//en los cuales se lastima a la entidad o la entidad muere (enemigo o jugador)
	//Establecemos el valor de export para darle vida maxima
	[Signal] public delegate void DamagedEventHandler(int dmg);
	[Signal] public delegate void DiedEventHandler();
	[Export] public int maxHealth = 3;
	
	public int currentHealth;
	
	//Inicialilzamos la vida actual asignandole la vida maxima
	public override void _Ready(){
		currentHealth = maxHealth;
	}
	/*Quitamos vida de la entidad basandonos en el daño que llega
	Y luego emitimos una señal para el evento de Damaged, e mandamos el daño como variable
	Si la vida actual de la entidad es menor o igual a 0 activamos el evento de die
	*/
	public void takeDamage(int dmg){
		currentHealth -= dmg;
		GD.Print($"{GetParent().Name} recibio daño: {dmg}  Vida restante {currentHealth}");
		EmitSignal(SignalName.Damaged, dmg);
		if(currentHealth <=0){
			die();
		}
		
	}
	//Metodo para curar la entidad, no manda ninguna señal, solo cura
	public void Heal(int cura)
	{
		currentHealth += cura;
		if(currentHealth > maxHealth) currentHealth = maxHealth;
		GD.Print($"{GetParent().Name} Se curo la cantidad de: {cura}");
	}
	/*Metodo de muerte, mandamos una señal para el evento de Died
	y quitamos la entidad de la queue
	*/
	public void die(){
<<<<<<< HEAD
		GD.Print($"{GetParent().Name} murio");
		EmitSignal(SignalName.Died);
		GetParent().QueueFree();
=======
	GD.Print($"{GetParent().Name} murio");
	GetParent().QueueFree();
	GetTree().ChangeSceneToFile("res://scenes/Game Over/Game Over.tscn");
>>>>>>> a8365dfa1b527da306c5d74cdce44a7d369954ef
	}
}
