using Godot;
using System;
using System.Collections.Generic;

public class NetworkManager : Node
{

	#region Instance
    
    static NetworkManager instance;

    public static NetworkManager Instance { get { return instance; } }

    NetworkManager()
    {
        instance = this;
        GD.Print("hi im NetworkManager");
    }

    #endregion

    #region MESSAGE CONSTANTS
    
    public const byte POSITION = 0;

    public const byte VELOCITY = 1;

    public const byte ROTATION = 2;

    public const byte CHAT_MESSAGE = 3;

    #endregion

    // NETWORK DATA
    // Port Tip: Check the web for available ports that is not preoccupied by other important services
    // Port Tip #2: If you are the server; you may want to open it (NAT, Firewall)
    const int SERVER_PORT = 31416;
    const int MAX_PLAYERS = 256;

    public Dictionary<int, object[]> players = new Dictionary<int, object[]>();

    // Almost a struct but i couldn't be, may get new entries or changed to other type of system later.
    // The entry organization is:
    // [0] is the player nickname 
    // [1] is the position vector
    object[] self_data = { "",  Vector2.Zero};

    // SIGNALS to Main Menu (GUI)
    [Signal] delegate void RefreshLobby();
    [Signal] delegate void ServerEnded();
    [Signal] delegate void ServerError();
    [Signal] delegate void ConnectionSuccess();
    [Signal] delegate void ConnectionFail();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
		GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnected));
		GetTree().Connect("connected_to_server", this, nameof(ConnectedToServer));
		GetTree().Connect("connection_failed", this, nameof(ConnectionFailed));
		GetTree().Connect("server_disconnected", this, nameof(ServerDisconnected));
    }

    // Join a server
    public void JoinGame(string name, string ip_address){

        ClientMessageManager cmm = new ClientMessageManager();

        // Store own player name
        self_data[0] = name;
        
        // Initializing the network as server
        var host = new NetworkedMultiplayerENet();
        host.CreateClient(ip_address, SERVER_PORT);
        GetTree().NetworkPeer = host;
    }

    // Host the server
    public void HostGame(string name){

        ServerMessageManager smm = new ServerMessageManager();
        ClientMessageManager cmm = new ClientMessageManager();

        // Store own player name
        players[1] = self_data;
        self_data[0] = name;
        
        // Initializing the network as client
        var host = new NetworkedMultiplayerENet();
        host.CreateServer(SERVER_PORT, MAX_PLAYERS); // Max N players can be connected
        GetTree().NetworkPeer = host;
    }

    // Client connected with you (can be both server or client)
    public void PlayerConnected(int id){

        //send my data to that user BY BRUTE FORCE, can fail probably
        if(id == 1)
            RpcId(id, nameof(SendPlayerInfo), GetTree().GetNetworkUniqueId(), self_data);

    }

    // Client disconnected from you
    public void PlayerDisconnected(int id){
        // If I am server, send a signal to inform that an player disconnected
        if(GetTree().IsNetworkServer())
            Rpc(nameof(UnregisterPlayer), id);
    }

    // Successfully connected to server (client)
    public void ConnectedToServer(){
        players[GetTree().GetNetworkUniqueId()] = self_data;
        
        // Send signal to server that we are ready to be assigned;
        // Either to lobby or ingame
        RpcId(1, nameof(UserReady), GetTree().GetNetworkUniqueId());
    }

    // Could not connect to server (client)
    public void ConnectionFailed(){
        GetTree().NetworkPeer = null;
        EmitSignal(nameof(ConnectionFail));
    }

    // Server disconnected (client)
    public void ServerDisconnected(){
        QuitGame();
        EmitSignal(nameof(ServerEnded));
    }

    // Server receives this from players that have just connected
    [Remote] public void UserReady(int id){
        // Only the server can run this!
        if(GetTree().IsNetworkServer())
            // If we are ingame, add player to session, else send to lobby	
            if(HasNode("/root/world"))
                RpcId(id, nameof(RegisterInGame), id);
            else
                RpcId(id, nameof(RegisterAtLobby), id);
    }

    // Register yourself directly ingame
    [Remote] public void RegisterInGame(int id){
        Rpc(nameof(RegisterPlayerInGame), GetTree().GetNetworkUniqueId(), players[id]);
    }

    // Register myself with other players at lobby
    [Remote] public void RegisterAtLobby(int id){
        Rpc(nameof(RegisterPlayerAtLobby), GetTree().GetNetworkUniqueId(), players[id]);
        EmitSignal(nameof(ConnectionSuccess)); // Sends command to gui & will send player to lobby
    }

    // Register the player and jump ingame
    [Remote] public void RegisterPlayerInGame(int id, object[] info){
        RecievePlayerData(id, info);

        // This runs only once from server
        if(GetTree().IsNetworkServer())
            foreach (var peer_id in players)
                RpcId(id, nameof(RegisterPlayerInGame), peer_id.Key, players[peer_id.Key]); // Send the new player info to others
        
        // Spawn player with id 'id' and at position 'spawn_pos[id]'
        SpawnPlayer(id);
    }

    // Register player the ol' fashioned way and refresh lobby
    [Remote] public void RegisterPlayerAtLobby(int id, object[] info){
        RecievePlayerData(id, info);

        // If I am the server (not run on clients)
        // Notify lobby (GUI) about changes
        EmitSignal(nameof(RefreshLobby));
    }

    // Unregister a player, whether he is in lobby or ingame
    [RemoteSync] public void UnregisterPlayer(int id){
        GD.Print("bye bye " + players[id][0]);
        // If the game is running
        
        if(WorldManager.Instance.world != null){
            // Remove player from game
            if(WorldManager.Instance.world.GetNode("Players/" + id.ToString()) != null)
                WorldManager.Instance.world.GetNode("Players/" + id.ToString()).QueueFree();

            players.Remove(id);
        }else{
            // Remove from lobby
            players.Remove(id);
            EmitSignal(nameof(RefreshLobby));
        }

    }

    // A function to be used if needed. The purpose is to request all players in the current session.
    [Remote] public void RequestPlayersList(int id){
        if (GetTree().IsNetworkServer())
            RpcId(id, nameof(RecievePlayerInfoList), players);
    }

    [Remote] public void RecievePlayerInfoList(Dictionary<int, object[]> info){
        players = info;

        GD.Print("recieving data list!");
    }

    [Remote] public void SendPlayerInfo(int id, object[] info){
        RecievePlayerData(id, info);
    }

    public void RecievePlayerData(int id, object[] info){

        if(!players.ContainsKey(id)){

            players[id] = info;
            GD.Print("recieving " + id);

        }else{

            GD.Print("already recieved " + id);

        }

    }

    // Returns a list of players (lobby)
    public object[][] GetPlayerList(){

        object[][] array = new object[1][];
        players.Values.CopyTo(array, 0);

        return array;
    }

    // Returns your name
    public String GetPlayerName(){
        return self_data[0].ToString();
    }

    public void StartGame(){
        foreach (var peer_id in players)
            SpawnPlayer(peer_id.Key);
    }

    // Quits the game, will automatically tell the server you disconnected; neat.
    public void QuitGame(){
        GetTree().NetworkPeer = null;
        players.Clear();
    }

    [Remote] public void SpawnPlayer(int id){

        GD.Print("Spawning " + players[id][0]);
        
        // If your game have already started, we get the current reference, 
        // else we create our instance and add it to root
        if(WorldManager.Instance.world == null){
            WorldManager.Instance.SpawnWorld();
        }
        
        // Spawn! Spawn ALL the players!
        // There are only multiple players when we wait for players in lobby before starting.
        // Else we generate a random spawn point and throw him in with the other players.
        
        
        // If the new player is you
        if (id == GetTree().GetNetworkUniqueId()){

            // Create player instance
            PlayerEntity player = (PlayerEntity)AssetManager.Instance.playerScene.Instance();
            ((Node2D)player).Transform = ((Node2D)WorldManager.Instance.world.GetNode("Spawn")).Transform;
            
            if (GetTree().IsNetworkServer()){
                players[id][1] = ((Node2D)player).Transform.origin;
            }

            // Set Player ID as node name - Unique for each player!
            player.Name = id.ToString();

            GD.Print("hi host!");

            // Add the player (or you) to the world!
            WorldManager.Instance.world.GetNode("Entities").AddChild(player);
        }else{

            // Create player instance
            PuppetPlayer player = (PuppetPlayer)AssetManager.Instance.puppetPlayerScene.Instance();
            ((Node2D)player).Transform = ((Node2D)WorldManager.Instance.world.GetNode("Spawn")).Transform;
            
            if (GetTree().IsNetworkServer()){
                players[id][1] = ((Node2D)player).Transform.origin;
            }

            // Set Player ID as node name - Unique for each player!
            player.Name = id.ToString();

            // init player nameTag
            player.Init((String)players[id][0], (Vector2)players[id][1]);

            // Add the player (or you) to the world!
            WorldManager.Instance.world.GetNode("Entities").AddChild(player);
        }

    }

    // Send data to server in a Unreliable way
    public void SendDataUnreliable(object[] data, byte messageId){
        
        if(GetTree().IsNetworkServer()){
            ServerMessageManager.Instance.HandleMessage(data, messageId);
        }else{
            RpcUnreliableId(1, nameof(RecieveDataUnrelieable), data, messageId);
        }

    }

    // Send data to server in a "Reliable" way
    public void SendDataReliable(object[] data, byte messageId){
        
        if(GetTree().IsNetworkServer()){
            ServerMessageManager.Instance.HandleMessage(data, messageId);

        }else{
            RpcId(1, nameof(RecieveDataRelieable), data, messageId);
        }

    }

    [Remote] public void RecieveDataUnrelieable(object[] data, byte messageId){

        if (GetTree().IsNetworkServer()){
            ServerMessageManager.Instance.HandleMessage(data, messageId);
        }else{
            ClientMessageManager.Instance.HandleMessage(data, messageId);
        }

    }

    [Remote] public void RecieveDataRelieable(object[] data, byte messageId){

        if (GetTree().IsNetworkServer()){
            ServerMessageManager.Instance.HandleMessage(data, messageId);
        }else{
            ClientMessageManager.Instance.HandleMessage(data, messageId);
        }

    }

    

}
