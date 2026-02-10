
namespace DroneModel;

// public class Drone
// {
//     public required string Name { get; set; }
//     public required int MaxCheckpoints { get; set; }
//     public required int DelayMs { get; set; }
// };


public class Drone(string name, int maxCheckpoints, int delayMs)
{
    public string Name { get; set; } = name;
    public int MaxCheckpoints { get; set; } = maxCheckpoints;
    public int DelayMs { get; set; } = delayMs;
};