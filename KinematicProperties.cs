using UnitsNet;

namespace TravelingSalesman
{
    public record KinematicProperties
    {
        public Acceleration Acceleration { get; init; }
        public Speed MaxSpeed { get; set; }
    }
}
