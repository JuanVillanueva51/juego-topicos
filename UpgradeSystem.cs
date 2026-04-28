using Godot;
using System;

public partial class UpgradeSystem : Node
{
	/*Obtenemos las estadisticas del jugador
	*/
	private StatsForPlayer stats;
	/*
	Añadimos al grupode upgrade_system para utilizarlo despues y
	obtenemos al jugador junto con sus estadisticas*/
	public override void _Ready()
	{
		AddToGroup("upgrade_system");
		var player = GetTree().GetFirstNodeInGroup("player");

		if (player == null)
		{
			GD.PrintErr("no se encontro al player");
			return;
		}

		stats = player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");

		if (stats == null)
		{
			GD.PrintErr("no se encontro StatsForPlayerComponent");
		}
	}
	/*Metodo simple para aplicar los upgrades,
	simplemente obtenemos la opcion, que es llamada desde otro metodo
	y aplicamos un multiplicador.
	Al finalizar quitamos la pausa de la escena de juego*/
	public void ApplyUpgrade(int option)
	{
		if (stats == null)
			return;

		switch (option)
		{
			case 1:
				stats.addDmg(0.2f);
				break;

			case 2:
				stats.addSpeed(0.15f);
				break;
		}

		GetTree().Paused = false;
	}
}
