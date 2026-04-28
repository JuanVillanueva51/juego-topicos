using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// UpgradeTower – Torre con campana que mejora las estadísticas del jugador.
///
/// Flujo:
///   1. Jugador entra al rango (Area2D grande).
///   2. Timer de 15 s comienza. Una barra de progreso opcional puede mostrar el avance.
///   3. Al completarse, se muestran 3 mejoras aleatorias en la GUI.
///   4. El jugador elige una. Las mejoras se guardan en PlayerUpgradeStats,
///      SEPARADAS del WeaponData, de modo que se aplican a cualquier arma equipada.
///   5. La torre tiene un cooldown antes de poder usarse de nuevo (configurable).
///
/// Nodos requeridos en la escena:
///   - CollisionShape2D  (radio grande, ej. 250+)
///   - Timer             (llamado "StayTimer", one_shot=true, wait_time=15)
///   - CanvasLayer > Control  (llamado "UpgradeUI") con:
///       - ProgressBar  "ProgressBar"
///       - Panel        "Panel" con:
///           - Button   "Btn0"
///           - Button   "Btn1"
///           - Button   "Btn2"
/// </summary>

public partial class UpgradeTower : Area2D
{
	[Export] public float StayDuration    = 1f;   // segundos que el jugador debe permanecer
	[Export] public float TowerCooldown   = 1f;   // segundos antes de poder usar la torre de nuevo
	[Export] public bool  ShowProgressBar = true;

	// ── Pool de posibles mejoras ───────────────────────────────────────────
	// Ajusta los valores a tu gusto o hazlos exportables
	private static readonly List<(string stat, float value, string text)> UpgradePool = new()
	{
		("dmg",             2,      "+2 Daño"),
		("dmg",             5,      "+5 Daño"),
		("cooldown",       -0.05f,  "-0.05 Cooldown (más rápido)"),
		("cooldown",       -0.1f,   "-0.10 Cooldown (más rápido)"),
		("shootQuantity",   1,      "+1 Proyectil por disparo"),
		("projectileScale", 0.2f,   "+20% Tamaño de proyectil"),
		("projectileScale", 0.4f,   "+40% Tamaño de proyectil"),
		("projectileSpeed", 50f,    "+50 Velocidad de proyectil"),
		("projectileSpeed", 100f,   "+100 Velocidad de proyectil"),
		("lifeTime",        0.5f,   "+0.5 s Vida de proyectil"),
		("pierceCount",     1,      "+1 Pierce"),
		("spread",         -5f,     "-5° Dispersión (más preciso)"),
	};

	private Timer        _stayTimer;
	private ProgressBar  _progressBar;
	private Control      _upgradePanel;
	private Button[]     _buttons = new Button[3];

	private bool                  _playerInside  = false;
	private bool                  _onCooldown    = false;
	private double                _cooldownRemaining = 0;
	private UpgradeOption[]       _currentOptions = new UpgradeOption[3];
	private PlayerUpgradeStats    _playerStats;

	public override void _Ready()
	{
		// Timer de permanencia
		_stayTimer = GetNodeOrNull<Timer>("StayTimer");
		if (_stayTimer == null)
		{
			GD.PrintErr("UpgradeTower: no se encontró el nodo 'StayTimer'.");
			return;
		}
		_stayTimer.WaitTime = StayDuration;
		_stayTimer.OneShot  = true;
		_stayTimer.Timeout  += OnStayTimerTimeout;

		// UI
		// Busca dentro de un CanvasLayer → Control → UpgradeUI
		// Ajusta la ruta según tu escena
		_progressBar  = GetNodeOrNull<ProgressBar>("CanvasLayer/UpgradeUI/ProgressBar");
		_upgradePanel = GetNodeOrNull<Control>("CanvasLayer/UpgradeUI/Panel");

		if (_upgradePanel != null)
		{
			_buttons[0] = _upgradePanel.GetNodeOrNull<Button>("VBoxContainer/Btn0");
			_buttons[1] = _upgradePanel.GetNodeOrNull<Button>("VBoxContainer/Btn1");
			_buttons[2] = _upgradePanel.GetNodeOrNull<Button>("VBoxContainer/Btn2");

			for (int i = 0; i < 3; i++)
			{
				int idx = i; // captura para el closure
				if (_buttons[idx] != null)
					_buttons[idx].Pressed += () => OnUpgradeChosen(idx);
			}
			_upgradePanel.Visible = false;
		}

		if (_progressBar != null)
		{
			_progressBar.MinValue = 0;
			_progressBar.MaxValue = StayDuration;
			_progressBar.Value    = 0;
			_progressBar.Visible  = false;
		}

		BodyEntered += OnBodyEntered;
		BodyExited  += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		// Barra de progreso
		if (ShowProgressBar && _progressBar != null && _playerInside && !_onCooldown)
		{
			double elapsed = StayDuration - _stayTimer.TimeLeft;
			_progressBar.Value = elapsed;
		}

		// Cooldown de la torre
		if (_onCooldown)
		{
			_cooldownRemaining -= delta;
			if (_cooldownRemaining <= 0)
			{
				_onCooldown = false;
				GD.Print("UpgradeTower: lista para usar de nuevo.");
			}
		}
	}

	//Entrar / Salir
	private void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("player")) return;
		if (_onCooldown) return;

		_playerInside = true;
		_playerStats  = body.GetNodeOrNull<PlayerUpgradeStats>("PlayerUpgradeStats");

		if (_playerStats == null)
		{
			GD.PrintErr("UpgradeTower: el jugador no tiene nodo 'PlayerUpgradeStats'.");
			return;
		}

		_stayTimer.Start();

		if (_progressBar != null)
		{
			_progressBar.Value   = 0;
			_progressBar.Visible = ShowProgressBar;
		}
		GD.Print("UpgradeTower: jugador dentro del rango. Iniciando cuenta de 15 s…");
	}

	private void OnBodyExited(Node body)
	{
		if (!body.IsInGroup("player")) return;

		_playerInside = false;
		_stayTimer.Stop();

		if (_progressBar != null)
		{
			_progressBar.Value   = 0;
			_progressBar.Visible = false;
		}
		GD.Print("UpgradeTower: jugador salió del rango. Cuenta cancelada.");
	}

	//Timer completado mostrar mejoras
	private void OnStayTimerTimeout()
	{
		if (!_playerInside || _playerStats == null) return;

		GD.Print("UpgradeTower: ¡15 segundos completados! Mostrando mejoras.");
		GenerateAndShowOptions();
	}

	//Generar 3 mejoras únicas aleatorias
	private void GenerateAndShowOptions()
	{
		var pool = new List<(string stat, float value, string text)>(UpgradePool);
		var rng  = new Random();
		for (int i = pool.Count - 1; i > 0; i--)
		{
			int j = rng.Next(i + 1);
			(pool[i], pool[j]) = (pool[j], pool[i]);
		}

		for (int i = 0; i < 3; i++)
		{
			_currentOptions[i] = new UpgradeOption
			{
				StatName    = pool[i].stat,
				Value       = pool[i].value,
				DisplayText = pool[i].text
			};
			if (_buttons[i] != null)
				_buttons[i].Text = pool[i].text;
		}

		if (_upgradePanel != null) _upgradePanel.Visible = true;
		if (_progressBar  != null) _progressBar.Visible  = false;

		// Activar mouse filter para que los botones reciban clicks
		if (_upgradePanel != null)
			_upgradePanel.MouseFilter = Control.MouseFilterEnum.Stop;

		GetTree().Paused = true;
	}

	//El jugador elige una opción
	private void OnUpgradeChosen(int index)
	{
		if (_playerStats == null || index < 0 || index >= 3) return;

		_playerStats.ApplyUpgrade(_currentOptions[index]);

		if (_upgradePanel != null)
		{
			_upgradePanel.Visible     = false;
			// Volver a Ignore para no bloquear clicks de otros UI
			_upgradePanel.MouseFilter = Control.MouseFilterEnum.Ignore;
		}

		GetTree().Paused = false;

		_onCooldown        = true;
		_cooldownRemaining = TowerCooldown;
		_playerInside      = false;

		GD.Print($"UpgradeTower: mejora '{_currentOptions[index].DisplayText}' aplicada.");
	}
}

public class UpgradeOption
{
	public string StatName    { get; set; }
	public float  Value       { get; set; }
	public string DisplayText { get; set; }
}
