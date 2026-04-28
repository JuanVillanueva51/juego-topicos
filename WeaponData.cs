using Godot;
using System;

[GlobalClass]
public partial class WeaponData : Resource
{
	/*Establecemos los valores de las armas*/
	//Nombre del arma
	[Export] public string name = "Placeholder";
	//Escena del projectil
	[Export] public PackedScene ProjectileScene;
	//Textura del arma
	[Export] public Texture2D WeaponTexture;
	//Cooldown, que es equivalente a velocidad de disparo
	[Export] public float cooldown = 0.4f;
	//Daño
	[Export] public int dmg = 1;
	//Velocidad del proyectil
	[Export] public float projectileSpeed = 400f;
	//Tamaño del proyectil
	[Export] public float projectileScale = 1f;
	//Tiempo de vida del proyectil
	[Export] public float lifeTime = 2f;
	//Cantida de proyectiles
	[Export] public int shootQuantity = 1;
	//Esparcimiento de las balas
	[Export] public float spread = 0f;
	//Define si puede penetrar enemigos o no
	[Export] public bool pierce = false;
	//Define cuantos enemigos puede penetrar
	[Export] public int pierceCount = 0;
	//Define precio
	[Export] public int price = 0;
}
