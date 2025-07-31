using Godot;
using System;

public partial class SpawnerEnemigos : Area2D
{
    [Export] public PackedScene[] enemigos; // spawnean tanto enemigos shooter como cuerpo a cuerpo
    [Export]public Areapuertas mi_area;
    [Export] public int maximo = 3;
    [Export] public enemigoRecurso[] recursos;
    [Export] public float tiempo = 1.5f; // tiempo de spawn
    public CollisionShape2D _colision;
    public Timer _timer;
    private RandomNumberGenerator _rng = new();
    private bool inside = false; // si el jugador esta dentro del àrea
    private int cont_enemigos = 0;

    public override void _Ready()
    {
        _timer = new Timer();
        AddChild(_timer);
        _timer.WaitTime = tiempo;
        _timer.Timeout += SpawnEnemigo;
        _colision=GetNode<CollisionShape2D>("CollisionShape2D");
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("jugador"))
        {
            inside = true;
            _timer.Start();
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body.IsInGroup("jugador"))
        {
            inside = false;
            _timer.Stop(); // se detiene el spawner
        }
    }

    private void SpawnEnemigo()
    {
        if (!inside || GetTree().Paused) return;
        if (cont_enemigos >= maximo)
        {
            _timer.Stop();
            return;
        }

        if (_colision.Shape is not RectangleShape2D rectangle)
            return;

        Vector2 size = rectangle.Size * GlobalScale;
        Vector2 topLeft = GlobalPosition - size / 2f;
        Rect2 rect = new Rect2(topLeft, size);


        Vector2 random_pos = Vector2.Zero;
        const int maxIntentos = 10;
        bool posicionValida = false;

        var spaceState = GetWorld2D().DirectSpaceState;

        for (int i = 0; i < maxIntentos; i++)
        {
            random_pos = new Vector2(
                _rng.RandfRange(rect.Position.X, rect.End.X),
                _rng.RandfRange(rect.Position.Y, rect.End.Y)
            );

            var parameters = new PhysicsPointQueryParameters2D
            {
                Position = random_pos,
                CollideWithAreas = false,
                CollideWithBodies = true
            };

            var result = spaceState.IntersectPoint(parameters);

            if (result.Count == 0)
            {
                posicionValida = true;
                break;
            }
        }

        if (!posicionValida)
        {
            GD.Print("No se pudo encontrar una posición válida para spawnear.");
            return;
        }

        var escena_enemigo = enemigos[_rng.RandiRange(0, enemigos.Length - 1)];
        var recurso = recursos[_rng.RandiRange(0, recursos.Length - 1)];
        var instancia = escena_enemigo.Instantiate();

        if (instancia is Enemigo enemigo)
        {
            enemigo.GlobalPosition = random_pos;
            GetParent().AddChild(enemigo);
            enemigo.cargar(recurso);
            cont_enemigos++;
            enemigo.miarea = mi_area; 
            enemigo.Connect(Enemigo.SignalName.Muerto, Callable.From(() =>
            {
                enemigo.miarea?.RegistrarMuerte();
            }));
            if (enemigo._anim != null)
            {
                enemigo._anim.Play("instancia");
                enemigo._anim.AnimationFinished += (StringName name) =>
                {
                    if (name == "instancia")
                    {
                        enemigo.Activar();
                    }
                };
            }
        }
        else
        {
            GD.PrintErr("El enemigo no es del tipo Enemigo");
        }
    }
    private void OnEnemigoMuerto()
    {
        var area = GetTree().GetFirstNodeInGroup("controlador_puertas") as Areapuertas;
        area?.RegistrarMuerte();
    }
}


