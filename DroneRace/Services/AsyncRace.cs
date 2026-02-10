using DroneModel;

namespace DroneRace.Services;

public class AsyncRace
{

    public static async Task RaceDronesUsingAsync(Drone[] drones)
    {
        Task<string>[] tasks = [.. drones.Select(RunDrone)];

        var droneNames = await Task.WhenAll(tasks);
        foreach (var name in droneNames)
            Console.WriteLine($"Drone {name} finished");
    }

    private static async Task<string> RunDrone(Drone drone)
    {
        Console.WriteLine($"Drone {drone.Name} starts");

        for (int i = 0; i < drone.MaxCheckpoints; i++)
        {
            await Task.Delay(drone.DelayMs);
            Console.WriteLine($"Drone {drone.Name} has reached checkpoint {i + 1}");
        }
        Console.WriteLine($"Drone {drone.Name} has completed");
        return drone.Name;
    }

}