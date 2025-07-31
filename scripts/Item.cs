using Godot;
using System;

public partial class Item : Area2D
{
	[Export] public ObjetoConfig Config;

	private Sprite2D _icono;
	private Label _mensaje;
	private bool _jugadorCerca = false;
	private Personaje1 _personaje;

	public override void _Ready()
	{
		_icono = GetNode<Sprite2D>("Sprite2D");
		_mensaje = GetNode<Label>("Mensaje");
		_mensaje.Visible = false;

		if (Config != null && Config.Icono != null)
		{
			_icono.Texture = Config.Icono;
		}

		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		if (_jugadorCerca && Input.IsActionJustPressed("x"))
		{
			switch (Config)
			{
				case ArmaConfig arma:
					ArmaConfig anterior = _personaje.RecogerArma(arma);
					cargar(anterior);
					break;

				case botiquinConfig botiquin:
					_personaje.CurarVida(botiquin.vidaquecura);
					QueueFree(); // desaparece tras usarlo
					break;
			}
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Personaje1 pj)
		{
			_jugadorCerca = true;
			_personaje = pj;
			_mensaje.Visible = true;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body == _personaje)
		{
			_jugadorCerca = false;
			_mensaje.Visible = false;
		}
	}
	public void cargar(ObjetoConfig nueva)
    {
        if (nueva != null)
        {
            Config = nueva;
            _icono.Texture = nueva.Icono;
            AddToGroup(nueva.GrupoItem);
        }
    }
}
