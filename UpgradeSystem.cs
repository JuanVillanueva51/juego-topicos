using Godot;
using System;

public partial class UpgradeSystem : Node
{
	/*Obtenemos las estadisticas del jugador
	*/
	private StatsForPlayer stats;
	/*
	Añadimos al grupo de upgrade_system para utilizarlo despues.
	La busqueda del player se difiere con CallDeferred porque UpgradeSystem
	es hijo del player, por lo que su _Ready() corre ANTES que el _Ready()
	del player (donde se hace AddToGroup). Con CallDeferred esperamos a que
	todos los _Ready() terminen antes de buscar al player.*/
	public override void _Ready()
	{
		AddToGroup("upgrade_system");
		CallDeferred(MethodName.FindPlayer);
	}
 
	private void FindPlayer()
	{
		var player = GetTree().GetFirstNodeInGroup("player");
 
		if (player == null)
		{
			GD.PrintErr("UpgradeSystem: no se encontro al player");
			return;
		}
 
		stats = player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
 
		if (stats == null)
		{
			GD.PrintErr("UpgradeSystem: no se encontro StatsForPlayerComponent");
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
