using Godot;
using System;

public class PlayerCredentials
{
    // basic for user credential saving and loading, may contain more stuff later, just a draft
    public string userName = "player";
    public string userPassword = "";

    // really important variable here, this will be used to tell godot where to load the files of this user
    // this will allow the user to name their save location name once in the registration form
    // it may be enabled 3 save slots later on, maybe
    //
    // this will also hold the user settings in other file at the same dir
    // so the player will be able to change stuff 
    // such as keybinds, grapichs, sound settings and more
    public string saveDir = "";

    // basically how much money a user has, used in shops
    private float userCredits = 0f;

}
