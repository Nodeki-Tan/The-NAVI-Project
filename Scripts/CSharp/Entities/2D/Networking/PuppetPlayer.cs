using Godot;
using System;

public class PuppetPlayer : KinematicPuppetEntity
{

    public void Init(String nickname, Vector2 startPosition)
    {

        RichTextLabel text =  (RichTextLabel)GetNode("Nametag").GetChild(0);
        text.Text = nickname;
        Position = startPosition;

    }

}
