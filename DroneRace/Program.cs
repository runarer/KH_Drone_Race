
using DroneModel;
using DroneRace.Services;

// Drone[] drones =
// [
//     new(){Name = "Big Berta", MaxCheckpoints = 12, DelayMs = 15},
//     new(){Name = "Tiny Timmy", MaxCheckpoints = 18, DelayMs = 10}
//         // new("Robust Roberta",9,20),
//         // new("Quick Quin",10,18),
//         // new("Two Ton Tony",15,12),
//     ];


Drone[] drones = [new("Timmy", 12, 15), new("Bertha", 18, 10)];

ThreadRace.RaceDronesUsingThreads(drones);

TaskRace.RaceDronesUsingTasks(drones).Wait();

await AsyncRace.RaceDronesUsingAsync(drones);


try
{
    TaskRaceWithCrash.RaceDronesUsingTasks(drones).Wait();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


try
{
    await AsyncRace.RaceDronesUsingAsync(drones);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
