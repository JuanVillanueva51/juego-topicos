using Godot;

public partial class SceneIntro : Control
{
	public override void _Ready()
	{
		GD.Print("Menu cargado");
	}

	private void _on_button_pressed()
	{
GetTree().ChangeSceneToFile("res://scenes/game.tscn");
}
}
