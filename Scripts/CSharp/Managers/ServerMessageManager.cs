using Godot;
using System;

public class ServerMessageManager
{
    
    #region Instance
    
    static ServerMessageManager instance;

    public static ServerMessageManager Instance { get { return instance; } }

    public ServerMessageManager()
    {
        instance = this;
    }

    #endregion

    public void HandleMessage(object[] data, byte messageId){

        int senderID = (int)data[0];

        switch (messageId)
        {
            
            case NetworkManager.POSITION:

                NetworkManager.Instance.players[senderID][1] = data[1];

                // This runs only once from server
                foreach (var peer_id in NetworkManager.Instance.players)
                    if(peer_id.Key != senderID && peer_id.Key != 1){
                        NetworkManager.Instance.RpcUnreliableId(peer_id.Key, nameof(NetworkManager.Instance.RecieveDataUnrelieable), data, messageId); // Send the new player info to others
                    };

                if(senderID != 1){
                    ClientMessageManager.Instance.HandleMessage(data, NetworkManager.POSITION);
                }
                
                break;

            case NetworkManager.VELOCITY:

                // This runs only once from server
                foreach (var peer_id in NetworkManager.Instance.players)
                    if(peer_id.Key != senderID && peer_id.Key != 1){
                        NetworkManager.Instance.RpcUnreliableId(peer_id.Key, nameof(NetworkManager.Instance.RecieveDataUnrelieable), data, messageId); // Send the new player info to others
                    };

                if(senderID != 1){
                    ClientMessageManager.Instance.HandleMessage(data, NetworkManager.VELOCITY);
                }

                break;

            case NetworkManager.ROTATION:

                // This runs only once from server
                foreach (var peer_id in NetworkManager.Instance.players)
                    if(peer_id.Key != senderID && peer_id.Key != 1){
                        NetworkManager.Instance.RpcUnreliableId(peer_id.Key, nameof(NetworkManager.Instance.RecieveDataUnrelieable), data, messageId); // Send the new player info to others
                    };

                if(senderID != 1){
                    ClientMessageManager.Instance.HandleMessage(data, NetworkManager.ROTATION);
                }

                break;

            case NetworkManager.CHAT_MESSAGE:

                GD.Print(data[1].ToString());

                // This runs only once from server
                foreach (var peer_id in NetworkManager.Instance.players)
                    if(peer_id.Key != senderID && peer_id.Key != 1){
                        NetworkManager.Instance.RpcId(peer_id.Key, nameof(NetworkManager.Instance.RecieveDataUnrelieable), data, messageId); // Send the new player info to others
                    };

                
                ClientMessageManager.Instance.HandleMessage(data, NetworkManager.CHAT_MESSAGE);

                break;

            default:
                break;
        }

    }

}
