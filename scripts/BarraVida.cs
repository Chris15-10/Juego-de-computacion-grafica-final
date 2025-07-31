using Godot;
using System;

public partial class BarraVida : CanvasLayer
{
    public ProgressBar _barraVida;

    public override void _Ready()
    {
        _barraVida = GetNode<ProgressBar>("Vida");
    }

    public void ActualizarBarra(int actual, int max)
    {
        _barraVida.MaxValue = max;
        _barraVida.Value = actual;
    }
}
