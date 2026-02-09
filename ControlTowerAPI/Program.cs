
using ControlTowerAPI.Listener;

var tower = new ControlTower("http://localhost:6060");

try
{
    _ = tower.StartListener();
    Console.WriteLine("Press any key to stop burn down the tower...");
    Console.ReadKey(intercept: true);
}
finally
{
    tower.Stop();
}