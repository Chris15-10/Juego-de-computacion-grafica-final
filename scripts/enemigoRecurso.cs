using Godot;
using System;

public partial class enemigoRecurso : Resource
{
    [Export] public SpriteFrames _sprite;
    [Export] public Texture2D _bala;
    [Export] public int dano=20;
    [Export] public int dano_bala = 12;
    [Export] public float velocidad;
    [Export] public float vida;
    [Export] public float RangoDisparo;
    [Export] public float DistanciaMinima;
    [Export] public float tamano;
}
