using DroneRace.Model;

namespace DroneRace.Services;

public static class TaskRaceWithCrash
{
    public static Task RaceDronesUsingTasks(Drone[] participants)
    {
        Task<string>[] tasks = [.. participants.Select(SetupTscForDrone)];

        var mainTsc = new TaskCompletionSource();

        Task.WhenAll(tasks).ContinueWith(allTasks =>
        {
            foreach (var name in allTasks.Result)
            {
                Console.WriteLine($"Drone {name} has finished.");
            }
            mainTsc.SetResult();
        });
        return mainTsc.Task;
    }

    private static Task<string> SetupTscForDrone(Drone drone)
    {
        var tsc = new TaskCompletionSource<string>();

        Task.Run(() =>
        {
            try
            {
                Console.WriteLine($"Drone {drone.Name} starts");
                RunDrone(drone, tsc);
            }
            catch (Exception ex)
            {
                tsc.SetException(ex);
            }
        });

        return tsc.Task;
    }

    private static void RunDrone(Drone drone, TaskCompletionSource<string> tsc)
    {
        int currentCheckPoint = 0;

        void Continuation()
        {
            if (drone.Name == "Tiny Timmy" && currentCheckPoint == 5)
            {
                throw new Exception($"Drone {drone.Name} crashed before reaching checkpint {currentCheckPoint + 1}");
            }

            if (currentCheckPoint < drone.MaxCheckpoints)
            {
                var delayTask = Task.Delay(drone.DelayMs);

                var awaiter = delayTask.GetAwaiter();

                awaiter.OnCompleted(() =>
                {
                    Console.WriteLine($"Drone {drone.Name} has reached checkpoint {++currentCheckPoint}");
                    Continuation();
                });
            }
            else
            {
                Console.WriteLine($"Drone {drone.Name} has reached final destination.");
                tsc.SetResult(drone.Name);
            }

        }
        Continuation();
    }
}