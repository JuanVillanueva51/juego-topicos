using Godot;
using System;

public partial class GOLDCOIN : BasePick
{
	//Creamos un valor exportable para asignar la cantidad de dinero que dara
	[Export] public int goldAplied = 1;
	//Traemos el metodo de PickUp de la clase base y aplicamos el oro a las estadisticas del jugador
	protected override void PickUp(Node player)
	{
		var stats = player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		if (stats == null)
		{
			GD.PrintErr("GOLDCOIN: No se encontró StatsForPlayerComponent en el jugador.");
			return;
		}
		stats.addGold(goldAplied);
		GD.Print($"{goldAplied} dinero recogido. Dinero actual {stats.gold}");
	}
}
