using Godot;
using System;
public partial class Areapuertas : Area2D
{

	[Export] public ParedAbajo[] _pared_abajo;
	[Export] public ParedArriba puerta_arriba;

	[Export] public int enemigosObjetivo = 5;

	private int enemigosMuertos = 0;
	private bool activado = false;
	private bool puertasAbiertas = false;

	private CollisionShape2D _colision;

	public override void _Ready()
	{
		_colision = GetNode<CollisionShape2D>("CollisionShape2D");
		BodyEntered += OnBodyEntered;
		
	}

	private void OnBodyEntered(Node body)
	{
		if (!body.IsInGroup("jugador") || activado) return;

		GD.Print("¡Jugador entró! Cerrando puertas");
		activado = true;

		for (int i = 0; i < _pared_abajo.Length; i++)
		{
			_pared_abajo[i].cerrar();
		}
		if (puerta_arriba != null)
		{
			puerta_arriba.cerrar();
		}
		var hud = GetTree().Root.GetNodeOrNull<HUD>("Main/HUD");
		hud?.MostrarMensaje("¡Sobrevive!");
	}

	public void RegistrarMuerte()
	{
		if (!activado || puertasAbiertas) return; 

		enemigosMuertos++;
		GD.Print($"Enemigos muertos: {enemigosMuertos}");

		if (enemigosMuertos >= enemigosObjetivo)
		{
			GD.Print("Se cumple la condición para abrir puertas");

			puertasAbiertas = true;

			for (int i = 0; i < _pared_abajo.Length; i++)
			{
				_pared_abajo[i].Abrir();
			}
			if (puerta_arriba != null)
			{
				puerta_arriba.Abrir();
			}
			var hud = GetTree().Root.GetNodeOrNull<HUD>("Main/HUD");
			hud?.MostrarMensaje("¡Zona Despejada!");
			_colision?.SetDeferred("monitoring", false);

			var spawner = GetTree().GetFirstNodeInGroup("spawner") as SpawnerEnemigos;
			spawner?.SetDeferred("monitoring", false);
			spawner?._timer?.Stop();

			GD.Print("Contador de enemigos reiniciado");
			enemigosMuertos = 0;
		}
	}
}
