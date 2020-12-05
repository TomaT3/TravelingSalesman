using System.Collections.Generic;
using System.Linq;
using UnitsNet;

namespace TravelingSalesman
{
    public static class RealWorldNextNearest
    {
        public static (List<Position> travelList, Duration timeToTravel) GetFastestWayToGetDrunk(Position currentPosition, List<Position> positionsToTravelTo, KinematicProperties xAxis, KinematicProperties yAxis)
        {
            List<Position> getDrunkFastWayToTravel = new();
            Duration travelledTime = Duration.FromMilliseconds(0);

            while (positionsToTravelTo.Any())
            {
                var result = GetNextFastestReachablePosition(currentPosition, positionsToTravelTo, xAxis, yAxis);
                currentPosition = result.nextPosition;
                travelledTime += result.timeToTravel;
                positionsToTravelTo.Remove(currentPosition);
                getDrunkFastWayToTravel.Add(currentPosition);
            }

            return (getDrunkFastWayToTravel, travelledTime);
        }

        private static (Position nextPosition, Duration timeToTravel) GetNextFastestReachablePosition(Position currentPosition, List<Position> positionsToTravelTo, KinematicProperties xAxis, KinematicProperties yAxis)
        {
            bool firstIteration = true;
            Duration fastestWay = default(Duration);
            Position nextPosition = null;
            foreach (var position in positionsToTravelTo)
            {
                var xDistance = currentPosition.X - position.X;
                var yDistance = currentPosition.Y - position.Y;

                var timeX = MotionCalculatorHelper.GetTimeForDistance(xDistance, xAxis.Acceleration, xAxis.MaxSpeed);
                var timeY = MotionCalculatorHelper.GetTimeForDistance(yDistance, yAxis.Acceleration, yAxis.MaxSpeed);

                Duration timeForTravel = timeX > timeY ? timeX : timeY;

                if (timeForTravel < fastestWay
                    || firstIteration)
                {
                    firstIteration = false;
                    fastestWay = timeForTravel;
                    nextPosition = position;
                }
            }

            return (nextPosition, fastestWay);
        }
    }
}
