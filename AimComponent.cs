using Godot;
using System;

public partial class AimComponent : Node2D
{
	//Se inicializan los componentes para obtener al jugador y el sprite
	private Node2D player;
	private Sprite2D weaponSprite;
	
	public override void _Ready()
	{
		//Aseguramos de agarrar los componentes de los nodos 
		player = GetParent<Node2D>();
		weaponSprite = GetNode<Sprite2D>("WeaponSprite");
		if(weaponSprite == null){
			GD.Print("Component no encontrado");
		}
	}
	//Se obtiene la posicion del mouse usando GetGlobalMousePosition
	//Luego utilizamos el metodo de LookAt para movernos hacia la posicion
	//Ajustamos la posicion al final utilizando angle
	public override void _Process(double delta)
	{
		
		Vector2 mousePos = GetGlobalMousePosition();
		LookAt(mousePos);
		float angle = (mousePos - GlobalPosition).Angle();
		if (angle > Mathf.Pi / 2 || angle < -Mathf.Pi / 2)
		{
		weaponSprite.FlipV = true;
		}
		else
		{
		weaponSprite.FlipV = false;
		}
	}
}
