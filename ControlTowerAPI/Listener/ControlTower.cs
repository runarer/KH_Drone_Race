

using System.Net;
using System.Text;
using ControlTowerAPI.Model;

namespace ControlTowerAPI.Listener;

public class ControlTower
{
    private HttpListener _listener;
    private Dictionary<string, Drone> _registredDrones;

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
                if (context.Request.HttpMethod == "GET")
                    await ProcessGetRequest(context);
                else
                    await ProcessBadRequest(context);
            }
            catch (HttpListenerException) { break; }
            catch (ArgumentException) { break; }
        }
    }

    private async Task ProcessBadRequest(HttpListenerContext context)
    {

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
                responseMessage = Encoding.UTF8.GetBytes($"'weather':'{GetWeather()}");
                response.ContentType = "application/json";
            }
            // /drone?=name
            else if (request.QueryString["drone"] is not null)
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
                    responseMessage = Encoding.UTF8.GetBytes($"'checkpoints':{drone.MaxCheckpoints}");
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

    private async Task<bool> RegisterDrone(Drone drone)
    {
        if (_registredDrones.ContainsKey(drone.Name))
            return false;
        _registredDrones[drone.Name] = drone;
        return true;
    }

    private string GetWeather()
    {
        return string.Empty;
    }

    private Drone? GetRoute(string name)
    {
        if (_registredDrones.TryGetValue(name, out Drone? drone))
            return drone;
        return null;
    }

}