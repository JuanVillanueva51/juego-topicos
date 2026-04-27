using Godot;
using System;

public partial class PlayerShooterComponent : Node
{
	/*
	Obtenemos el data de el arma equipada, esta arma esta colocada inicialmente
	pero luego se actualiza utilizando el ShopUI*/
	[Export] public WeaponData currentWeapon;
	/*
	Utilizamos el point para indicarle desde donde salen las balas
	y le damos el sprite*/
	[Export] public Node2D Point;
	[Export] public Sprite2D weaponSprite;

/*Inicializamos el temporizador del disparo, junto con las 
variables de player y stats*/
	private float timer = 0f;
	private CharacterBody2D player;
	private StatsForPlayer stats;

/*
	Buscamos el jugador y sus stats, en cuanto se encuentren spawneamos el arma*/
	public override void _Ready()
	{
		player = GetParent<CharacterBody2D>();
		stats = player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		SpawnWeapon();
	}

	public override void _Process(double delta)
	{
		/*
		Regresamos si no se encuentran algunos de los componentes necesarios*/
		if (player == null || currentWeapon == null || currentWeapon.ProjectileScene == null || Point == null)
		{
			return;
		}
		timer -= (float)delta;
		/*
		En caso de que el temporizador sea 0 y se presione click izquierdo se puede disparar
		El temporizador es en base al cooldown del arma*/
		if (Input.IsActionPressed("shoot") && timer <= 0f)
		{
			Shoot();
			timer = currentWeapon.cooldown;
		}
	}
	
	private void Shoot()
	{
		/*Obtenemos la cantidad de projectiles, por defecto los projectiles son 1
		para asegurarse de que si la cantidad de disparos se vuelve 0 no se quede
		sin projectiles lo hacemos de un rango maximo de entre 1 y la cantidad de balas del arma*/
		int projectiles = Mathf.Max(1, currentWeapon.shootQuantity);
		/*
		Convertimos el spread a radianes para poder utilizarlo como 
		un area del cual se dispara y usamos StartAngle para definir desde donde inicia*/
		float spreadFinal = Mathf.DegToRad(currentWeapon.spread);
		float startAngle = -spreadFinal / 2f;
		/*
		Repetimos el proceso por la cantidad de projectiles
		El offset lo utilizamos para darle un angulo a cada projectil individual,cuando haya mas de un projectil repartimos el angulo
		de cada projectil antes de spawnearlo. Para que no aparezcan entre si*/
		for (int i = 0; i < projectiles; i++)
		{
			float offSet = 0f;

			if (projectiles > 1)
			{
				float t = (float)i / (projectiles - 1);
				offSet = startAngle + spreadFinal * t;
			}

			SpawnProjectiles(offSet);
		}
	}

	private void SpawnProjectiles(float offSet)
	{
		/*Obtenemos la escena del proyectil como un nodo*/
		Node projectileNode = currentWeapon.ProjectileScene.Instantiate();
		/*
		En caso de que no sea un node2D lo quitamos*/
		if (projectileNode is not Node2D projectile)
		{
			projectileNode.QueueFree();
			return;
		}

		GetTree().CurrentScene.AddChild(projectile);
		/*
		Obtenemos la posicion utilizando Point como posicion hacia la cual disparar
		usamos el offSet para añadirle la rotacion al punto de disparo, de esta
		manera se mostrara el spread y en caso de que tenga una mayor escala la ajustamos*/
		projectile.GlobalPosition = Point.GlobalPosition;
		projectile.GlobalRotation = Point.GlobalRotation + offSet;
		projectile.Scale = Vector2.One * currentWeapon.projectileScale;
		/*
		Calcula la direccion de la bala*/
		Vector2 direction = Vector2.Right.Rotated(projectile.GlobalRotation).Normalized();

		ProjectileMovementComponent movement =
			projectile.GetNodeOrNull<ProjectileMovementComponent>("ProjectileMovementComponent");
		/*
		En este pedazo le mandamos la direccion al projectileMovementComponent
		y luego manadmos el daño al ProjectileDamageComponent*/
		if (movement != null)
		{
			movement.SetDirection(direction);
			movement.Speed = currentWeapon.projectileSpeed;
		}

		ProjectileDamageComponent damage =
			projectile.GetNodeOrNull<ProjectileDamageComponent>("ProjectileDamageComponent");

		if (damage != null)
		{
			damage.Configure(
				currentWeapon.dmg,
				currentWeapon.lifeTime,
				currentWeapon.pierce,
				currentWeapon.pierceCount
			);
		}
	}
		/*
		Equipamos el arma, ese se utiliza dentro de otros componentes 
		para equiparle un nuevo arma*/
	public void EquipWeapon(WeaponData newWeapon)
	{
		currentWeapon = newWeapon;
		SpawnWeapon();
		timer = 0f;
	}
	/*
	Spawneamos el arma cambiando la textura del WeaponSprite del jugador a la textura
	del arma*/
	private void SpawnWeapon()
	{
		if (currentWeapon == null || weaponSprite == null || currentWeapon.WeaponTexture == null)
		{
			return;
		}
			weaponSprite.Texture = currentWeapon.WeaponTexture;
	}
}
