using DroneModel;


namespace DroneRace.Services;

public static class ThreadRace
{
    public static void RaceDronesUsingThreads(Drone[] participants)
    {
        Thread[] threads = [.. participants.Select(participant => new Thread(() => RunDrone(participant)))];

        Console.WriteLine("Race is starting");
        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
        Console.WriteLine("Race is finished");
    }

    private static void RunDrone(Drone drone)
    {
        Console.WriteLine($"Drone {drone.Name} started its run.");

        for (int i = 0; i < drone.MaxCheckpoints; i++)
        {
            Thread.Sleep(drone.DelayMs);
            Console.WriteLine($"Drone {drone.Name} reached checkpoint {i + 1}");

        }
        Console.WriteLine($"Drone {drone.Name} reached its final destination");
    }


}