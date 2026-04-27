using Godot;
using System;

public partial class StatsUpZone : Area2D
{
	private Timer stayTimer;
	private Control upgradeMenu;

	public override void _Ready()
	{
		GD.Print("Pillar");
		
		stayTimer = GetNode<Timer>("Timer1");
		// Ajusta la ruta según dónde tengas tu menú en la escena principal
		
		upgradeMenu = GetTree().GetFirstNodeInGroup("upgrademenu") as Control;

		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
		stayTimer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("Entro");
		if (body.IsInGroup("player"))
		{
			stayTimer.Start();
		}
	}

	private void OnBodyExited(Node body)
	{
		GD.Print("Salio");
		if (body.IsInGroup("player"))
		{
			stayTimer.Stop();
		}
	}

	private void OnTimerTimeout()
	{
		GD.Print("Okay!");
		upgradeMenu.Visible = true;
	}
	
}
