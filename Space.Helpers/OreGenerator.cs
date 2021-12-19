using Space.Helpers.Interfaces;
using System;

namespace Space.Helpers
{
    public class OreGenerator : IGenerator<int>
    {
        private const int minValue = 100;
        private const int maxValue = 1000;

        public int Generate()
        {
            Random rnd = new Random();
            return rnd.Next(minValue, maxValue);
        }
    }
}
