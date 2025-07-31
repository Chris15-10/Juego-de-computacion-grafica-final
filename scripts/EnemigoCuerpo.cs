using Godot;
using System;
// prueba de github mari
public partial class EnemigoCuerpo : Enemigo
{
    [Export] public enemigoRecurso config;
    [Export] public int dano = 20;
    [Export] public float rango_ataque = 40.0f;
    private Timer _timerAtaque;
    protected AnimatedSprite2D _animatedSprite;
    
    public override void _Ready()
    {
        base._Ready();
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _timerAtaque = GetNodeOrNull<Timer>("Ataque");
        if (_timerAtaque != null)
            _timerAtaque.Timeout += AtacarJugador;

        cargar(config);
    }

    private void AtacarJugador()
    {
        if (muerto) return;
        if (player == null) return;

        float distancia = GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distancia <= rango_ataque)
        {
            if (player.IsInGroup("jugador"))
            {
                var vida = player.GetNodeOrNull<Vida>("Vida");
                vida.RecibirDano(dano);
            }
        }
    }
    public override void cargar(enemigoRecurso config)
    {
        if (config != null)
        {
            _sprite.SpriteFrames = config._sprite;
            _sprite.Scale = new Vector2(config.tamano, config.tamano);
           dano = config.dano;
        }

    }
}

