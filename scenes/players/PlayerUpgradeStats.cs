using Godot;
using System;

/// <summary>
/// Almacena las mejoras de estadísticas que el jugador acumula
/// a través de las UpgradeTowers. Son INDEPENDIENTES del WeaponData,
/// por lo que se conservan sin importar qué arma se equipe.
/// 
/// Uso: PlayerShooterComponent debe sumar estas al arma actual al disparar.
/// </summary>
public partial class PlayerUpgradeStats : Node
{
	// ── Mejoras acumuladas (se suman a las del arma equipada) ──────────────
	[Export] public int   BonusDmg           = 0;
	[Export] public float BonusCooldown       = 0f;   // negativo = más rápido
	[Export] public int   BonusShootQuantity  = 0;
	[Export] public float BonusProjectileScale= 0f;
	[Export] public float BonusProjectileSpeed= 0f;
	[Export] public float BonusLifeTime       = 0f;
	[Export] public int   BonusPierceCount    = 0;
	[Export] public float BonusSpread         = 0f;

	public override void _Ready()
	{
		// Nos añadimos a un grupo para que la UpgradeTower nos encuentre fácilmente
		AddToGroup("player_upgrade_stats");
		GD.Print("PlayerUpgradeStats listo.");
	}

	// ── API pública ────────────────────────────────────────────────────────

	public void ApplyUpgrade(UpgradeOption opt)
	{
		switch (opt.StatName)
		{
			case "dmg":             BonusDmg            += (int)opt.Value; break;
			case "cooldown":        BonusCooldown       += opt.Value;      break;
			case "shootQuantity":   BonusShootQuantity  += (int)opt.Value; break;
			case "projectileScale": BonusProjectileScale+= opt.Value;      break;
			case "projectileSpeed": BonusProjectileSpeed+= opt.Value;      break;
			case "lifeTime":        BonusLifeTime       += opt.Value;      break;
			case "pierceCount":     BonusPierceCount    += (int)opt.Value; break;
			case "spread":          BonusSpread         += opt.Value;      break;
			default:
				GD.PrintErr($"PlayerUpgradeStats: stat desconocida '{opt.StatName}'");
				break;
		}
		GD.Print($"[UpgradeTower] Aplicada mejora: +{opt.Value} {opt.StatName}");
	}

}
