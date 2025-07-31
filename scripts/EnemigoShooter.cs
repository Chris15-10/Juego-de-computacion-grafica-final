using Godot;
using System;

public partial class EnemigoShooter : Enemigo
{
    [Export] public enemigoRecurso config;
    [Export] public float RangoDisparo = 200.0f; // distancia desde la que disparan (determina el tamaño del Raycast)
    [Export] public float DistanciaMinima = 100.0f; // distancia a partir de la que los enemigos se alejan del jugador
    [Export] public float VelocidadShooter = 40.0f;
    [Export] public int balavel = 300;

    private Arma _arma;
    private Timer _timerDisparo;
    private RayCast2D _rayJugador;


    public override void _Ready()
    {
        base._Ready();

        _arma = GetNodeOrNull<Arma>("Arma");
        _timerDisparo = GetNodeOrNull<Timer>("Disparo");
        _rayJugador = GetNodeOrNull<RayCast2D>("RayJugador");

        cargar(config);
        _arma.balavel = balavel;

        if (_rayJugador != null)
            _rayJugador.Enabled = true;

        if (_timerDisparo != null)
            _timerDisparo.Timeout += Disparar;
    }

    public override void _PhysicsProcess(double delta)
    {

        if (muerto || player == null) return;

        if (knockbackTimer > 0)
        {
            Velocity = knockback;
            knockbackTimer -= (float)delta;
            MoveAndSlide();
            return;
        }

        Node2D centro = player.GetNodeOrNull<Node2D>("Centro");
        if (centro == null) return;

        Vector2 direccionCentro = (centro.GlobalPosition - GlobalPosition).Normalized();

        if (_rayJugador != null && activo)
        {
            _rayJugador.TargetPosition = direccionCentro * RangoDisparo;
            _rayJugador.ForceRaycastUpdate();
        }

        float distancia = GlobalPosition.DistanceTo(centro.GlobalPosition);

        bool jugadorVisible = false;
        if (_rayJugador != null && _rayJugador.IsColliding() && activo)
        {
            Node? collider = _rayJugador.GetCollider() as Node;
            if (collider != null && (collider.IsInGroup("jugador") || (collider.GetParent()?.IsInGroup("jugador") == true)))
                jugadorVisible = true;
        }

        if (distancia < DistanciaMinima && activo)
        {
            Velocity = -direccionCentro * VelocidadShooter;
        }
        else if (jugadorVisible && activo)
        {
            Velocity = Vector2.Zero;
            _arma?.LookAt(centro.GlobalPosition);
        }
        else if (activo)
        {
            Velocity = direccionCentro * VelocidadShooter;
        }

        MoveAndSlide();

        if (direccionCentro != Vector2.Zero)
            ReproducirAnimacionPorDireccion(direccionCentro);
    }

    private void Disparar()
    {
        if (muerto || _arma == null || _rayJugador == null) return;

        Vector2 direccion = (player.GlobalPosition - GlobalPosition).Normalized();
        _rayJugador.TargetPosition = direccion * RangoDisparo;
        _rayJugador.ForceRaycastUpdate();

        if (_rayJugador.IsColliding() && activo)
        {
            Node? collider = _rayJugador.GetCollider() as Node;
            if (collider != null && (collider.IsInGroup("jugador") || (collider.GetParent()?.IsInGroup("jugador") == true)))
            {
                _arma.LookAt(player.GlobalPosition);
                _arma.Disparar(player.GlobalPosition);
                return;
            }
        }
    }

    public override void cargar(enemigoRecurso config)
    {
        if (config != null)
        {
            _sprite.SpriteFrames = config._sprite;
            _sprite.Scale = new Vector2(config.tamano, config.tamano);
            RangoDisparo = config.RangoDisparo;
            DistanciaMinima = config.DistanciaMinima;
            VelocidadShooter = config.velocidad;
            _arma._bala = config._bala;
        }

    }
    public override void mover(Vector2 direccion, float fuerza)
    {
        knockback = direccion * fuerza;
        knockbackTimer = knockbackDuration;
    }
}

