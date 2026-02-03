
using DroneRace.Model;
using DroneRace.Services;

Drone[] drones =
{
    new("Big Berta",12,15),
    new("Tiny Timmy",18,10),
    // new("Robust Roberta",9,20),
    // new("Quick Quin",10,18),
    // new("Two Ton Tony",15,12),
};

ThreadRace.RaceDronesUsingThreads(drones);