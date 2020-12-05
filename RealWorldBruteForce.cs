using System.Collections.Generic;
using System.Linq;
using UnitsNet;

namespace TravelingSalesman
{
    public static class RealWorldBruteForce
    {
        public static (List<Position> travelList, Duration timeToTravel) GetFastestWayToGetDrunk(Position currentPosition, List<Position> positionsToTravelTo, KinematicProperties xAxis, KinematicProperties yAxis)
        {
            var currentPath = new Stack<int>();
            var minPath = new int[0];
            var minDuration = Duration.FromMilliseconds(double.MaxValue);

            var allPositions = new List<(Position Position, bool AlreadyTravelledTo)>() { (currentPosition, false) };
            allPositions.AddRange(positionsToTravelTo.Select(pos => (pos, false)).ToList());


            PathSearch(Duration.FromMilliseconds(0), 0, allPositions.ToArray(), currentPath, ref minDuration, ref minPath, xAxis, yAxis);

            var shortestWay = new List<Position>();
            foreach (var item in minPath.Reverse())
            {
                var pos = positionsToTravelTo.First(p => p.Number == item);
                shortestWay.Add(pos);
            }

            return (shortestWay, minDuration);
        }

        private static void PathSearch(Duration currentDuration, int fromGlass, (Position Position, bool AlreadyTravelledTo)[] glasses, Stack<int> currentStack, ref Duration minLength, ref int[] minPath, KinematicProperties xAxis, KinematicProperties yAxis)
        {
            var glassCount = glasses.Length - 1;
            for (int i = 1; i <= glassCount; i++)
            {
                if (!glasses[i].AlreadyTravelledTo)
                {
                    var xDistance = glasses[i].Position.X - glasses[fromGlass].Position.X;
                    var yDistance = glasses[i].Position.Y - glasses[fromGlass].Position.Y;

                    var timeX = MotionCalculatorHelper.GetTimeForDistance(xDistance, xAxis.Acceleration, xAxis.MaxSpeed);
                    var timeY = MotionCalculatorHelper.GetTimeForDistance(yDistance, yAxis.Acceleration, yAxis.MaxSpeed);

                    Duration timeForTravel = timeX > timeY ? timeX : timeY;
                    timeForTravel = timeForTravel + currentDuration;


                    if (timeForTravel < minLength)
                    {
                        currentStack.Push(i);
                        glasses[i].AlreadyTravelledTo = true;
                        if (currentStack.Count < glassCount)
                        {
                            PathSearch(timeForTravel, i, glasses, currentStack, ref minLength, ref minPath, xAxis, yAxis);
                        }
                        else
                        {

                            minLength = timeForTravel;
                            minPath = currentStack.ToArray();
                        }
                        glasses[i].AlreadyTravelledTo = false;
                        currentStack.Pop();
                    }
                }
            }
        }
    }
}
