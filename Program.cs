using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using UnitsNet;

namespace TravelingSalesman
{
    class Program
    {
        private static double PanelWidth = 1000;
        private static double PanelHeight = 1000;
        private static int GlassCount = 12;

        static void Main(string[] args)
        {
            var xAxisProperties = new KinematicProperties() { Acceleration = Acceleration.FromMillimetersPerSecondSquared(40), MaxSpeed = Speed.FromMillimetersPerSecond(50) };
            var yAxisProperties = new KinematicProperties() { Acceleration = Acceleration.FromMillimetersPerSecondSquared(80), MaxSpeed = Speed.FromMillimetersPerSecond(80) };
            var glasses = GetTwelveGlasses();
            var positions = TransormToPositions(glasses);
            var startPosition = positions[0];
            positions.Remove(startPosition);

            var startTime = DateTime.Now;
            Console.WriteLine("Start: "+ startTime);
            var (minLength, minPath) = PathSearch(glasses);
            var endTime = DateTime.Now;
            Console.WriteLine("End: " + endTime);
            var timeSpan = endTime - startTime;
            Console.WriteLine("Time: " + timeSpan);
            Console.WriteLine("MinLength: " + minLength);

            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++");
            
            startTime = DateTime.Now;
            Console.WriteLine("Start: " + startTime);
            var nextNearestFastestWay = NextNearest.GetFastestWayToGetDrunk(glasses);
            endTime = DateTime.Now;
            Console.WriteLine("End: " + endTime);
            timeSpan = endTime - startTime;
            Console.WriteLine("Time: " + timeSpan);
            Console.WriteLine("MinLength: " + nextNearestFastestWay.distance);

            Console.WriteLine($"Olaf's Path: {string.Join(",", minPath.Reverse())}");
            Console.WriteLine($"Micha's Path: {string.Join(",", nextNearestFastestWay.travelList.Select(p => p.PositionNumber).ToList())}");

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("+++++ Let's get to real world examples ++++++++");
            Console.WriteLine("+++++++++++ RealWorldBruteForce +++++++++++++");
            startTime = DateTime.Now;
            (List<Position> pos, Duration timeTravelled) resultBruteForce = RealWorldBruteForce.GetFastestWayToGetDrunk(startPosition, positions, xAxisProperties, yAxisProperties);
            endTime = DateTime.Now;
            timeSpan = endTime - startTime;
            Console.WriteLine("Time for calculation: " + timeSpan);
            Console.WriteLine($"Plaf's Path: {string.Join(",", resultBruteForce.pos.Select(p => p.Number).ToList())}");
            Console.WriteLine($"Time to travel: {resultBruteForce.timeTravelled.ToUnit(UnitsNet.Units.DurationUnit.Second)}");

            Console.WriteLine("+++++++++++ RealWorlNextNearest +++++++++++++");
            startTime = DateTime.Now;
            (List<Position> pos, Duration timeTravelled) resultNextNearest = RealWorldNextNearest.GetFastestWayToGetDrunk(startPosition, positions, xAxisProperties, yAxisProperties);
            endTime = DateTime.Now;
            timeSpan = endTime - startTime;
            Console.WriteLine("Time for calculation: " + timeSpan);
            Console.WriteLine($"Micha's Path: {string.Join(",", resultNextNearest.pos.Select(p => p.Number).ToList())}");
            Console.WriteLine($"Time to travel: {resultNextNearest.timeTravelled.ToUnit(UnitsNet.Units.DurationUnit.Second)}");
            Console.ReadKey();
        }

        private static List<Position> TransormToPositions((double XPos, double YPos, bool)[] glasses)
        {
            List<Position> positions = new List<Position>();
            int pos = 0;
            foreach (var glasPos in glasses)
            {
                Position item = new Position() with { Number = pos, X = Length.FromMillimeters(glasPos.XPos), Y = Length.FromMillimeters(glasPos.YPos) };
                positions.Add(item);
                pos++;
            }

            return positions;
        }

        private static (double, int[]) PathSearch((double, double, bool)[] glasses)
        {
            var currentPath = new Stack<int>();
            var minPath = new int[0];
            var minLength = double.MaxValue;


            PathSearch(0, 0, glasses, currentPath, ref minLength, ref minPath);
            minLength = Math.Sqrt(minLength);

            return (minLength, minPath);
        }

        private static void PathSearch(double currentLength, int fromGlass, (double, double, bool)[] glasses, Stack<int> currentStack, ref double minLength, ref int[] minPath)
        {
            for (int i = 1; i <= GlassCount; i++)
            {
                if (!glasses[i].Item3)
                {
                    var x1 = glasses[fromGlass].Item1;
                    var y1 = glasses[fromGlass].Item2;
                    var x2 = glasses[i].Item1;
                    var y2 = glasses[i].Item2;
                    var pathLength = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + currentLength;


                    if (pathLength < minLength)
                    {
                        currentStack.Push(i);
                        glasses[i].Item3 = true;
                        if (currentStack.Count < GlassCount)
                        {
                            PathSearch(pathLength, i, glasses, currentStack, ref minLength, ref minPath);
                        }
                        else
                        {

                            minLength = pathLength;
                            minPath = currentStack.ToArray();
                        }
                        glasses[i].Item3 = false;
                        currentStack.Pop();
                    }
                }
            }
        }

        private static (double, double, bool)[] GetTwelveGlasses()
        {
            var xStart = 0;
            var yStart = 0;
            var startPoint = new (double, double, bool)[] {(xStart, yStart, true)};

            var rand = new Random();
            var sequence = startPoint.Concat(Enumerable.Range(0, GlassCount).Select(i => (rand.NextDouble() * PanelWidth, rand.NextDouble() * PanelHeight, false))).ToArray();
            return sequence;
        }
    }
}
