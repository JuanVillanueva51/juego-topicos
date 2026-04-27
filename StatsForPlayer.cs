using Godot;
using System;

public partial class StatsForPlayer : Node
{
	/*
	Inicializamos las estadisticas del jugador como variables exportables
	Nota: Añadir mas estadisticas para los multiplicadores*/
	[Export] public int level = 1;
	[Export] public int xp = 0;
	[Export] public int xpForNextLevel = 100;
	[Export] public int gold = 0;
	[Export] public float dmgExtra = 1f;
	[Export] public float speedExtra = 1f;
	/*
	Metodo para añadir expriencia al jugador, este metodo se llama dentro de otras clases
	Si la experiencia es mayor o igual para subir al siguiente nivel llamamos
	el metodo de levelUp
	am hace referencia a amount, que es la cantida de xp que llegue*/
	public void addXP(int am){
		xp += am;
		GD.Print($"Experiencia actual: {xp}/{xpForNextLevel}");
		if(xp >= xpForNextLevel)
		{
			levelUp();
		}
	}
	/*
	Metodo para añadir oro al jugador
	mGold hace referencia a la cantidad de oro otorgada*/
	public void addGold(int mGold)
	{
		gold += mGold;
		GD.Print($"Gold: {gold}");
	}
	/*
	Metodo de levelUp, establecemos la xp en 0 para reiniciarla, subimos el level.
	y aplicamos un multiplicador de 0.5 para que el siguiente nivel cueste mas
	Buscamos el componente de levelUpUI para llamar a el metodo de upgrades
	y pausamos la escena del juego en lo que se selecciona un upgrade
	Nota: Es necesario cambiar este metodo para que la experiencia sobrante del jugador
	se guarde, ya que aqui si se pasa de mas xp de la cual es necesaria para
	subir de nivel esta experiencia no se guarda y se resetea a 0*/
	private void levelUp()
	{
		level++;
		xp = 0;
		xpForNextLevel += (int)(xpForNextLevel * 0.5f);
		GD.Print("YOU LEVELED UP!!!");
		var levelUpUI = GetTree().GetFirstNodeInGroup("levelUpUI");
		if(levelUpUI != null)
		{
			levelUpUI.Call("Upgrades");
			GetTree().Paused = true;
		}
	}
	/*
	Estos 2 metodos se llaman dentro de otros metodos, se utilizand para aplicar el multiplicador
	de daño y velocidad*/
	public void addDmg(float extra)
	{
		dmgExtra += extra;
		GD.Print($"Daño aumentado a {dmgExtra}");
	}
	public void addSpeed(float extra){
		speedExtra += extra;
		GD.Print($"Velocidad aumentada a {speedExtra}");
	}
	public bool spendGold(int amount)
	{
	if (gold < amount)
		return false;
	gold -= amount;
	return true;
	}
	
}
