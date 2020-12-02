using System;
using System.Threading;
using System.Collections.Generic;
public class MapCore
{

	public Thread thread;

	public static bool paused = false;
	public static bool running = false;

	#region Instance
    
    static MapCore instance;

    public static MapCore Instance { get { return instance; } }

    public MapCore()
    {
        thread = new Thread(new ThreadStart(start));
        thread.Start(); 

        instance = this;
    }

    #endregion

    private void init() {
        
        Console.WriteLine("hi im MapCore");

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

                if (TimeUtils.CurrentTimeMillis() - lastTimer >= 1000) {
                    lastTimer += 1000;
                    
                    Console.WriteLine(ticks + " MapCore ticks");
                    ticks = 0;
                }
            }

        }
        
        stop();
        
    }
	
	public void start(){
		running = true;
        Run();
	}
	
	public void stop(){

		Console.WriteLine("MapCore has stopped!");
        thread.Abort();
	}
	
	public void tick() {


		
	}
	
}
