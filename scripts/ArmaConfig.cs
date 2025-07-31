using Godot;
using System;

public partial class ArmaConfig : ObjetoConfig
{
    [Export] public int velocidad = 700;
    [Export] public SpriteFrames Animaciones;
    [Export] public int Dano = 10;
    [Export] public PackedScene BalaScene;
    [Export] public float CadenciaDisparo = 0.5f;
    [Export] public Texture2D bala;
    [Export] public float shake=0.5f;
}
