using Godot;
using System;

public class PlayerEntity : NetworkEntity
{
    // Camera Variables
    float camera_angle = 0f;
    float mouse_sensitivity = 0.4f;
    Vector2 camera_change = new Vector2();

    // Walk Variables
    float gravity = -9.8f;
    const float MAX_SPEED = 8f;
    const float MAX_RUNNING_SPEED = 12f;
    const float ACCEL = 2f;
    const float DEACCEL = 6f;

    // Jumping
    float jump_height = 6f;
    bool has_contact = false;

    // Fly (for using ladders, climbing, maybe swimming)
    const float FLY_SPEED = 6f;
    const float FLY_ACCEL = 2f;
    bool flying = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        // for setting the mouse invisible and stuck in the window
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _Input(InputEvent @event){
        if (@event is InputEventMouseMotion){
            InputEventMouseMotion input = @event as InputEventMouseMotion;
            camera_change = input.Relative;
        }

    }

    public override void _Process(float delta){
        if(GetNode("Head/Camera") != null)
            aim();
    }

    public override void CalculateVelocity(float delta){
        if(GetNode("Head/Camera") != null)
            if (flying)
                fly(delta);
            else
                move(delta);
    }

    void aim(){
        if (camera_change.Length() > 0){
            (GetNode("Head") as Spatial).RotateY(Mathf.Deg2Rad(-camera_change.x * mouse_sensitivity));
                
            float change = -camera_change.y * mouse_sensitivity;

            if (change + camera_angle < 90 && change + camera_angle > -90) {
                (GetNode("Head/Camera") as Spatial).RotateX(Mathf.Deg2Rad(change));
                camera_angle += change;
            }

            camera_change = Vector2.Zero;
        }
    }

    void move(float delta){
        // Reset the direction of the player
        Vector3 direction = Vector3.Zero;

        Basis aim = (GetNode("Head/Camera") as Spatial).GlobalTransform.basis;
        
        if (Input.IsActionPressed("move_forward"))
            direction -= aim.z;
        if (Input.IsActionPressed("move_back"))
            direction += aim.z;
        if (Input.IsActionPressed("move_left"))
            direction -= aim.x;
        if (Input.IsActionPressed("move_right"))
            direction += aim.x;
        
        direction = direction.Normalized();

        if (IsOnFloor())
            has_contact = true;
        else if ((GetNode("RayCast") as RayCast).IsColliding())
            has_contact = false;
        
        if (has_contact && IsOnFloor())
            MoveAndCollide(Vector3.Down);
        
        velocity.y += gravity * delta;
        
        Vector3 temp_velocity = velocity;
        temp_velocity.y = 0;
        
        float speed;
        if (Input.IsActionPressed("move_sprint"))
            speed = MAX_RUNNING_SPEED;
        else
            speed = MAX_SPEED;
            
        Vector3 target = direction * speed;
        
        float acceleration;
        if (direction.Dot(temp_velocity) > 0)
            acceleration = ACCEL;
        else
            acceleration = DEACCEL;
        
        temp_velocity = temp_velocity.LinearInterpolate(target, acceleration * delta);
        
        velocity.x = temp_velocity.x;
        velocity.z = temp_velocity.z;
        
        if (has_contact && Input.IsActionJustPressed("jump")){
            velocity.y = jump_height;
            has_contact = false;
        }
        
    }

    void fly(float delta){
        // Reset the direction of the player
        Vector3 direction = Vector3.Zero;

        Basis aim = (GetNode("Head/Camera") as Spatial).GlobalTransform.basis;
        
        if (Input.IsActionPressed("move_forward"))
            direction -= aim.z;
        if (Input.IsActionPressed("move_back"))
            direction += aim.z;
        if (Input.IsActionPressed("move_left"))
            direction -= aim.x;
        if (Input.IsActionPressed("move_right"))
            direction += aim.x;
        
        direction = direction.Normalized();
        
        Vector3 target = direction * FLY_SPEED;
        
        velocity = velocity.LinearInterpolate(target, FLY_ACCEL * delta);

    }

    ////////////// FIX ME ///////////////
    public void _on_Area_body_entered(Node body){
        if (body.Name == Name)
            flying = true;
    }
    
    public void _on_Area_body_exited(Node body){
        if(body.Name == Name)
            flying = false;
    }
    /////////////////////////////////////

}
