using Godot;
using System;

public partial class KnockBackComponent : Node
{
	/*
	Exportamos las variables de fuerza y duracion para controlar el knockback
	Y declaramos las variables para recibir un body, que sera el cual recibira
	el knochback, junto con la velocidad que usaremos para mover el body y
	un temporizador para controlar la duracion
	*/
	[Export] public float force = 200f;
	[Export] public float duration = 0.15f;
	
	private CharacterBody2D body;
	private Vector2 velocity;
	private float timer = 0f;
	/*
	Inicializamos la varaible de body tomando el nodo padre y comprobamos que este
	ligado con un knockbackComponent
	*/
	public override void _Ready()
	{
		body = GetParent<CharacterBody2D>();
		if(body == null)
		{
			GD.Print("KnockBackComponent debe tener un nodo padre de characterBody2D");
		}
	}
	/*
	Aplicamos utilizando una variable de direccion 
	le aplicamos normalizacion a la direccion para evitar saltos y aplicamos la fuerza
	Y reseteamos el temporizador
	*/
	public void knockBack(Vector2 direccion)
	{
		velocity = direccion.Normalized() * force;
		timer = duration;
	}
	/*
	Revisamos constantemente si el temporizador es mayor a 0, en caso de que si reducimos el timepo 
	del temporizador utilizando delta como variable
	Mantenemos la velocidad del body y contianuamos ejecutando su movimiento
	*/
	public override void _PhysicsProcess(double delta)
	{
		if(body == null) return;
		
		if(timer > 0){
			timer -= (float)delta;
			body.Velocity = velocity;
			body.MoveAndSlide();
		}
	}
}
