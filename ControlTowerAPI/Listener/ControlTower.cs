

using System.Net;
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
                if (context.Request.HttpMethod != "GET")
                    await ProcessBadRequest(context);
                else
                    await ProcessGetRequest(context);
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

    }

    private async Task<bool> RegisterDrone(Drone drone)
    {
        if (_registredDrones.ContainsKey(drone.Name))
            return false;
        _registredDrones[drone.Name] = drone;
        return true;
    }

    private async Task<string> GetWeather(int checkpoint)
    {
        return string.Empty;
    }

    private async Task<Drone?> GetRoute(string name)
    {
        if (_registredDrones.TryGetValue(name, out Drone? drone))
            return drone;
        return null;
    }

}