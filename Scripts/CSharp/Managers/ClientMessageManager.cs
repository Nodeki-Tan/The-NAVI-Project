using Godot;
using System;

public class ClientMessageManager : Node
{

    #region Instance
    
    static ClientMessageManager instance;

    public static ClientMessageManager Instance { get { return instance; } }

    public ClientMessageManager()
    {
        instance = this;
    }

    #endregion

    public void HandleMessage(object[] data, byte messageId){

        int senderID = (int)data[0];

        switch (messageId)
        {
            
            case NetworkManager.POSITION:

                if(WorldManager.Instance.world != null){
                    // Check if player exists
                    if(WorldManager.Instance.world.GetNode("Entities/" + senderID.ToString()) != null){
                        //Set the player pos
                        ((PuppetEntity)WorldManager.Instance.world.GetNode("Entities/" + senderID.ToString())).puppetPos = (Vector2)data[1];
                    }
                }

                break;

            case NetworkManager.VELOCITY:

                if(WorldManager.Instance.world != null){
                    // Check if player exists
                    if(WorldManager.Instance.world.GetNode("Entities/" + senderID.ToString()) != null){
                        //Set the player pos
                        ((PuppetEntity)WorldManager.Instance.world.GetNode("Entities/" + senderID.ToString())).puppetVelocity = (Vector2)data[1];
                    }
                }

                break;

            case NetworkManager.ROTATION:

                if(WorldManager.Instance.world != null){
                    // Check if player exists
                    if(WorldManager.Instance.world.GetNode("Entities/" + senderID.ToString()) != null){
                        //Set the player pos
                        ((PuppetEntity)WorldManager.Instance.world.GetNode("Entities/" + senderID.ToString())).puppetRotationAngle = (float)data[1];
                    }
                }

                break;

            case NetworkManager.CHAT_MESSAGE:

                GD.Print(data[1].ToString());

                break;

            default:
                break;
        }

    }

    public object[] ConstructMessage(params object[] list){

        return list;

    }

}
