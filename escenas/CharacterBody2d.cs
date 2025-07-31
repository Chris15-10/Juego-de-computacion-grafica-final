using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D

{
	[Export]
	public float Speed = 300.0f;

	[Export]
	public AnimatedSprite2D _animatedSprite2D;

	public override void _PhysicsProcess(double delta)
	{
		// Obtiene la dirección desde las teclas de movimiento
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		// Si hay movimiento, normaliza la dirección para evitar velocidad diagonal mayor
		if (direction != Vector2.Zero)
		{
			direction = direction.Normalized();
			Velocity = direction * Speed;

			// Animaciones según dirección
			_animatedSprite2D.Play("caminando");

			// Flip solo si se mueve a la izquierda o derecha
			if (direction.X != 0)
				_animatedSprite2D.FlipH = direction.X < 0;
		}
		else
		{
			Velocity = Vector2.Zero;
			_animatedSprite2D.Play("default");
		}

		MoveAndSlide();
	}
}

