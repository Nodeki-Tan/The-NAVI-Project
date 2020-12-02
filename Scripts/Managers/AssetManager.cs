using Godot;
using System;

public class AssetManager : Node
{

	#region Instance
    
    static AssetManager instance;

    public static AssetManager Instance { get { return instance; } }

    AssetManager()
    {
        instance = this;
        GD.Print("hi im AssetManager");
    }

    #endregion
    
    public PackedScene worldScene;
    public PackedScene playerScene;
    public PackedScene cameraScene;

    public PackedScene puppetPlayerScene;

    String objectPath = "res://Objects/";

    String networkObjectPath = "res://Objects/NetworkObjects/";

    String scenesPath = "res://Scenes/";

    String resFormat = ".tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//load assets
        worldScene =  LoadScene("Maps/World");
        playerScene = LoadObject("Player");
        cameraScene = LoadObject("Camera");

        puppetPlayerScene = LoadNetObject("PuppetPlayer");

	}

	public override void _Process(float delta)
	{
		
	}

    PackedScene LoadObject(String name)
    {
        return (PackedScene)ResourceLoader.Load(objectPath + name + resFormat);
    }

    PackedScene LoadNetObject(String name)
    {
        return (PackedScene)ResourceLoader.Load(networkObjectPath + name + resFormat);
    }

    PackedScene LoadScene(String name)
    {
        return (PackedScene)ResourceLoader.Load(scenesPath + name + resFormat);
    }

}
