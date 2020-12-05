using Godot;
using System;

public class NAVIProgram
{
    //if u are not me, nodeki, and ur reading this, as simple words this is a NAVI module, is just a modifier
    //for now is universal, but later diferent modules will affect diferent parts and will be interchangables(is that a word?)
    private int ID;

    private float cpuSpeed;  //in GHz/s
    private int RAM; //in "megabytes"

    private float gpuSpeed; //in GHz/s
    private int VRAM; //in "megabytes"

    private int ROM; //in "megabytes"
    
    private String moduleName;

    public NAVIProgram(int ID, string moduleName, float cpuSpeed, int RAM, float gpuSpeed, int VRAM, int ROM)
    {
        this.ID = ID;
        this.moduleName = moduleName;
        this.cpuSpeed = cpuSpeed;
        this.RAM = RAM;
        this.gpuSpeed = gpuSpeed;
        this.VRAM = VRAM;
        this.ROM = ROM;
    }
    
}
