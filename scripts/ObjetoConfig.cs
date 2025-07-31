using Godot;
using System;

public abstract partial class ObjetoConfig : Resource
{
    [Export] public Texture2D Icono;
    [Export] public string GrupoItem = "default";
}