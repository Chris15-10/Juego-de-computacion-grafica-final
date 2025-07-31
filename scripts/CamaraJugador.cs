using Godot;
using System;

public partial class CamaraJugador : Camera2D
{
    private float _shakeStrength = 0f;
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _rng.Randomize();
        
        if (GetParent() is CharacterBody2D parent)
        {
            Vector2 z = Zoom / parent.Scale;
            Zoom = z;
        }
    }

    public override void _Process(double delta)
    {
        if (_shakeStrength > 0)
        {
            Offset = new Vector2(
                _rng.RandfRange(-_shakeStrength, _shakeStrength),
                _rng.RandfRange(-_shakeStrength, _shakeStrength)
            );
            _shakeStrength = Mathf.MoveToward(_shakeStrength, 0, (float)delta * 20f);
        }
        else
        {
            Offset = Vector2.Zero;
        }
    }

    public void Shake(float strength = 5f)
    {
        _shakeStrength = strength;
    }
}
