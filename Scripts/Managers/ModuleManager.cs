using Godot;
using System;
using System.Collections.Generic;

public class ModuleManager
{
    //this class will be used to upgrade ur navi, create new modules and in the future maybe load modules made from files
    //with help of the UtilsBox func for loading files

    public static List<Module> MODULE_LIST = new List<Module>();

    public static  void loadModules(){

        //Basic kit for starters
        MODULE_LIST.Add(new Module(
                0,
                "Basic kit",
                1.0f,
                16,
                0.1f,
                8,
                512));

        //medium kit
        MODULE_LIST.Add(new Module(
                0,
                "Basic 2.o",
                2.0f,
                32,
                0.1f,
                8,
                1024));

        //medium kit 2+
        MODULE_LIST.Add(new Module(
                0,
                "Basic 2+",
                2.5f,
                48,
                0.2f,
                8,
                1024));

    }

    public static void upgradeNAVI(int moduleID, NAVI navi){



    }

}
