using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{

    public record PositionWithoutUnit 
    {
        public int PositionNumber { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsFilled { get; set; }

        public PositionWithoutUnit(int positionNumber, double x, double y, bool isFilled) => (PositionNumber, X, Y, IsFilled) = (positionNumber, x, y, isFilled);
    }

    public static class NextNearest
    {

        public static (List<PositionWithoutUnit> travelList, double distance) GetFastestWayToGetDrunk((double x, double y, bool isFilled)[] positions)
        {
            PositionWithoutUnit currentPosition = new PositionWithoutUnit(0, positions[0].x, positions[0].y, false);
            List<PositionWithoutUnit> positionsToTravel  = new();
            List<PositionWithoutUnit> getDrunkFastWayToTravel = new();
            double travelledDistance = 0;

            for (int i = 1; i < positions.Length; i++)
            {
                positionsToTravel.Add(new PositionWithoutUnit(i, positions[i].x, positions[i].y, positions[i].isFilled));
            }


            while (positionsToTravel.Any())
            {
                var result = GetNextFastestReachablePosition(currentPosition, positionsToTravel);
                currentPosition = result.nextPosition;
                travelledDistance += result.wayToTravel;
                positionsToTravel.Remove(currentPosition);
                getDrunkFastWayToTravel.Add(currentPosition);
            }

            return (getDrunkFastWayToTravel, Math.Sqrt(travelledDistance)); 
        }

        private static (PositionWithoutUnit nextPosition, double wayToTravel) GetNextFastestReachablePosition(PositionWithoutUnit currentPosition, List<PositionWithoutUnit> positionsToTravel)
        {
            var shortestWay = double.MaxValue;
            PositionWithoutUnit nextPosition = null;
            foreach(var position in positionsToTravel)
            {
                var distance = (currentPosition.X - position.X) * (currentPosition.X - position.X) + (currentPosition.Y - position.Y) * (currentPosition.Y - position.Y);
                if (distance < shortestWay)
                {
                    shortestWay = distance;
                    nextPosition = position;
                }
            }

            return (nextPosition, shortestWay);
        }
    }
}
