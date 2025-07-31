using Godot;
using System;

public partial class PersonajeConfig : Resource
{
    [Export] public float MoveSpeed = 300.0f;
    [Export] public SpriteFrames Animaciones;

}
