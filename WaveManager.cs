using Godot;
using System;

public partial class WaveManager : Node
{
	/*
	Este es un scrip bastante largo
	Primaro los valores exportables sirven para controlar un array de enemigos que se
	utilizaran en la oleada. Este array es determinado por SpawnData, revisar Spawn data para
	saber como se utliza
	Luego aplicamos variables de control, para manejar la cantidad de enemigos base del jeugo
	El incremento que se hara por oleada, un tiempo de descanso, un cooldown entre spawn y
	la distancia que se spawnearan alrededor del jugador*/
	[Export] public Godot.Collections.Array<SpawnData> enemies;
	[Export] public int baseEnemigos = 32;
	[Export] public int increment = 16;
	[Export] public float descanso = 10f;
	[Export] public float spawnCooldown = 0.2f;
	[Export] public float spawnDistancia = 400f;
	/*
	En estas variables las mas importantes son currentWave y espaciosDisponibles.
	Currentwave maneja el numero de oleada actual y espaciosDisponibles se inicializara a la cantidad de enemigos
	al iniciar la oleada. espaciosDisponibles maneja el Peso de los enemigos en su data. 
	Este peso es un costo que tiene el enemigo para spawnear, si el slime tiene un peso de 1
	tendra un costo de 1 para espacios, osea solo toma un espacio.
	La abeja tiene un costo de 3, en ese caso la abeja tomara 3 espacios de espaciosDisponibles para spawnear
	las demas variables son temporizadores para controlar el descanso entre waves y el temporizador
	de spawn
	Nota: Revisar el funcionamiento de los costos de enemigos, porque no se toman a random
	de manera adecuada y aparecen mas abejas en la oleada 2 que slimes*/
	private Node2D player;
	private float timer = 0f;
	private int currentWave = 0;
	private int espaciosDisponibles = 0;
	private int enemigosVivos = 0;
	public float descansoTimer = 0f;
	private float spawnTimer = 0f;
	public bool descansando = true;
	private bool spawneando = false;
	
	/*
	Al iniciar la wave iniciamos el descanso y diferimos la busqueda del player.
	CallDeferred garantiza que la busqueda ocurra despues de que todos los _Ready()
	del arbol de escena hayan terminado, incluyendo el del player que es donde
	se hace AddToGroup("player").*/
	public override void _Ready(){
		descansar();
		CallDeferred(MethodName.FindPlayer);
	}

	private void FindPlayer(){
		player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if(player == null){
			GD.PrintErr("WaveManager: no se encontro al player");
		}
	}

	/*
	Metodo de proccess*/
	public override void _Process(double delta){
		if(player == null || enemies == null || enemies.Count == 0) return;
		/*
		Al estar dentro de descanso, reducimos el temporizador constantemente
		hasta que sea menor o igual a 0, en ese caso inciamos la wave*/
		if(descansando)
		{
			descansoTimer -= (float)delta;
			if(descansoTimer <= 0f){
				startWave();
			}
			return;
		}
		/*
		Aqui spawneamos a los enemigos, se van spawneando en cuanto el tiempo enre spawn
		sea menor o igual a 0 y los espacios disponibles sean mas de 0
		Cuando ya no haya espacios entonces spawnear se vuelve faso
		y en caso de que spawnear sea falso y ya no haya enemigos iniciamos un descanso*/
		if(spawneando)
		{
			spawnTimer -= (float)delta;
			if(spawnTimer <= 0f && espaciosDisponibles > 0){
				spawnEnemy();
				spawnTimer = spawnCooldown;
			}
			if(espaciosDisponibles <= 0) spawneando = false;
		}
		if(!spawneando && enemigosVivos <= 0) 
		{
			descansar();
		}
	}
	/*
	Inicamos el descanso y reiniciamos el temporizador de descanso*/
	private void descansar(){
		descansando = true;
		spawneando = false;
		descansoTimer = descanso;
	}
	/*
	Se inicializa la wave, moviendo el numero de la wave y
	estableciendo los espacios disponibles en base a los enemigos base y el incremento
	multiplicado por el numero de Wave actual*/
	private void startWave(){
		currentWave++;
		espaciosDisponibles = baseEnemigos + increment *(currentWave -1);
		descansando = false;
		spawneando = true;
		spawnTimer = 0f;
		GD.Print("Iniciando oleada " + currentWave);
	}
	/*
	Obtenemos la data de los enemigos para luego aplicar el metodo de randomEncounter
	Obtenemos al enemigo utilizando las escenas dentro de la data
	y spawneamos al enemigo basandonos en la posicion que de el metodo de 
	GetSpawnAroungPlayer para spanearlo en los alrededores
	Añadimos al enemigo a la escena
	Aumentamos los enemigos vivos
	y reducimos los espacios disponibles en base a el peso del enemigo*/
	private void spawnEnemy(){
		SpawnData data = randomEncounter();
		if(data == null || data.enemyScene == null){
			spawneando = false;
			espaciosDisponibles = 0;
			return;
		}
		Node2D enemy = data.enemyScene.Instantiate<Node2D>();
		enemy.GlobalPosition =  getSpawnAroundPlayer();
		GetTree().CurrentScene.AddChild(enemy);
		espaciosDisponibles -= data.Difficulty;
		enemigosVivos++;
		/*
		Cuando el enemigo sale del arbol, asumimos que el enemigo murio*/
		enemy.TreeExited += () =>
		{
			enemigosVivos--;
			GD.Print("Enemigo eliminado. Quedan: " + enemigosVivos);
	};
	}
	/*
	Obtenemos el angulo del jugador de un angulo entre 0 y Mathf.Tau
	Este es el equivalente de alrededor de todo el jugador, ya que da una vuelta al
	radio
	Luego convertimos esa direccion en un vector
	Y retornamos como vector la posicion actual del jugador, la direccion que obtuvimos y 
	una distancia a la cual se spawneara*/
	private Vector2 getSpawnAroundPlayer(){
		float angle = (float)GD.RandRange(0, Mathf.Tau);
		Vector2 direccion = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		return player.GlobalPosition + direccion * spawnDistancia;
	}
	/*
	Aqui es donde crearemos los encuentros*/
	private SpawnData randomEncounter()
	{
		var disponibles = new Godot.Collections.Array<SpawnData>();
		/*
		Por cada enemigo dentro de la variable gloal de enemies buscaremos si 
		puede spawnear en este oleada y si tiene el peso suficiente para spawnear*/
		foreach (var e in enemies)
		{
			if(currentWave >= e.spawnWave && e.Difficulty <= espaciosDisponibles){
				disponibles.Add(e);
			}
		}
		if(disponibles.Count == 0){
			return null;
		}
		/*
		Aqui obtendremos el peso total de todos los enemigos disponibles para spawnear*/
		int pesoTotal = 0;
		foreach(var e in disponibles)
		{
			pesoTotal += e.peso;
		}
		/*
		Utilizando el peso total, haremos un roll, que sera un numero aleatorio entre
		0 y el peso total - 1.
		Luego lo que haremos aqui es ir sumando el valor calculado de los pesos totales de los enemigos
		Osea sumaremos todos los pesos.
		Luego checaremos cada vez si el numero de roll es menor al peso acumulado, retornaremos el
		enemigo que equivalga a ese peso.
		Es decir, si primero llega un bee, que tiene un peso de 20
		y se rolleo 7, entonces spawneamos una bee.
		Pero si luego llega un slime su peso se acumula, haciendo que el peso total pase de 20 a 30
		En ese caso, se checa si cuando se hace el roll cae dentro de los primero 0 a 19 que seria bee
		o 20 a 29, que seria slime*/
		int roll = (int)(GD.Randi() % pesoTotal);
		int acumulado = 0;
		foreach(var e in disponibles)
		{
			acumulado += e.peso;
			if(roll < acumulado) 
				return e;
		}
		return disponibles[0];
	}
}
