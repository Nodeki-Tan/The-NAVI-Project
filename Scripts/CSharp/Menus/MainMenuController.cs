 using Godot;
using System;

public class MainMenuController : Node2D
{

    // CONTAINERS
    Node menuContainer;
    Node joinContainer;
    Node hostContainer;
    Node lobbyContainer;

    // Player Name
    const string PLAYER_NAME_DEFAULT = "Player";
    const string SERVER_NAME_DEFAULT = "Server";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

        menuContainer = GetNode("menu_container");
        joinContainer = GetNode("join_container");
        hostContainer = GetNode("host_container");
        lobbyContainer = GetNode("lobby_container");

        // Set default nicknames on host/join
        ((LineEdit)joinContainer.FindNode("lineedit_nickname")).Text = PLAYER_NAME_DEFAULT;
        ((LineEdit)hostContainer.FindNode("lineedit_nickname")).Text = SERVER_NAME_DEFAULT;

        // Setup Network Signaling between NetworkManager.Instance and Game UI
        NetworkManager.Instance.Connect("RefreshLobby", this, nameof(RefreshLobby));
        NetworkManager.Instance.Connect("ServerEnded", this, nameof(OnServerEnded));
        NetworkManager.Instance.Connect("ServerError", this, nameof(OnServerEerror));
        NetworkManager.Instance.Connect("ConnectionSuccess", this, nameof(OnConnectionSuccess));
        NetworkManager.Instance.Connect("ConnectionFail", this, nameof(OnConnectionFail));

        // Setup menuContainer Signaling between the button and Game UI
        menuContainer.FindNode("join_game_button").Connect("pressed", this, nameof(OnJoinGameButtonPressed));
        menuContainer.FindNode("host_game_button").Connect("pressed", this, nameof(OnHostGameButtonPressed));
        menuContainer.FindNode("quit_game_button").Connect("pressed", this, nameof(OnQuitGameButtonPressed));

        // Setup joinContainer Signaling between the button and Game UI
        joinContainer.FindNode("cancel_button").Connect("pressed", this, nameof(OnCancelButtonPressed));
        joinContainer.FindNode("connect_button").Connect("pressed", this, nameof(OnConnectButtonPressed));

        // Setup hostContainer Signaling between the button and Game UI
        hostContainer.FindNode("cancel_button").Connect("pressed", this, nameof(OnCancelButtonPressed));
        hostContainer.FindNode("continue_button").Connect("pressed", this, nameof(OnContinueButtonPressed));

        // Setup lobbyContainer Signaling between the button and Game UI
        lobbyContainer.FindNode("cancel_button").Connect("pressed", this, nameof(OnCancelLobbyButtonPressed));
        lobbyContainer.FindNode("start_game_button").Connect("pressed", this, nameof(OnStartGameButtonPressed));

    }

    // MAIN MENU - Join Game
    // Opens up the 'Connect to Server' window
    public void OnJoinGameButtonPressed(){
        ((Control)menuContainer).Hide();
        ((Control)joinContainer).Show();
    }

    // MAIN MENU - Host Game
    // Opens up the 'Choose a nickname' window
    public void OnHostGameButtonPressed(){
        ((Control)menuContainer).Hide();
        ((Control)hostContainer).Show();
    }

    // MAIN MENU - Quit Game
    public void OnQuitGameButtonPressed(){
        GetTree().Quit();
    }

    // JOIN CONTAINER - Connect
    // Attempts to connect to the server
    // If successful, continue to Lobby or jump in-game (if running)
    public void OnConnectButtonPressed(){
        // Check entered IP address for errors
        string ipAddress = ((LineEdit)joinContainer.FindNode("lineedit_ip_address")).Text;

        if(!ipAddress.IsValidIPAddress()){
            ((Label)joinContainer.FindNode("label_error")).Text = "Invalid IP address";
            return;
        }

        // Check nickname for errors
        string playerName = ((LineEdit)joinContainer.FindNode("lineedit_nickname")).Text;

        if(playerName == ""){
            ((Label)joinContainer.FindNode("label_error")).Text = "Nickname cannot be empty";
            return;
        }

        // Clear error (if any)
        ((Label)joinContainer.FindNode("label_error")).Text = "";

        // Connect to server
        NetworkManager.Instance.JoinGame(playerName, ipAddress);

        // While we are attempting to connect, disable button for 'continue'
        ((Button)joinContainer.FindNode("connect_button")).Disabled = true;

    }

    // HOST CONTAINER - Continue (from choosing a nickname)
    // Opens the server for connectivity from clients
    public void OnContinueButtonPressed(){
        // Check if nickname is valid
        string playerName = ((LineEdit)hostContainer.FindNode("lineedit_nickname")).Text;

        if(playerName == ""){
            ((Label)hostContainer.FindNode("label_error")).Text = "Nickname cannot be empty";
            return;
        }

        // Clear error (if any)
        ((Label)hostContainer.FindNode("label_error")).Text = "";

        // Establish network
        NetworkManager.Instance.HostGame(playerName);

        // Refresh Player List (with your own name)
        RefreshLobby();

        // Toggle to Lobby
        ((Control)hostContainer).Hide();
        ((Control)lobbyContainer).Show();
        ((Button)lobbyContainer.FindNode("start_game_button")).Disabled = false;

    }

    // LOBBY CONTAINER - Starts the Game
    public void OnStartGameButtonPressed(){
        NetworkManager.Instance.StartGame();
    }

    // LOBBY CONTAINER - Cancel Lobby
    // (The only time you are already connected from main menu)
    public void OnCancelLobbyButtonPressed(){
        // Toggle containers
        ((Control)lobbyContainer).Hide();
        ((Control)menuContainer).Show();

        // Disconnect NetworkManager.Instance
        NetworkManager.Instance.QuitGame();

        // Enable buttons
        ((Button)joinContainer.FindNode("connect_button")).Disabled = false;

    }

    // ALL - Cancel (from any container)
    public void OnCancelButtonPressed(){
        ((Control)menuContainer).Show();
        ((Control)joinContainer).Hide();

        ((Label)joinContainer.FindNode("label_error")).Text = "";
        
        ((Control)hostContainer).Hide();
        ((Label)hostContainer.FindNode("label_error")).Text = "";
    }

    // Refresh Lobby's player list
    // This is run after we have gotten updates from the server regarding new players
    public void RefreshLobby(){
        // Get the latest list of players from NetworkManager.Instance
        object[][] playerList = NetworkManager.Instance.GetPlayerList();

        // Add the updated player_list to the itemlist
        ItemList itemlist = lobbyContainer.FindNode("itemlist_players") as ItemList;
        itemlist.Clear();
        itemlist.AddItem(NetworkManager.Instance.GetPlayerName() + " (YOU)"); // Add yourself to the top

        // Add every other player to the list
        foreach (object[] player in playerList){
            if (player[0] != NetworkManager.Instance.GetPlayerName())
            {
                itemlist.AddItem(player[0].ToString());
            }
        }

        // If you are not the server, we disable the 'start game' button
        if(!GetTree().IsNetworkServer())
            ((Button)lobbyContainer.FindNode("start_game_button")).Disabled = false;

    }

    // Handles what to happen after server ends
    public void OnServerEnded(){
        ((Control)lobbyContainer).Hide();
        ((Control)joinContainer).Hide();

        ((Button)joinContainer.FindNode("connect_button")).Disabled = false;

        ((Control)menuContainer).Show();

        // If we are ingame, remove world from existence!
        if(HasNode("/root/world")){
            ((Node2D)GetNode("/root/main_menu")).Show(); // Enable main menu
            GetNode("/root/world").QueueFree(); // Terminate world
        }

    }

    public void OnServerEerror(){
        GD.Print("OnServerEerror: Unknown error");
    }

    public void OnConnectionSuccess(){
        ((Control)joinContainer).Hide();
        ((Control)lobbyContainer).Show();
    }

    public void OnConnectionFail(){
        // Display error telling the user that the server cannot be connected
        ((Label)joinContainer.FindNode("label_error")).Text = "Cannot connect to server, try again or use another IP address";

        // Enable continue button again
        ((Button)joinContainer.FindNode("connect_button")).Disabled = false;
        
    }

}

