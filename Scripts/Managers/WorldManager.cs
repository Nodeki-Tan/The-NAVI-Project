using Godot;
using System;

public class WorldManager : Node
{

	#region Instance
    
    static WorldManager instance;

    public static WorldManager Instance { get { return instance; } }

    WorldManager()
    {
        instance = this;
        GD.Print("hi im WorldManager");
    }

    #endregion

    public Node world;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void _Process(float delta)
	{
		
	}

    public void SpawnWorld(){
        if(HasNode("/root/world")){
            world = GetNode("/root/world");
        }else{
            world = AssetManager.Instance.worldScene.Instance();
            GetTree().Root.AddChild(world);

            ((Node2D)GetNode("/root/main_menu")).Hide(); // Away with the menu! AWAY I SAY!
        }
    }

}

