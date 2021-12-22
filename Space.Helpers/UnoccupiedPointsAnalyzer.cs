using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Space.Helpers
{
    public class UnoccupiedPointsAnalyzer
    {
        private static List<Point> freePoints;

        static UnoccupiedPointsAnalyzer()
        {
            freePoints = new List<Point>();

            for(int i = 0; i < 40; i++)
            {
                for(int j = 0; j < 40; j++)
                {
                    freePoints.Add(new Point { X = i, Y = j });
                }
            }
        }

        public static List<Point> GetUnoccupiedPoints(List<Point> usedPoints)
        {
            if(usedPoints.Count > 0)
            {
                var uniqueValues = freePoints.Where(x => !usedPoints.Contains(x)).ToList();
                freePoints = uniqueValues;
            }

            return freePoints;
        }
    }
}
