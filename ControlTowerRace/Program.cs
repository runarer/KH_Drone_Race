
using DroneModel;
using ControlTowerRace.Services;

var client = new HttpClient
{
    BaseAddress = new Uri("http://localhost:6060/")
};

Drone[] drones =
[
    new("Big Berta",12,15),
    new("Tiny Timmy",18,10),
    new("Robust Roberta",9,20),
    new("Quick Quin",10,18),
    new("Two Ton Tony",15,12),
];

List<Task> tasks = [.. drones.Select(drone => DroneRegistrer.RegisterDrone(client, drone))];

await Task.WhenAll(tasks);

Console.WriteLine("All drones registered.");