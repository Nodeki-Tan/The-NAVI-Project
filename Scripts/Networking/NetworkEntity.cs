using Godot;
using System;

public class NetworkEntity : KinematicBody2D
{

    protected Vector2 velocity = new Vector2();

    protected float rotationAngle = 0;

    protected object[] dataBuffer;

    // The process
    public override void _PhysicsProcess(float delta){
        
        // Get directional vectors, Move the object and apply forces
        CalculateVelocity(delta);
        
        // Update the puppet vars for the other players playing.
        // This lets them know where you are moving, 
        // and is set in the "else:" below on their machine.

        dataBuffer = ClientMessageManager.Instance.ConstructMessage(int.Parse(Name),  Transform.origin);
        NetworkManager.Instance.SendDataUnreliable(dataBuffer, NetworkManager.POSITION);


        dataBuffer = ClientMessageManager.Instance.ConstructMessage(int.Parse(Name),  velocity);
        NetworkManager.Instance.SendDataUnreliable(dataBuffer, NetworkManager.VELOCITY);


        dataBuffer = ClientMessageManager.Instance.ConstructMessage(int.Parse(Name),  Transform.Rotation);
        NetworkManager.Instance.SendDataUnreliable(dataBuffer, NetworkManager.ROTATION);
        

        if (GetTree().IsNetworkServer()){
            NetworkManager.Instance.players[int.Parse(Name)][1] = Position;
        }
        
        // Move
        velocity = MoveAndSlide(velocity, Vector2.Up, true);
    }

    // This must be overwritten in the entity for include it's specific moving patters, 
    // is used for things like the player or NPC, enemies and such,
    // but may be used for other movable objects in the future like barrels, etc.)
    public virtual void CalculateVelocity(float delta){}

    public virtual void Init(){}

}

