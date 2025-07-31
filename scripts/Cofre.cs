using Godot;
using System;

public partial class Cofre : Area2D
{
    [Export] public CofreConfig Config;
    [Export] public ObjetoLootable[] ObjetosPosibles;
    private Label _mensaje;
    private AnimatedSprite2D _sprite;
    private Texture2D _icon1;
    private Texture2D _icon2;
    private bool _jugadorDentro = false;
    private bool _abierto = false;
    private AnimationPlayer _anim;

    public override void _Ready()
    {
        _mensaje = GetNode<Label>("Mensaje");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _icon1 = GetNode<Sprite2D>("Sprite2D").Texture;
        _icon2 = GetNode<Sprite2D>("Sprite2D2").Texture;
        _anim = GetNode<AnimationPlayer>("AnimationPlayer");

        _mensaje.Visible = false;
        if (Config != null)
        {
            _sprite.SpriteFrames = Config.Animaciones;
        }

        for (int i = 0; i < ObjetosPosibles.Length; i++)
        {
            var item = ObjetosPosibles[i];
            var icono = item.config;
            if (i % 2 != 0)
            {
                _icon1 = icono.Icono;
                GetNode<Sprite2D>("Sprite2D2").Texture = _icon1;
            }
            else
            {
                _icon2 = icono.Icono;
                GetNode<Sprite2D>("Sprite2D").Texture = _icon2;
            }
        }
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        if (_jugadorDentro && !_abierto && Input.IsActionJustPressed("x"))
        {
            Abrir();
        }
    }

    private void Abrir()
    {
        _abierto = true;
        _anim.Play("abierto");
        _mensaje.Visible = false;
    }

    private void InstanciarObjetos()
    {
        int direccion = 1; 

        for (int i = 0; i < ObjetosPosibles.Length; i++)
        {
            var obj = ObjetosPosibles[i];

            if (obj.Escena == null) continue;

            var instancia = obj.Escena.Instantiate();
            if (instancia is Item item)
            {
                Vector2 offset = new Vector2(40 * direccion * this.Scale.X, 35 * this.Scale.Y);
                item.Position = GlobalPosition + offset - new Vector2(0,-10);
                item.Config = obj.config;
                GetParent().AddChild(item);
                item.ZAsRelative = true;
                item.ZIndex = this.ZIndex - 1; 
                item.YSortEnabled = true;
                direccion *= -1; 
            }
        }
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("Body entró:", body.Name);
        if (body is Personaje1)
        {
            _jugadorDentro = true;
            if (!_abierto)
            {
                _mensaje.Visible = true;
            }
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Personaje1)
        {
            _jugadorDentro = false;
            _mensaje.Visible = false;
        }
    }
}
