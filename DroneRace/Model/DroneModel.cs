namespace DroneRace.Model;

public record Drone
{
    public required string Name { get; set; }
    public required int MaxCheckpoints { get; set; }
    public required int DelayMs { get; set; }
}