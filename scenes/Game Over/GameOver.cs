using Godot;
using System;

public partial class GameOver : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	var label = GetNode<RichTextLabel>("CenterContainer/Label");
	label.Text = $"[center][wave]GAME OVER\nSCORE: {Global.Score}[/wave][/center]";
	Global.Score = 0;

	var timer = GetNode<Timer>("CenterContainer/Label/Timer");
	timer.Timeout += OnTimerTimeout;
	}
	
	private void OnTimerTimeout()
	{
		GetTree().ChangeSceneToFile("res://scenes/intro/scene_intro.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
