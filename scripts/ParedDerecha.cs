using Godot;
using System;

public partial class ParedDerecha : StaticBody2D
{

	public AnimationPlayer _anim;
	public CollisionShape2D _colision;
	public Texture2D pared1;
	public Texture2D pared2;
	public override void _Ready()
	{
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		_colision = GetNode<CollisionShape2D>("CollisionShape2D");
		_colision.SetDeferred("disabled", true);
		 
	}

	public void cerrar()
	{
		_anim.Play("cerrar");
		_colision.SetDeferred("disabled", false);
		GD.Print("se cierra la puerta derecha ");

	}
	public void Abrir()
	{
		GD.Print("se abre la puerta derecha ");
		_anim.Play("abrir");
		_colision.SetDeferred("disabled", true);
	}
}
