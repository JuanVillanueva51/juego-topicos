using Godot;
using System;
//NO UTILIZADO DENTRO DE EL FLUJO DEL CODIGO
public partial class EnemyDeathComponent : Node
{
	[Export] public int xpReward = 5;
	[Export] public int goldReward = 1;
	private HealthComponent health;
	public override void _Ready()
	{
		health = GetParent().GetNodeOrNull<HealthComponent>("HealthComponent");
		if (health == null)
		{
			GD.Print("Necesita un HealthComponent");
			return;
		}
		health.Died += EnemyDeath;
	}
	private void EnemyDeath()
	{
		var player = GetTree().GetFirstNodeInGroup("player");
		if (player == null)
		{
			GD.Print("Jugador no encontrado");
			return;
		}
		var stats = player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		if (stats != null)
		{
			stats.addXP(xpReward);
			stats.addGold(goldReward);
		}
	}
}
