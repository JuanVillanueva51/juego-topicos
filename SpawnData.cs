using Godot;
using System;

[GlobalClass]
public partial class SpawnData : Resource
{
	/*
	Valores exportables simples, estos se utlizan para crear la data de los enemigos
	Las datas de los enemigos se encuentran dentro de la carpeta de assets/Data
	Dentro de ahi puedes darle click izquierdo dos veces para
	modificar los valores dentro de ellos, estos valores son como los siguientes.
	Para crear data de un nuevo enemigo haz click derecho en la carpeta de Data,
	seleccionada añadir nuevo - recurso - busca SpawnData - dale el nombre del
	enemigo junto con _data
	Ejemplo: bee_data.tres
	*/
	[Export] public PackedScene enemyScene;
	[Export] public int spawnWave = 1;
	[Export] public int peso = 10;
	[Export] public int Difficulty = 1;
}
