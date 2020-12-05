using Godot;
using System;
using System.Collections.Generic;

public class ProgramManager
{
    //this class will be used to upgrade ur navi, create new modules and in the future maybe load modules made from files
    //with help of the UtilsBox func for loading files

    public static List<NAVIProgram> PROGRAM_LIST = new List<NAVIProgram>();

    public static  void loadPrograms(){

        //Basic kit for starters
        PROGRAM_LIST.Add(new NAVIProgram(
                0,
                "Basic kit",
                1.0f,
                16,
                0.1f,
                8,
                512));

    }

}
