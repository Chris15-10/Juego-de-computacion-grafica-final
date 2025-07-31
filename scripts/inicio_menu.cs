using Godot;
using System;

public partial class inicio_menu : Control
{
    public override void _Ready()
    {
        GetNode<Button>("VBoxContainer/Iniciar Partida").Pressed += OnIniciarPressed;
        GetNode<Button>("VBoxContainer/Seleccion de Jugador").Pressed += OnSeleccionPressed;
        GetNode<Button>("VBoxContainer/Salir").Pressed += OnSalirPressed;
    }

    private void OnIniciarPressed()
    {
        GetTree().ChangeSceneToFile("res://escenas/mapa nivel 1.tscn");
    }

    private void OnSeleccionPressed()
    {
        GD.Print("Función de selección de jugador aún no implementada.");
    }

    private void OnSalirPressed()
    {
        GetTree().Quit();
    }
}
