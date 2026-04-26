using Godot;
using System;

public partial class WaveManager : Node
{
	[Export] public PackedScene EnemyScene;
	[Export] public int baseEnemigos = 32;
	[Export] public int increment = 16;
	[Export] public float descanso = 10f;
	[Export] public float spawnCooldown = 0.2f;
	[Export] public float spawnDistancia = 400f;
	private Node2D player;
	private float timer = 0f;
	private int currentWave = 0;
	private int enemigosSpawneados = 0;
	private int enemigosVivos = 0;
	private float descansoTimer = 0f;
	private float spawnTimer = 0f;
	private bool descansando = true;
	private bool spawneando = false;
	
	public override void _Ready(){
		player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if(player == null){
			GD.Print("Wave manager no encontro al player");
		}
		descansar();
	}
	public override void _Process(double delta){
		if(player == null || EnemyScene == null) return;
		if(descansando)
		{
			descansoTimer -= (float)delta;
			if(descansoTimer <= 0f){
				startWave();
			}
			return;
		}
		if(spawneando)
		{
			spawnTimer -= (float)delta;
			if(spawnTimer <= 0f && enemigosSpawneados > 0){
				spawnEnemy();
				enemigosSpawneados--;
				enemigosVivos++;
				spawnTimer = spawnCooldown;
			}
			if(enemigosSpawneados <= 0) spawneando = false;
		}
		if(!spawneando && enemigosVivos <= 0) 
		{
			descansar();
		}
	}
	private void descansar(){
		descansando = true;
		spawneando = false;
		descansoTimer = descanso;
	}
	private void startWave(){
		currentWave++;
		enemigosSpawneados = baseEnemigos + increment *(currentWave -1);
		descansando = false;
		spawneando = true;
		spawnTimer = 0f;
		GD.Print("Iniciando oleada " + currentWave);
	}
	private void spawnEnemy(){
		Node2D enemy = EnemyScene.Instantiate<Node2D>();
		enemy.GlobalPosition =  getSpawnAroundPlayer();
		GetTree().CurrentScene.AddChild(enemy);
		enemy.TreeExited += () =>
		{
			enemigosVivos--;
			GD.Print("Enemigo eliminado. Quedan: " + enemigosVivos);
		};
	}
	private Vector2 getSpawnAroundPlayer(){
		float angle = (float)GD.RandRange(0, Mathf.Tau);
		Vector2 direccion = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		return player.GlobalPosition + direccion * spawnDistancia;
	}
}
