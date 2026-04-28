using Godot;
using System;

public partial class PlayerShooterComponent : Node
{
	[Export] public WeaponData currentWeapon;
	[Export] public Node2D     Point;
	[Export] public Sprite2D   weaponSprite;

	private float              _timer = 0f;
	private CharacterBody2D    _player;
	private StatsForPlayer     _stats;
	private PlayerUpgradeStats _upgradeStats;   // ← nuevo

	public override void _Ready()
	{
		_player       = GetParent<CharacterBody2D>();
		_stats        = _player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		_upgradeStats = _player.GetNodeOrNull<PlayerUpgradeStats>("PlayerUpgradeStats");   // ← nuevo

		if (_upgradeStats == null)
			GD.PrintErr("PlayerShooterComponent: no se encontró 'PlayerUpgradeStats'. " +
						"Las mejoras de la torre no se aplicarán.");
		SpawnWeapon();
	}

	public override void _Process(double delta)
	{
		if (_player == null || currentWeapon == null ||
			currentWeapon.ProjectileScene == null || Point == null)
			return;

		_timer -= (float)delta;

		// El cooldown final = arma - bonificación acumulada (mínimo 0.05 s)
		float effectiveCooldown = GetEffectiveCooldown();

		if (Input.IsActionPressed("shoot") && _timer <= 0f)
		{
			Shoot();
			_timer = effectiveCooldown;
		}
	}

	// ── Disparo ────────────────────────────────────────────────────────────
	private void Shoot()
	{
		// Cantidad de proyectiles con bonus
		int projectiles = Mathf.Max(1,
			currentWeapon.shootQuantity + (_upgradeStats?.BonusShootQuantity ?? 0));

		float spreadFinal = Mathf.DegToRad(
			currentWeapon.spread + (_upgradeStats?.BonusSpread ?? 0f));

		float startAngle = -spreadFinal / 2f;

		for (int i = 0; i < projectiles; i++)
		{
			float offSet = 0f;
			if (projectiles > 1)
			{
				float t = (float)i / (projectiles - 1);
				offSet  = startAngle + spreadFinal * t;
			}
			SpawnProjectiles(offSet);
		}
	}

	private void SpawnProjectiles(float offSet)
	{
		Node projectileNode = currentWeapon.ProjectileScene.Instantiate();
		if (projectileNode is not Node2D projectile)
		{
			projectileNode.QueueFree();
			return;
		}

		GetTree().CurrentScene.AddChild(projectile);
		projectile.GlobalPosition = Point.GlobalPosition;
		projectile.GlobalRotation = Point.GlobalRotation + offSet;

		// Escala con bonus
		float scaleBonus = _upgradeStats?.BonusProjectileScale ?? 0f;
		projectile.Scale  = Vector2.One * (currentWeapon.projectileScale + scaleBonus);

		Vector2 direction = Vector2.Right.Rotated(projectile.GlobalRotation).Normalized();

		var movement = projectile.GetNodeOrNull<ProjectileMovementComponent>("ProjectileMovementComponent");
		if (movement != null)
		{
			movement.SetDirection(direction);
			movement.Speed = currentWeapon.projectileSpeed +
							 (_upgradeStats?.BonusProjectileSpeed ?? 0f);
		}

		var damage = projectile.GetNodeOrNull<ProjectileDamageComponent>("ProjectileDamageComponent");
		if (damage != null)
		{
			// Daño: base del arma × multiplicador de StatsForPlayer + bonus plano del upgrade
			int finalDmg = Mathf.RoundToInt(
				currentWeapon.dmg * (_stats?.dmgExtra ?? 1f)) +
				(_upgradeStats?.BonusDmg ?? 0);

			damage.Configure(
				finalDmg,
				currentWeapon.lifeTime  + (_upgradeStats?.BonusLifeTime    ?? 0f),
				currentWeapon.pierce,
				currentWeapon.pierceCount + (_upgradeStats?.BonusPierceCount ?? 0),
				currentWeapon.shootQuantity + (_upgradeStats?.BonusShootQuantity ?? 0),
				currentWeapon.projectileScale + (_upgradeStats?.BonusProjectileScale ?? 0f),
				currentWeapon.projectileSpeed + (_upgradeStats?.BonusProjectileSpeed ?? 0f),
				GetEffectiveCooldown()
			);
		}
	}

	// ── Helpers ────────────────────────────────────────────────────────────
	private float GetEffectiveCooldown()
	{
		float cd = currentWeapon.cooldown + (_upgradeStats?.BonusCooldown ?? 0f);
		return Mathf.Max(0.05f, cd);   // evita cooldown negativo
	}

	public void EquipWeapon(WeaponData newWeapon)
	{
		currentWeapon = newWeapon;
		SpawnWeapon();
		_timer = 0f;
	}

	private void SpawnWeapon()
	{
		if (currentWeapon == null || weaponSprite == null || currentWeapon.WeaponTexture == null)
			return;
		weaponSprite.Texture = currentWeapon.WeaponTexture;
	}
}
