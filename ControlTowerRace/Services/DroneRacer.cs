using System.Net.Cache;
using System.Net.Http.Json;
using System.Text.Json;
using DroneModel;

namespace ControlTowerRace.Services;

public static class DroneRacer
{

    private static readonly string _weatherEndpoint = "weather";
    private static readonly Dictionary<string, int> _weatherDelay = new()
    {
        ["clear"] = 0,
        ["wind"] = 50,
        ["storm"] = 300,
    };
    public static async Task RunDrone(HttpClient client, string droneName, int delayMs)
    {
        // Get Checkpoints
        int plan = await GetCheckpoints(client, droneName);

        for (int i = 0; i < plan; i++)
        {
            string weather = await GetWeather(client);

            if (!_weatherDelay.TryGetValue(weather, out int weatherDelay))
            {
                Console.WriteLine($"Unknown weather({weather}) encounterd, drone {droneName} stopped.");
                throw new TaskCanceledException("Unknown weather encountered!");
            }

            Console.WriteLine($"Drone {droneName} reached checkpoint {i + 1}, weather is {weather}");

            await Task.Delay(delayMs + weatherDelay);
        }
    }

    private static async Task<string> GetWeather(HttpClient client)
    {
        var response = await client.GetAsync(_weatherEndpoint);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<WeatherDto>()
            ?? throw new HttpRequestException("content is null");

        return content.Weather;
    }

    private static async Task<int> GetCheckpoints(HttpClient client, string name)
    {
        var response = await client.GetAsync(BuildQuery(name));
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var checkpoints = JsonSerializer.Deserialize<CheckpointsDto>(content);

        if (checkpoints is null)
            return 0;
        return checkpoints.Checkpoints;
    }

    private static string BuildQuery(string name)
    {
        return $"route?drone={name}";
    }
}

public record CheckpointsDto(int Checkpoints);
public record WeatherDto(string Weather);

