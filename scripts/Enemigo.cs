using Godot;
using System;

public partial class Enemigo : CharacterBody2D
{
    [Export] public float Speed = 100f;
    [Export] public float ActivacionDistancia = 200f;
    [Signal] public delegate void MuertoEventHandler();
    [Export] public Areapuertas miarea;

    protected Node2D player;
    protected AnimatedSprite2D _sprite;
    public AnimationPlayer _anim;
    protected bool muerto = false;
    private string animActual = "";
    protected bool activo = false;
    public Vector2 knockback = Vector2.Zero;
    public float knockbackTimer = 0f;
    public const float knockbackDuration = 0.1f;
    

    public override void _Ready()
    {

        var jugadores = GetTree().GetNodesInGroup("jugador");
        if (jugadores.Count > 0)
            player = jugadores[0] as Node2D;

        _sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
        if (_sprite == null)
            GD.PrintErr("No se encontró el nodo AnimatedSprite2D");
        _anim = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
    }

    private Vector2 ultimaDireccion = Vector2.Zero;

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

        float distancia = GlobalPosition.DistanceTo(player.GlobalPosition);

        if (distancia <= ActivacionDistancia && activo)
        {
            Vector2 dir = GlobalPosition.DirectionTo(player.GlobalPosition);
            ultimaDireccion = dir;

            Velocity = dir * Speed;
            MoveAndSlide();

            ReproducirAnimacionPorDireccion(dir);
        }
        else
        {
            Velocity = Vector2.Zero;
            if (animActual != "walk1")
            {
                _sprite.Play("walk1");
                animActual = "walk1";
            }
        }
    }

    protected virtual void ReproducirAnimacionPorDireccion(Vector2 direccion)
    {
        if (direccion == Vector2.Zero)
        {
            // animacion de idle 
            if (animActual != "walk9")
            {
                _sprite.Play("walk9");
                _sprite.FlipH = false;
                animActual = "walk9";
            }
            return;
        }

        float angulo_radianes = direccion.Angle();
        float angulo_grados = Mathf.RadToDeg(angulo_radianes); // -180 to 180

        angulo_grados += 90;
        if (angulo_grados < 0)
            angulo_grados += 360;
        angulo_grados %= 360;

        string anim = "walk1";
        bool voltear = false;

        float angulo = angulo_grados + 11.25f;
        angulo %= 360; // mantener de 0 a 360

        // indice del segmento (0 a 15)
        int indice = (int)(angulo / 22.5f);

        switch (indice)
        {
            case 0: anim = "walk1"; voltear = false; break;
            case 1: anim = "walk2"; voltear = false; break;
            case 2: anim = "walk3"; voltear = false; break;
            case 3: anim = "walk4"; voltear = false; break;
            case 4: anim = "walk5"; voltear = false; break;
            case 5: anim = "walk6"; voltear = false; break;
            case 6: anim = "walk7"; voltear = false; break;
            case 7: anim = "walk9"; voltear = false; break;
            case 8: anim = "walk8"; voltear = true; break;
            case 9: anim = "walk7"; voltear = true; break;
            case 10: anim = "walk6"; voltear = true; break;
            case 11: anim = "walk5"; voltear = true; break;
            case 12: anim = "walk4"; voltear = true; break;
            case 13: anim = "walk3"; voltear = true; break;
            case 14: anim = "walk2"; voltear = true; break;
            case 15: anim = "walk1"; voltear = false; break;
            default: anim = "walk1"; voltear = false; break;
        }

        if (_sprite.FlipH != voltear)
        {
            _sprite.FlipH = voltear;
        }
        if (anim != animActual)
        {
            _sprite.Play(anim);
            animActual = anim;
        }
    }

    public virtual void SetMuerto(bool estado)
    {
        muerto = estado;
    }
    public void Activar()
    {
        activo = true;
    }

    public virtual void cargar(enemigoRecurso config)
    {
        if (config == null)
        {
            GD.PrintErr("config es null en Enemigo.cargar");
            return;
        }
        if (_sprite == null)
        {
            GD.PrintErr("_sprite es null en Enemigo.cargar");
            return;
        }

        if (config._sprite == null)
        {
            GD.PrintErr("config._sprite es null en Enemigo.cargar");
            return;
        }

        _sprite.SpriteFrames = config._sprite;
        _sprite.Scale = new Vector2(config.tamano, config.tamano);
    }

    public virtual void mover(Vector2 direccion, float fuerza)
    {
        knockback = direccion * fuerza;
        knockbackTimer = knockbackDuration;
    }
}
