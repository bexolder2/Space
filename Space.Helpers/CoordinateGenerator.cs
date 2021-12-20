using Space.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Space.Helpers
{
    public class CoordinateGenerator : IGenerator<Point>
    {
        private const int minValue = 0;

        public Point Generate(List<Point> args)
        {
            Point result = new Point();
            var freePoints = FreePointsAnalyzer.GetFreePoints(args);
            Random rnd = new Random();

            int index = rnd.Next(minValue, freePoints.Count);
            result = freePoints[index];

            return result;
        }
    }
}
