using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{

    public record Position 
    {
        public int PositionNumber { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsFilled { get; set; }

        public Position(int positionNumber, double x, double y, bool isFilled) => (PositionNumber, X, Y, IsFilled) = (positionNumber, x, y, isFilled);
    }

    public static class NextNearest
    {

        public static (List<Position> travelList, double distance) GetFastestWayToGetDrunk((double x, double y, bool isFilled)[] positions)
        {
            Position currentPosition = new Position(0, positions[0].x, positions[0].y, false);
            List<Position> positionsToTravel  = new();
            List<Position> getDrunkFastWayToTravel = new();
            double travelledDistance = 0;

            for (int i = 1; i < positions.Length; i++)
            {
                positionsToTravel.Add(new Position(i, positions[i].x, positions[i].y, positions[i].isFilled));
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

        private static (Position nextPosition, double wayToTravel) GetNextFastestReachablePosition(Position currentPosition, List<Position> positionsToTravel)
        {
            var shortestWay = double.MaxValue;
            Position nextPosition = null;
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
