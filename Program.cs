using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace TravelingSalesman
{
    class Program
    {
        private static double PanelWidth = 1000;
        private static double PanelHeight = 1000;
        private static int GlassCount = 12;

        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            Console.WriteLine("Start: "+ startTime);
            var glasses = GetTwelveGlasses();
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

            Console.ReadKey();
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

                    currentStack.Push(i);
                    glasses[i].Item3 = true;
                    if (currentStack.Count < GlassCount)
                    {
                        PathSearch(pathLength, i, glasses, currentStack, ref minLength, ref minPath);
                    }
                    else
                    {
                        if (pathLength < minLength)
                        {
                            minLength = pathLength;
                            minPath = currentStack.ToArray();
                        }
                    }
                    glasses[i].Item3 = false;
                    currentStack.Pop();
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
