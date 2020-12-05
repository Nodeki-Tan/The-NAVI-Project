using Godot;
using System;

public class KinematicPuppetEntity : PuppetEntity
{
    // The process
    public override void _PhysicsProcess(float delta){
       
        Position = puppetPos;
        rotationAngle = puppetRotationAngle;
        velocity = puppetVelocity;

        if (GetTree().IsNetworkServer()){
            NetworkManager.Instance.players[int.Parse(Name)][1] = Position;
        }
        
        // Move
        velocity = MoveAndSlide(velocity, Vector2.Up, true);
    }

}
