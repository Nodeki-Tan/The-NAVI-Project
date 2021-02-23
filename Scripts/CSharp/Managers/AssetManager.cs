using Godot;
using System;
using System.Collections.Generic;

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

    String localObjectPath = "res://Objects/Local/";

    String networkObjectPath = "res://Objects/Network/";

    String Object2DPath = "2D/";

    String Object3DPath = "3D/";

    String scenesPath = "res://Scenes/";

    String resFormat = ".tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//load assets
        worldScene =  LoadScene("Maps/World");

        playerScene = LoadLocalObject("Player/Player", true);
        cameraScene = LoadLocalObject("Player/Camera", true);

        puppetPlayerScene = LoadNetObject("Player/PuppetPlayer", true);

	}

	public override void _Process(float delta)
	{
		
	}

    PackedScene LoadLocalObject(String name, bool is2D)
    {
        if(is2D)
            return (PackedScene)ResourceLoader.Load(localObjectPath + Object2DPath + name + resFormat);
        else
            return (PackedScene)ResourceLoader.Load(localObjectPath + Object3DPath + name + resFormat);
    }

    PackedScene LoadNetObject(String name, bool is2D)
    {
        if(is2D)
            return (PackedScene)ResourceLoader.Load(networkObjectPath + Object2DPath + name + resFormat);
        else
            return (PackedScene)ResourceLoader.Load(networkObjectPath + Object3DPath + name + resFormat);
    }

    PackedScene LoadScene(String name)
    {
        return (PackedScene)ResourceLoader.Load(scenesPath + name + resFormat);
    }

    public void createDataFile(string saveName, params object[] list)
    {
        object[] data = UtilsBox.ConstructObjectBuffer(list);

        UtilsBox.createDataFile(data, saveName);
    }

    public String loadDataFile(string saveName)
    {

        object[] bufferData = UtilsBox.loadDataFile(saveName);

        for (int i = 0; i < bufferData.Length; i++)
        {
            //data.Add(bufferData[i]);
        }

        return "data";
    }



}
