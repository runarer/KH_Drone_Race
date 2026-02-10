
using DroneModel;
using DroneRace.Services;

using ControlTowerAPI.Listener;

namespace DroneRaceApp;

class Program
{
    private readonly ControlTower _tower = new("http://localhost:6060/");

    private readonly Drone[] _drones =
    [
        new("Big Berta",12,15),
        new("Tiny Timmy",18,10),
    ];
    static async Task Main()
    {
        Program app = new();
        await app.Run();
    }

    public async Task Run()
    {


        bool running = true;

        while (running)
        {
            DisplayMenu();

            var choice = Console.ReadKey(intercept: true).Key;

            switch (choice)
            {
                case ConsoleKey.D1:
                    ThreadRace.RaceDronesUsingThreads(_drones);
                    break;
                case ConsoleKey.D2:
                    TaskRace.RaceDronesUsingTasks(_drones).Wait();
                    break;
                case ConsoleKey.D3:
                    await AsyncRace.RaceDronesUsingAsync(_drones);
                    break;
                case ConsoleKey.D4:
                    try
                    {
                        TaskRaceWithCrash.RaceDronesUsingTasks(_drones).Wait();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case ConsoleKey.D5:
                    try
                    {
                        await AsyncRace.RaceDronesUsingAsync(_drones);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case ConsoleKey.D6:
                    RunTower();
                    break;
                case ConsoleKey.D7:
                    StopTower();
                    break;
                case ConsoleKey.D8:
                    RunTowerRace();
                    break;
                case ConsoleKey.Q:
                    running = false;
                    break;
            }
            if (running)
                PressToContinue();
        }
    }

    private void RunTower()
    {
        _ = _tower.StartListener();
        Console.Clear();
        Console.WriteLine("File TestTower.http in DroneRaceApp folder can be used to test");
        Console.WriteLine("connections to the server with VSCode extensions like \"Rest Client\"\n");
    }

    private void StopTower()
    {
        Console.Clear();
        _tower.Stop();
        Console.WriteLine("\nControl Tower stopped\n");
    }

    private void RunTowerRace()
    {

    }

    private static void PressToContinue()
    {

        Console.WriteLine("\nPress any key to continue.");
        Console.ReadKey(intercept: true);
    }


    private static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("""

    ------------- Select Race -------------

    1. Part A: Thread Race

    2. Part B: Task with TaskCompletionSource

    3. Part C: Async/Await

    ---------- Simulated Crashes ----------

    4. Part B: With A simulated Crash

    5. Part C: With A Simulated Crash

    ---------------------------------------
    
    6. Start the Control Tower on http://localhost:6060/

    7. Stop the Control Tower
       
    ---------------------------------------

    8. Race with control tower

    ---------------------------------------
    
    Q. Quit

    Enter your choice: 
    """);
    }
}

