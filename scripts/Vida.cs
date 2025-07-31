using Godot;
using System;

public partial class Vida : Node
{
    [Export] public int salud_max = 100;

    public int salud_act;
    private AnimatedSprite2D _sprite;
    public BarraVida _barravida;
    public bool muerto = false;
    [Export] public PackedScene DanoEnemigoEscena;


   public override void _Ready()
    {
        salud_act = salud_max;

        var padre = GetParent();
        _sprite = padre.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");

        if (_sprite != null)
            _sprite.AnimationFinished += OnAnimacionTerminada;

        if (padre.IsInGroup("jugador"))
        {
            var barra = GetTree().GetNodesInGroup("vidajugador");
            if (barra.Count > 0)
            {
                _barravida = barra[0] as BarraVida;
                _barravida?.ActualizarBarra(salud_act, salud_max);
            }
        }
    }

    public void RecibirDano(int cant)
    {
        salud_act -= cant;
        salud_act = Math.Max(salud_act, 0);

        // solo actualizar la barra de vida si es jugador
        if (GetParent().IsInGroup("jugador"))
        {
            _barravida?.ActualizarBarra(salud_act, salud_max);
        }

        float porcentaje = (float)salud_act / (float)salud_max;

        if (!GetParent().IsInGroup("jugador") && DanoEnemigoEscena != null)
        {
            var instancia = DanoEnemigoEscena.Instantiate<DanoEnemigo>();
            GetParent().AddChild(instancia);
            instancia.GlobalPosition = GetParent<Node2D>().GlobalPosition + new Vector2(0, -10);
            instancia.Mostrar(cant, porcentaje);
        }


        if (salud_act <= 0 && !muerto)
        {
            Morir();
            muerto = true;
        }
            
            
            
    }

    public void Morir()
    {
        var padre = GetParent();
        if (padre.IsInGroup("enemigo") && padre is Enemigo e)
        {
            e.EmitSignal(Enemigo.SignalName.Muerto);  
        }

        // si es enemigo reproduce animación
        if (_sprite != null && padre.IsInGroup("enemigo"))
        {
            if (padre is Enemigo enemigo)
                enemigo.SetMuerto(true);

            _sprite.Play("muerte");
        }
        else
        {
            padre.QueueFree();
        }
    }

    private void OnAnimacionTerminada()
    {
        if (_sprite.Animation != "muerte") return;

        Node padre = GetParent();

        bool esJugador = false;

        foreach (Node nodo in padre.GetChildren())
        {
            if (nodo.IsInGroup("jugador"))
            {
                esJugador = true;
                break;
            }
        }

        if (esJugador)
        {
            padre.QueueFree();
        }
        else if (padre.IsInGroup("enemigo"))
        {
            // desactiva colisiones del enemigo muerto
            var colision = padre.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            if (colision != null)
            {
                colision.Disabled = true;
            }
        }
    }
}
