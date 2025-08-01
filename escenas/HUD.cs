using Godot;
using System;

public partial class HUD : CanvasLayer
{
    private Label _mensaje;
    private Timer _timer;

    public override void _Ready()
    {
        _mensaje = GetNode<Label>("Label");
		_timer = GetNode<Timer>("Timer"); 
        _timer.Connect("timeout", new Callable(this, nameof(OcultarMensaje)));
        _mensaje.Visible = false;
    }

    public void MostrarMensaje(string texto, float duracion = 2f)
    {
        _mensaje.Text = texto;
        _mensaje.Visible = true;
        _timer.Start(duracion);
    }

    private void OcultarMensaje()
    {
        _mensaje.Visible = false;
    }
}
