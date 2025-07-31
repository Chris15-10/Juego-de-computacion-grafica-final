using Godot;
using System;

public partial class DanoEnemigo : Node2D
{
    private Label _label;
    private Timer _timer;
    private float _velocidad = 25f;

    public override void _Ready()
    {
        _label = GetNode<Label>("Texto");
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    public override void _Process(double delta)
    {
        Position += new Vector2(0, (float)-_velocidad * (float)delta);
    }

    public void Mostrar(int cantidad, float porcentajeVida)
    {
        _label = GetNode<Label>("Texto");
        _label.Text = cantidad.ToString();

    
        _label.Modulate = new Color("#EEE8AA"); 
        
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
