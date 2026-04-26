using Godot;
using System;

public partial class PlayerShooterComponent : Node
{
	/*
	Componente para disparo de jugador
	Le damos como variables la escena del projectil, junto con un cooldown entre
	disparo y un tamaño de proyectil
	Luego creamos las variables de jugador, timer para medir el cooldown y estadisticas*/
	[Export] public PackedScene ProjectileScene;
	[Export] public float ShootCooldown = 0.25f;
	[Export] public float ProjectileScale = 1f;

	private CharacterBody2D player;
	private float timer = 0f;
	private StatsForPlayer stats;
	/*
	Inicializamos tanto el jugador como las estadisticas*/
	public override void _Ready()
	{
		player = GetParent<CharacterBody2D>();
		stats = player.GetNodeOrNull<StatsForPlayer>("StatsForPlayerComponent");
		if (player == null)
		{
			GD.PrintErr("PlayerShooterComponent debe ser hijo del Player");
		}
	}
	/*
	Reducimos el temporizador de cooldown constantemente y checamos si
	el jugador esta presionando la tecla asignada a la accion de shoot.
	En este caso es click izquierdo y confirmamos que el temporizador
	sea menor o igual a 0 para poder disparar
	Una vez se dispara resetamos el timer
	*/
	public override void _Process(double delta)
	{
		if (player == null || ProjectileScene == null)
			return;

		timer -= (float)delta;

		if (Input.IsActionPressed("shoot") && timer <= 0f)
		{
			Shoot();
			timer = ShootCooldown;
		}
	}
	/*
	Metodo para disparo
	Agarramos la escena del proyectil que se toma de la escena exportada
	incialmente
	Inicializamos la posicion del projectil en la posicion del jugador y
	multiplicamos la escala por el multiplicador actual que se haya exportado o modificado
	Luego, tomamos la posicion del mouse utilizando al player para obtener 
	la posicion global del mouse del jugador
	Y dirigimos al player a Esa direccion
	Nota: Modificar este metodo, ya que el projectil aparece encima del jugador y lo empuja
	Y revisar que las estadisticas de daño se esten aplicando correctamente*/
	private void Shoot()
	{
		Node2D projectile = ProjectileScene.Instantiate<Node2D>();
		projectile.GlobalPosition = player.GlobalPosition;
		projectile.Scale = Vector2.One * ProjectileScale;
		Vector2 mousePosition = player.GetGlobalMousePosition();
		Vector2 direction = player.GlobalPosition.DirectionTo(mousePosition);
		/*
		Agarramos el daño del componente de projectileDamageComponent
		y aplicamos el multiplicador de dmgExtra a el damage del proyectil
		Asegurarse de convertir este valor en int para que no de errores*/
		var damage = projectile.GetNodeOrNull<ProjectileDamageComponent>("ProjectileDamageComponent");
		if(damage != null && stats != null){
			damage.dmg = Mathf.RoundToInt(damage.dmg * stats.dmgExtra);
		}
		/*
		Agarramos el movimiento de el projectileMovementComponent para 
		establecer la direccion y añadimos el projectil a la escena en la que se
		encuentra el player*/
		var movement = projectile.GetNodeOrNull<ProjectileMovementComponent>("ProjectileMovementComponent");

		if (movement != null)
		{
			movement.SetDirection(direction);
		}
		else
		{
			GD.PrintErr("El proyectil no tiene ProjectileMovementComponent");
		}
		player.GetTree().CurrentScene.AddChild(projectile);
	}
}
