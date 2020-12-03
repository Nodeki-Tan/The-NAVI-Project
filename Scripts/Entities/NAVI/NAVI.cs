using Godot;
using System;

public class NAVI
{
    // for now is basically a module, but in future it will hold other important data
    // such as a list of programs "installed" and other NAVI related stuff(instance related, for functions see ModuleManager.cs)

    private float cpuSpeed;  //in GHz/s
    private int RAM; //in "megabytes"

    private float gpuSpeed; //in GHz/s
    private int VRAM; //in "megabytes"

    private int ROM; //in "megabytes"

    public NAVI(float cpuSpeed, int RAM, float gpuSpeed, int VRAM, int ROM)
    {
        this.cpuSpeed = cpuSpeed;
        this.RAM = RAM;
        this.gpuSpeed = gpuSpeed;
        this.VRAM = VRAM;
        this.ROM = ROM;
    }

}
