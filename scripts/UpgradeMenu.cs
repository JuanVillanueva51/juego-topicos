using Godot;
using System;

public partial class UpgradeMenu : Control 
{
	private Node2D player;
	public override void _Ready()
	{	
		
		player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		player = GetTree().Root.GetNode<Player>("Game/player");
		GetNode<Button>("Button1").Pressed += OnButton1Pressed;
		GetNode<Button>("Button2").Pressed += OnButton2Pressed;
		GetNode<Button>("Button3").Pressed += OnButton3Pressed;
	}

	private void OnButton1Pressed()
	{
		GD.Print("+ 5 de velocidad");
		Hide();
	}

	private void OnButton2Pressed()
	{
		GD.Print("+ 10 de daño");
		Hide();
	}

	private void OnButton3Pressed()
	{
		GD.Print("+3% de EXP");
		Hide();
	}
}
