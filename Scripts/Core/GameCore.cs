using System;
using System.Threading;

public class GameCore
{
    Thread thread;

    public static bool paused = false;
    public static bool running = false;
    
    //States
    //private State gameState;
    //private State menuState;

    #region Instance
    
    static GameCore instance;

    public static GameCore Instance { get { return instance; } }

    public GameCore()
    {
        thread = new Thread(new ThreadStart(start));
        thread.Start(); 

        instance = this;
    }

    #endregion

    private void init() {
        
        Console.WriteLine("hi im GameCore");
        //gameState = new GameState();
        //menuState = new MenuState();
        //StateManager.setCurrentState(gameState);
        
    }
    
    public void Run() {
        init();
            
        long lastTime = TimeUtils.GetNanoseconds();
        double nsPerTick = 1000000000D / 60D;

        int ticks = 0;

        long lastTimer = TimeUtils.CurrentTimeMillis();
        double delta = 0;

        while (running) {

            if(!paused) {
                
                long now = TimeUtils.GetNanoseconds();
                delta += (now - lastTime) / nsPerTick;
                lastTime = now;
    
                while (delta >= 1) {
                    ticks++;
                    tick();
                    delta -= 1;
                }
    
                Thread.Sleep(2);

                //Console.WriteLine(ticks + " GameCore ticks");

                if (TimeUtils.CurrentTimeMillis() - lastTimer >= 1000) {
                    lastTimer += 1000;
                    
                    Console.WriteLine(ticks + " GameCore ticks");
                    ticks = 0;
                }
            }

        }
        
        stop();
        
    }
    
    private void tick() {
        
        /*
        if(InputHandler.isKeyPressed(Keyboard.KEY_ESCAPE) || Display.isCloseRequested()){
        MapCore.running = false;
        MeshingCore.running = false;
        running = false;
        }
        
        if(StateManager.getCurrentState() != null) {
        StateManager.getCurrentState().tick();
        }
        */

    }
    
    public void start(){
        running = true;
        Run();
    }
    
    public void stop(){
        MapCore.running = false;

        Console.WriteLine("GameCore has stopped!");
        thread.Abort();
    }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
//  public override void _Ready()
//  {
//        
//  }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

}
