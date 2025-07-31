using Godot;
using System;

public partial class Arma : Node2D
{
    [Export] public ArmaConfig Config;
    [Export] public float shake;
     [Export] public string Grupo = "enemigo"; 
    private AnimatedSprite2D _sprite;
    private Marker2D _canon;
    public Texture2D _bala;
    public int balavel=0;

    public override void _Ready()
    {
        shake = Config.shake;
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _canon = GetNode<Marker2D>("Cañón");

        if (Config != null && _sprite != null)
        {
            _sprite.SpriteFrames = Config.Animaciones;
        }
    }

    public void Disparar(Vector2 mousePos)
    {
        if (Config == null) return;

        PackedScene bala = Config.BalaScene;
        if (bala == null) return;

        Node2D balaInst = bala.Instantiate<Node2D>();
        balaInst.Position = _canon.GlobalPosition; 
        GetTree().CurrentScene.AddChild(balaInst); 

        Vector2 direction = (mousePos - _canon.GlobalPosition).Normalized();
        string objetivoGrupo = GetParent().IsInGroup("jugador") ? "enemigo" : "jugador";
        if (_bala == null)
        {
            _bala = Config.bala;
  
        }
        if (balavel == 0)
        {
            balavel = Config.velocidad;
        }
           
        balaInst.Call("Init", direction, balavel, Config.Dano, _bala, objetivoGrupo);
        balaInst.Rotation = direction.Angle();

        _sprite?.Play("disparo");
    }
    public void Aplicar()
    {
        if (Config != null && _sprite != null)
        {
            _sprite.SpriteFrames = Config.Animaciones;
        }
    }
}
