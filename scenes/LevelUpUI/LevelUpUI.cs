using Godot;
using System;

public partial class LevelUpUI : CanvasLayer
{
	/*
	Añadimos la UI a un grupo y establecemos su visibilidad inicial en falso
	*/
	public override void _Ready(){
		AddToGroup("levelUpUI");
		Visible = false;
		
		GetNode<Button>("VBoxContainer/Button").Pressed += () => SelectUpgrades(1);
		GetNode<Button>("VBoxContainer/Button2").Pressed += () => SelectUpgrades(2);
		GetNode<Button>("VBoxContainer/Button3").Pressed += () => SelectUpgrades(3);

	}
	/*
	Metodo para conocer si estamos entrando en las upgrades*/
	public void Upgrades()
	{
		Visible = true;
		GD.Print("Iniciando mejoras");
	}
	/*
	Checaremos constantemente en caso de que la pantalla del UI este visible
	Si se esta presionando uno de los botones asignados para los upgrades*/
public override void _Process(double delta)
{
	if (!Visible) return;
	if (Input.IsActionJustPressed("ui_1"))
		SelectUpgrades(1);
	if (Input.IsActionJustPressed("ui_2"))
		SelectUpgrades(2);
	if (Input.IsActionJustPressed("ui_3"))
		SelectUpgrades(3);
}
	/*
	Entramos aqui gracias a el process y tomamos la opcion seleccionada dentro del mismo
	Una vez hecho esto, tomamos el componente de upgradeSystem
	y llamamos al metood de ApplyUpgrade dentro de upgradeSystem
	Restauramos el juego estableciendo la UI en oculta y des-pausando el juego*/
	public void SelectUpgrades(int opt){
		GD.Print($"Mejora seleccionada {opt}");
		var upgradeSystem = GetTree().GetFirstNodeInGroup("upgrade_system");
		if (upgradeSystem != null)
		{
		upgradeSystem.Call("ApplyUpgrade", opt);
		}
		Visible = false;
		GetTree().Paused = false;
	}
}
