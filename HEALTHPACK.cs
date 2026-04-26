using Godot;
using System;

public partial class HEALTHPACK : BasePick
{
	/*Usando BasePick como base, creamos la clase de HealthPack
	Le damos como variable exportable la cantidad de vida que curara
	*/
	[Export] public int healthAplied = 5;
	/*Sobrescribimos el metodo de PickUp del nodo BasePick
	Agarramos el componente de HealthComponent del jugador y
	iniciamos el metodo de Heal para curar al jugador la vida de la variable exportable
	*/
	protected override void PickUp(Node player)
	{
		var health = player.GetNodeOrNull<HealthComponent>("HealthComponent");
		health.Heal(healthAplied);
		GD.Print($"{healthAplied} curado. Vida actual {health.currentHealth}");
	}
}
