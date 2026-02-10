
using DroneRace.Model;
using DroneRace.Services;

using ControlTowerAPI.Listener;


await Run();

static async Task Run()
{
    Drone[] drones =
    [
        new("Big Berta",12,15),
        new("Tiny Timmy",18,10),
        // new("Robust Roberta",9,20),
        // new("Quick Quin",10,18),
        // new("Two Ton Tony",15,12),
    ];

    bool running = true;

    while (running)
    {
        DisplayMenu();

        var choice = Console.ReadKey(intercept: true).Key;

        switch (choice)
        {
            case ConsoleKey.D1:
                ThreadRace.RaceDronesUsingThreads(drones);
                break;
            case ConsoleKey.D2:
                TaskRace.RaceDronesUsingTasks(drones).Wait();
                break;
            case ConsoleKey.D3:
                await AsyncRace.RaceDronesUsingAsync(drones);
                break;
            case ConsoleKey.D4:
                try
                {
                    TaskRaceWithCrash.RaceDronesUsingTasks(drones).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case ConsoleKey.D5:
                try
                {
                    await AsyncRace.RaceDronesUsingAsync(drones);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case ConsoleKey.Q:
                running = false;
                break;
        }
        if (running)
            PressToContinue();
    }
}

static void PressToContinue()
{
    Console.WriteLine("\nPress any key to continue.");
    Console.ReadKey(intercept: true);
}


static void DisplayMenu()
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
    
    Q. Quit

    Enter your choice: 
    """);
}

