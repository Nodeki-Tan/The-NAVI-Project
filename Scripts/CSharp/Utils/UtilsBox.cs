using System.Text;
using Godot;

public class UtilsBox
{
    // here ill put helper functions that doesnt need its own class
    // such as basic file loading and saving, or "buffer" making

    const string SEPARATOR = "#_#";
    const string DATA_PATH = "user://saves/";

    const string PLAYER_PATH = DATA_PATH + "PlayerData/";

    const string NAVI_PATH = PLAYER_PATH + "NaviData/";
    const string NAVI_EXTENSION = "NAVI";

    // creates a universal purpose save file, its generic, for now uses NAVI format
    public static void createDataFile(object[] contents, string saveName)
    {

        Directory dir = new Directory();

        if (!dir.DirExists(DATA_PATH))
        {
            dir.MakeDirRecursive(DATA_PATH);

            if (!dir.DirExists(NAVI_PATH))
            {
                dir.MakeDirRecursive(NAVI_PATH);
            }

        }

        GD.Print("saving " + saveName + " to file...");

        for (int i = 0; i < contents.Length; i++)
        {
            GD.Print(contents[i].ToString() + "/n");
        }

        string value = string.Join(SEPARATOR, contents);

        string path = NAVI_PATH + saveName + "." + NAVI_EXTENSION;

        File file = new File();
        Error error = file.OpenEncryptedWithPass(path, File.ModeFlags.Write, "L E T S  A L L  L O V E  L A I N");

        if(error == Error.Ok)
            file.StoreVar(value);
            file.Close();

        GD.Print("saved " + saveName + " successfull!");

    }

    public static object[] loadDataFile(string saveName)
    {

        Directory dir = new Directory();

        if (!dir.DirExists(DATA_PATH))
        {
            dir.MakeDirRecursive(DATA_PATH);

            if (!dir.DirExists(NAVI_PATH))
            {
                dir.MakeDirRecursive(NAVI_PATH);
            }

        }

        GD.Print("loading " + saveName + " from file...");

        string buffer = "";

        string path = NAVI_PATH + saveName + "." + NAVI_EXTENSION;

        File file = new File();

        if(file.FileExists(path)){

            Error error = file.OpenEncryptedWithPass(path, File.ModeFlags.Read, "L E T S  A L L  L O V E  L A I N");

            if(error == Error.Ok){
                buffer = file.GetVar().ToString();
                file.Close();
            }else{
                GD.Print("loading " + saveName + " failed because of [" + error + "] error code!!!");

                return null;
            }

        }else{
            GD.Print("loading " + saveName + " failed because file doesnt exist!");

            return null;
        }

        object[] obj = buffer.Split(SEPARATOR);

        GD.Print("loaded contents of " + saveName + " printing contents...");

        for (int i = 0; i < obj.Length; i++)
        {
            GD.Print(obj[i].ToString() + "/n");
        }
        
        GD.Print("those are the contents of " + saveName + " successfully loaded!");

        return obj;

    }

    public static object[] ConstructObjectBuffer(params object[] list){

        return list;

    }

}

