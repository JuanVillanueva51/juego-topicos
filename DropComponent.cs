using Godot;
using System;

public partial class DropComponent : Node
{
	//Creamos los valores exportables de los scene junto con los dropChance
	//para cada drop
	[Export] public PackedScene XPORBScene;
	[Export] public PackedScene GOLDCOINScene;
	[Export] public PackedScene HEALTHPACKScene;
	[Export] public float XPDropChance = 1.0f;
	[Export] public float GoldDropChance = 0.5f;
	[Export] public float HealthDropChance = 0.05f;
	//Se crea healthComponent para saber si el enemigo murio
	private HealthComponent health;
	//Inicializamos healthComponent y sincronizamos el evento de 
	//Died con el metodo de Spawn
	public override void _Ready()
	{
		health = GetParent().GetNodeOrNull<HealthComponent>("HealthComponent");

		if (health == null)
		{
			GD.PrintErr("DropComponent necesita HealthComponent");
			return;
		}

		health.Died += Spawn;
	}
	//Ejecutamos el metodo de Create para crear los pickUps
	private void Spawn()
	{
		Vector2 position = GetParent<Node2D>().GlobalPosition;

		Create(XPORBScene, XPDropChance, position);
		Create(GOLDCOINScene, GoldDropChance, position);
		Create(HEALTHPACKScene, HealthDropChance, position);
	}
	//Utilizando la chance inicializada la comparamos con el valor de roll
	//roll nos da un valor random el cual utilizamos para validar que este dentro de la chance
	private void Create(PackedScene scene, float chance, Vector2 position)
	{
		if (scene == null)
			return;

		float roll = (float)GD.RandRange(0.0, 1.0);

		if (roll > chance)
			return;

		Node2D drop = scene.Instantiate<Node2D>();
		drop.GlobalPosition = position;
		//Añadimos el drop a la escena de game
		GetTree().CurrentScene.AddChild(drop);
	}
}
