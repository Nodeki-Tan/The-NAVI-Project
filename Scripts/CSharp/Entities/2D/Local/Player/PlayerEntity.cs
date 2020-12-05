using Godot;
using System;

public class PlayerEntity : NetworkEntity
{
    // Camera Variables
    float mouse_sensitivity = 0.4f;

    // Walk Variables
    const float MAX_SPEED = 32f;
    const float MAX_RUNNING_SPEED = 48f;
    const float ACCEL = 6f;
    const float DEACCEL = 4f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        // for setting the mouse invisible and stuck in the window
        //    Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _Process(float delta){

        // TEST CODE FOR CHATTING!
        //if (Input.IsActionPressed("move_back")){

            //dataBuffer = ClientMessageManager.Instance.ConstructMessage(int.Parse(Name),  "hello!!!");

            //NetworkManager.Instance.SendDataReliable(dataBuffer, NetworkManager.CHAT_MESSAGE);

        //}
        
    }

    public override void CalculateVelocity(float delta){
        move(delta);
    }

    void move(float delta){
        // Reset the direction of the player
        Vector2 direction = Vector2.Zero;
        
        float speed;
        if (Input.IsActionPressed("move_sprint"))
            speed = MAX_RUNNING_SPEED;
        else
            speed = MAX_SPEED;

        if (Input.IsActionPressed("move_forward"))
            direction.y -= 1;
        if (Input.IsActionPressed("move_back"))
            direction.y += 1;
        if (Input.IsActionPressed("move_left"))
            direction.x -= 1;
        if (Input.IsActionPressed("move_right"))
            direction.x += 1;
        
        direction = direction.Normalized();

        Vector2 temp_velocity = velocity;
            
        Vector2 target = direction * speed;
        
        float acceleration;
        if (direction.Dot(temp_velocity) > 0)
            acceleration = ACCEL;
        else
            acceleration = DEACCEL;
        
        temp_velocity = temp_velocity.LinearInterpolate(target, acceleration * delta);

        // set the player velocity before finishing!!!

        velocity.x = temp_velocity.x;
        velocity.y = temp_velocity.y;
    }

    ////////////// FIX ME ///////////////
    public void _on_Area_body_entered(Node body){
        //if (body.Name == Name)
        //    flying = true;
    }
    
    public void _on_Area_body_exited(Node body){
        //if(body.Name == Name)
        //    flying = false;
    }
    /////////////////////////////////////

}

