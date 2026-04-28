using Godot;
using System;

public partial class XPORB : BasePick
{
	/*
	Le damos la xp que otorgara cada orbe de experiencia
	y luego utilizando los stats del jugador llamaremos al 
	metodo de añadir experiencia y aplicaremos la
	experiencia que tiene el orbe*/
	[Export] public int xpAplied = 5;
	
	protected override void PickUp(Node player)
	{
		var stats = player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		
		stats.addXP(xpAplied);
		GD.Print($"{xpAplied} experiencia recogida.");
	}
}
