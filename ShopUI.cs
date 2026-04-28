using Godot;
using System;

public partial class ShopUI : Control
{
	/* Valores exportables*/
	//WaveManager para permitir solo abrir la tienda durante el descanso
	[Export] public WaveManager waveManager;
	//Player shooter para modificar en base al weaponData que se obtenga de la
	//tienda
	[Export] public PlayerShooterComponent playerShooter;
	//Stats para hacer lo mismo
	[Export] public StatsForPlayer stats;
	//Aqui mostraremos en un label el tiempo restante de descanso
	[Export] public Label RestTimerLabel;
	//Mostraremos en un label el boton para abrir la tienda
	[Export] public Label OpenShopLabel;
	//Para poder abrir el panel de la tienda
	[Export] public Panel ShopPanel;
	//Mostraremos el weaponData de cada arma dentro del container
	[Export] public VBoxContainer WeaponListContainer;
	//Boton para cerrar la tienda
	[Export] public Button CloseButton;
	//Obtenemos la data de todas las armas
	[Export] public Godot.Collections.Array<WeaponData> weapons = new();

	private bool shopOpen = false;

	/*
	Establecemos todo como no visible al inicio y abrimos la tienda*/
	public override void _Ready()
	{
		ShopPanel.Visible = false;
		OpenShopLabel.Visible = false;
		RestTimerLabel.Visible = false;

		if (CloseButton != null)
		{
			CloseButton.Pressed += CloseShop;
		}
		AddToGroup("player");
		BuildShop();
	}

	public override void _Process(double delta)
	{
		if (waveManager == null)
			return;
		/*
		En el descanso activamos todos los labels y permitimos abrir la tienda*/
		if (waveManager.descansando)
		{
			RestTimerLabel.Visible = true;
			OpenShopLabel.Visible = !shopOpen;

			RestTimerLabel.Text = "Descanso: " + Mathf.CeilToInt(waveManager.descansoTimer).ToString() + "s";
			OpenShopLabel.Text = "Presiona B para abrir la tienda";
			/*
			Abrimos la tienda al presionar el boton asignado en el mapeo de telcas*/
			if (Input.IsActionJustPressed("shop"))
			{
				ToggleShop();
			}
		}
		/*
		Cuando el descanso acabe, entonces quitamos todo de la pantalla*/
		else
		{
			RestTimerLabel.Visible = false;
			OpenShopLabel.Visible = false;

			if (shopOpen)
			{
				CloseShop();
			}
		}
	}
	
	private void ToggleShop()
	{
		if (waveManager == null || !waveManager.descansando)
			return;
		/*
		Activamos la visibilidad de la tienda*/
		shopOpen = !shopOpen;
		ShopPanel.Visible = shopOpen;
		OpenShopLabel.Visible = !shopOpen;
	}
	/*
	Cerramos la tienda*/
	private void CloseShop()
	{
		shopOpen = false;
		ShopPanel.Visible = false;
	}
	/*
	Construimos la informacion de la tienda*/
	private void BuildShop()
	{
		if (WeaponListContainer == null)
			return;
		foreach (Node child in WeaponListContainer.GetChildren())
		{
			child.QueueFree();
		}
		/*
		Creamos las armas utilizando WeaponData*/
		foreach (WeaponData weapon in weapons)
		{
			CreateWeaponRow(weapon);
		}
	}

	private void CreateWeaponRow(WeaponData weapon)
	{
		if (weapon == null)
			return;

		HBoxContainer row = new HBoxContainer();
		/*
		Empezamos a asignar los valores dentro del weapon data para cada weapon*/
		Label weaponLabel = new Label();
		weaponLabel.Text = weapon.name
			+ " | Precio: " + weapon.price
			+ " | Daño: " + weapon.dmg
			+ " | Cooldown: " + weapon.cooldown
			+ " | Proyectiles: " + weapon.shootQuantity;
		Button buyButton = new Button();
		buyButton.Text = "Comprar";

		buyButton.Pressed += () => TryBuyWeapon(weapon);

		row.AddChild(weaponLabel);
		row.AddChild(buyButton);

		WeaponListContainer.AddChild(row);
	}
	/*
	Puñado de validaciones para permitir comprar las armas*/
	private void TryBuyWeapon(WeaponData weapon)
	{
		if (weapon == null)
			return;

		if (waveManager == null || !waveManager.descansando)
		{
			GD.Print("Solo puedes comprar durante el descanso.");
			return;
		}

		if (playerShooter == null)
		{
			GD.PrintErr("No se asignó PlayerShooterComponent en ShopUI.");
			return;
		}

		if (stats == null)
		{
			GD.PrintErr("No se asignó StatsForPlayer en ShopUI.");
			return;
		}

		if (!stats.spendGold(weapon.price))
		{
			GD.Print("No tienes suficiente oro para comprar: " + weapon.name);
			return;
		}
		/*
		Llamamos al equiWeapon de playerShooter*/
		playerShooter.EquipWeapon(weapon);
		GD.Print("Compraste y equipaste: " + weapon.name);
	}
}
