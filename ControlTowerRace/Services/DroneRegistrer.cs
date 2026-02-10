
using System.Text;
using System.Text.Json;
using DroneModel;

namespace ControlTowerRace.Services;

public static class DroneRegistrer
{

    public static async Task RegisterDrone(HttpClient client, Drone drone)
    {
        var endpoint = client.BaseAddress;

        string droneAsJson = JsonSerializer.Serialize(drone);

        var content = new StringContent(droneAsJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(endpoint, content);

        response.EnsureSuccessStatusCode();

        Console.WriteLine($"Drone {drone.Name} was succesfully registered.");
    }

}