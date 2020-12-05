using UnitsNet;

namespace TravelingSalesman
{
    public record Position
    {
        public int Number { get; init; }
        public Length X { get; init; }
        public Length Y { get; init; }
    }
}
