using Godot;
using System;

public partial class Bala : Area2D
{
    private Vector2 _direccion;
    private int _daño;
    private int _velocidad;
    private Sprite2D _sprite;
    private string _grupo;
    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        BodyEntered += OnBodyEntered;
    }

    public void Init(Vector2 direccion, int velocidad, int dano, Texture2D textura, string grupo)
    {
        _grupo = grupo;

        if (grupo == "jugador")
        {
            // obtiene el primer nodo del grupo jugador
            var jugadores = GetTree().GetNodesInGroup("jugador");
            if (jugadores.Count > 0)
            {
                var jugador = jugadores[0] as Node2D;
                if (jugador != null)
                {
                    var centro = jugador.GetNodeOrNull<Node2D>("Centro");
                    if (centro != null)
                    {
                        direccion = (centro.GlobalPosition - GlobalPosition).Normalized();
                    }
                    else
                    {
                        direccion = (jugador.GlobalPosition - GlobalPosition).Normalized();
                    }
                }
            }
        }
        _direccion = direccion;
        _velocidad = velocidad;
        _daño = dano;
        Rotation = _direccion.Angle();

        if (_sprite == null)
            _sprite = GetNode<Sprite2D>("Sprite2D");

        if (_sprite != null && textura != null)
            _sprite.Texture = textura;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direccion * _velocidad * (float)delta;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is TileMapLayer)
        {
            QueueFree();
            return;
        }

        if (body.IsInGroup(_grupo))
        {
            var vida = body.GetNodeOrNull<Vida>("Vida");
            if (vida != null)
            {
                vida.RecibirDano(_daño);
                if (body is Enemigo enemigoGolpeado)
                {
                    enemigoGolpeado.mover(_direccion, _daño * 1f);
                }
                else if (body is Personaje1 pers)
                {
                    pers.mover(_direccion, _daño * 0.7f );
                }
                QueueFree(); 
            }
        }

    }
}

