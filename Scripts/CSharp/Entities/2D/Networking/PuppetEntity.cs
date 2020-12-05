using Godot;
using System;

public class PuppetEntity : KinematicBody2D
{
    // Basically each "[Puppet] var" is updated by other players, which make it look like they are moving 
    // in your screen. They are set using Rset("var_name", data), 
    // where "var_name" is the name of the "[Puppet] var" you got here.
    public Vector2 puppetPos = new Vector2();
    public Vector2 puppetVelocity = new Vector2();
    public float puppetRotationAngle = 0;

    protected Vector2 velocity = new Vector2();

    protected float rotationAngle = 0;

    public virtual void Init(){}

}
