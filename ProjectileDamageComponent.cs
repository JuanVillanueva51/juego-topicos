using Godot;
using System;
using System.Collections.Generic;

public partial class ProjectileDamageComponent : Area2D
{
	[Export] public int   dmg               = 1;
	[Export] public float lifeTime          = 3f;
	[Export] public bool  Pierce            = false;
	[Export] public int   pierceCount       = 0;
	[Export] public bool  freeParentOnDestroy = true;
 
	// Valores extra que llegan desde PlayerShooterComponent
	// No se usan directamente aquí, pero se guardan por si
	// algún sistema externo los necesita consultar
	[Export] public int   shootQuantity     = 1;
	[Export] public float projectileScale   = 1f;
	[Export] public float projectileSpeed   = 400f;
	[Export] public float cooldown          = 0.4f;
 
	private double        _timer;
	private int           _piercedEnemies = 0;
	private bool          _destroyed      = false;
	private HashSet<Node> _hitBodies      = new HashSet<Node>();
 
	public override void _Ready()
	{
		_timer = lifeTime;
		BodyEntered += OnBodyEntered;
	}
 
	public override void _Process(double delta)
	{
		if (_destroyed) return;
 
		_timer -= delta;
		if (_timer <= 0)
			DestroyProjectile();
	}
 
	public void Configure(int newDmg, float newLifeTime, bool newPierce, int newPierceCount,
						  int newShootQuantity, float newProjectileScale,
						  float newProjectileSpeed, float newCooldown)
	{
		dmg             = newDmg;
		lifeTime        = newLifeTime;
		Pierce          = newPierce;
		pierceCount     = newPierceCount;
		shootQuantity   = newShootQuantity;
		projectileScale = newProjectileScale;
		projectileSpeed = newProjectileSpeed;
		cooldown        = newCooldown;
 
		_timer          = lifeTime;
		_piercedEnemies = 0;
		_hitBodies.Clear();
		_destroyed      = false;
	}
 
	private void OnBodyEntered(Node2D body)
	{
		if (_destroyed) return;
 
		bool isPlayer = body.IsInGroup("player");
		bool isEnemy  = body.IsInGroup("enemy");
 
		if (!isPlayer && !isEnemy) return;
		if (_hitBodies.Contains(body))  return;
 
		var health = body.GetNodeOrNull<HealthComponent>("HealthComponent");
		if (health == null)
		{
			GD.PrintErr("No se encontró HealthComponent dentro de: " + body.Name);
			return;
		}
 
		_hitBodies.Add(body);
		health.takeDamage(dmg);
 
		var knockback = body.GetNodeOrNull<KnockBackComponent>("KnockBackComponent");
		if (knockback != null)
		{
			Vector2 direction = GlobalPosition.DirectionTo(body.GlobalPosition);
			knockback.knockBack(direction);
		}
 
		if (isPlayer)
		{
			DestroyProjectile();
			return;
		}
 
		if (isEnemy && !Pierce)
		{
			DestroyProjectile();
			return;
		}
 
		if (isEnemy && Pierce)
		{
			_piercedEnemies++;
			if (pierceCount > 0 && _piercedEnemies >= pierceCount)
				DestroyProjectile();
		}
	}
 
	private void DestroyProjectile()
	{
		if (_destroyed) return;
		_destroyed = true;
 
		if (freeParentOnDestroy && GetParent() != null)
			GetParent().QueueFree();
		else
			QueueFree();
	}
}
