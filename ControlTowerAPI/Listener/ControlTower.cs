

using System.Net;
using System.Text;
using System.Text.Json;
using ControlTowerAPI.Model;

namespace ControlTowerAPI.Listener;

public class ControlTower
{
    private HttpListener _listener;
    private Dictionary<string, Drone> _registredDrones = [];

    private Random _randomizer = new();

    public ControlTower(string uriPrefix)
    {
        _listener = new();
        _listener.Prefixes.Add(uriPrefix);
    }

    public async Task StartListener()
    {
        _listener.Start();

        while (true)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                switch (context.Request.HttpMethod)
                {
                    case "GET":
                        await ProcessGetRequest(context);
                        break;
                    case "POST":
                        await ProcessPostRequest(context);
                        break;
                    default:
                        await ProcessBadRequest(context);
                        break;
                }
            }
            catch (HttpListenerException) { break; }
            catch (ArgumentException) { break; }
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }

    private async Task ProcessBadRequest(HttpListenerContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        byte[] message = Encoding.UTF8.GetBytes("Request not supported");
        context.Response.ContentType = "text/plain";
        context.Response.ContentLength64 = message.Length;
        using var output = context.Response.OutputStream;
        await output.WriteAsync(message);
    }

    private async Task ProcessGetRequest(HttpListenerContext context)
    {
        try
        {
            var request = context.Request;
            var response = context.Response;

            byte[] responseMessage;

            // /weather
            if (request.Url is not null && request.Url.AbsolutePath == "/weather")
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                responseMessage = Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(new { Weather = GetWeather() })
                );
                response.ContentType = "application/json";
            }
            // /drone?=name
            else if (request.Url is not null && request.Url.AbsolutePath == "/route" && request.QueryString["drone"] is not null)
            {
                string droneName = request.QueryString["drone"]!;
                Drone? drone = GetRoute(droneName);

                // drone not registered
                if (drone is null)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseMessage = Encoding.UTF8.GetBytes($"No drone with name {droneName} found!");
                    response.ContentType = "text/plain";
                }
                // return checkpoints
                else
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    // responseMessage = Encoding.UTF8.GetBytes($"{{'checkpoints':{drone.MaxCheckpoints}'}}");
                    responseMessage = Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(new { Checkpoints = drone.MaxCheckpoints })
                    );
                    response.ContentType = "application/json";
                }
            }
            // Bad get request
            else
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseMessage = Encoding.UTF8.GetBytes("Request not supported");
                response.ContentType = "text/plain";
            }
            response.ContentLength64 = responseMessage.Length;
            using var output = response.OutputStream;
            await output.WriteAsync(responseMessage);
        }
        catch (ArgumentNullException) { throw; }
    }

    private async Task ProcessPostRequest(HttpListenerContext context)
    {
        try
        {
            var request = context.Request;
            var response = context.Response;

            string responseMessage = string.Empty;

            if (!request.HasEntityBody)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseMessage = "Cannot process request, no request body";
            }
            else
            {
                string body = await ReadPostBody(request);
                try
                {
                    Drone? drone = JsonSerializer.Deserialize<Drone>(body);
                    if (drone is null)
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseMessage = "Body could not be understod";
                    }
                    else
                    {
                        var (code, msg) = RegisterDrone(drone);
                        response.StatusCode = (int)code;
                        responseMessage = msg;
                    }
                }
                catch (JsonException)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseMessage = "Jason did not play by the rules!";
                }
                catch (ArgumentNullException)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseMessage = "Json could not be understood";
                }
            }

            byte[] responseMessageEncoded = Encoding.UTF8.GetBytes(responseMessage);

            response.ContentLength64 = responseMessageEncoded.Length;

            using var output = response.OutputStream;
            await output.WriteAsync(responseMessageEncoded);

        }
        catch (ArgumentNullException) { throw; }
    }

    private static async Task<string> ReadPostBody(HttpListenerRequest request)
    {
        using Stream body = request.InputStream;
        using StreamReader reader = new(body, request.ContentEncoding);
        return await reader.ReadToEndAsync();
    }

    private (HttpStatusCode, string) RegisterDrone(Drone drone)
    {
        if (drone.MaxCheckpoints < 0)
            return (HttpStatusCode.UnprocessableContent, "Cannot use negative number of checkpoints");
        if (drone.DelayMs < 0)
            return (HttpStatusCode.UnprocessableContent, "Cannot use negative delay, time moves forward!");
        if (_registredDrones.ContainsKey(drone.Name))
            return (HttpStatusCode.Conflict, "Drone with same name already registered");
        _registredDrones[drone.Name] = drone;
        return (HttpStatusCode.Created, drone.Name);
    }

    private string GetWeather()
    {
        string[] weather = ["clear", "wind", "storm"];
        return weather[_randomizer.Next(0, weather.Length)];
    }

    private Drone? GetRoute(string name)
    {
        if (_registredDrones.TryGetValue(name, out Drone? drone))
            return drone;
        return null;
    }

}