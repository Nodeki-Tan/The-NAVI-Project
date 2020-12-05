using Godot;
using System;

public class MainMenu : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GameCore core = new GameCore();
    }

    public override void _Notification(int what)
    {
        if (what == MainLoop.NotificationWmQuitRequest)
            GameCore.running = false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

